using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace Assembler
{
    /**
     * Parses the source code.
     */
    class Parser
    {
        /**
         * The parser's list of directives.
         */
        Directives directiveList;

        /**
         * The parser's list of instructions.
         */
        Instructions instructionList;

        /**
         * The current value of the location counter.
         */
        public static string LC;

        /**
         * The total count of errors in this program.
         */
        private static int totalErrors = 0;
        public static int TotalErrors {
            get { return Parser.totalErrors; }
            set { Parser.totalErrors = value; }
        }

        /**
         * Creates a Parser with location counter set to 0, ready to parse a single soure file.
         *
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 8, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public Parser()
        {
            Logger.Log("Creating Parser object.", "Parser");
            directiveList = Directives.GetInstance();
            instructionList = Instructions.GetInstance();
            LC = "0";
        }

        /**
         * Parses a single line of source code.
         *
         * @param line current line to parse
         * @param lineNum line number of current line
         * @param symb symbol table reference
         * @return the line to be parsed as a single line in the intermediate file
         *
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 8, 2011
         * @modlog
         *  - April  9, 2011 - Mark - ParseLine parses lines with instructions.
         *  - April  9, 2011 - Mark - ParseLine parses lines with directives.
         *  - April  9, 2011 - Mark - Increments program counter after parsing instructions.
         *  - April  9, 2011 - Mark - Sets OpCategory = "_ERROR" if the token expected to be
         *      an instruction category or a directive is niether of those.
         *  - April 10, 2011 - Mark - Properly handles lines that only have a comment.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private IntermediateLine ParseLine(string line, short lineNum, ref SymbolTable symb)
        {
            Logger.Log("Parsing line " + lineNum, "Parser");
            string token = "";
            Tokenizer.TokenKinds tokenKind = Tokenizer.TokenKinds.Empty;

            // make Intermediate version of this line to be returned
            IntermediateLine interLine = new IntermediateLine(line, lineNum);

            // check for a label at the beginning of the line
            if (line.Trim().Length > 0)
            {
                if (char.IsLetter(line[0]))
                {
                    Logger.Log(String.Format("Line {0} has a label, parsing label", lineNum), "Parser");
                    Tokenizer.GetNextToken(ref line, out token, out tokenKind);
                    if (tokenKind == Tokenizer.TokenKinds.Label_Or_Command
                        && (2 <= token.Length && token.Length <= 32))
                    {
                        interLine.Label = token;
                        if (symb.ContainsSymbol(token))
                        {
                            if (symb.GetSymbol(token).usage != Usage.ENTRY)
                            {
                                // error: cannot redefine symbol
                                interLine.AddError(Errors.Category.Warning, 4);
                            }
                        }
                        else
                        {
                            // the symbol is not defined, define it
                            symb.AddSymbol(token, LC, Usage.LABEL);
                        }
                    }
                    else
                    {
                        interLine.Label = "_ERROR";
                        interLine.AddError(Errors.Category.Serious, 2);
                    }
                }
                else if (!char.IsWhiteSpace(line[0]) && line[0] != ':')
                {
                    // invalid label start
                    interLine.Label = "_ERROR";
                    interLine.AddError(Errors.Category.Serious, 2);
                }
            }

            // Get the next token, which will specify whether or not the line has a
            // directive or an instruction.
            Tokenizer.GetNextToken(ref line, out token, out tokenKind);

            if (tokenKind == Tokenizer.TokenKinds.Label_Or_Command)
            {
                // coerce into uppercase
                token = token.ToUpper();
                // instruction
                if (instructionList.IsGroup(token))
                {
                    interLine.OpCategory = token;
                    ParseInstruction(ref line, ref interLine, ref symb);
                    interLine.ProgramCounter = LC;
                    IncrementLocationCounter();
                }
                // directive
                else if (directiveList.Contains(token))
                {
                    interLine.Directive = token;
                    DirectiveParser.ParseDirective(ref line, ref interLine, ref symb);
                }
                else
                {
                    // bad category, but don't NOP
                    interLine.OpCategory = "_ERROR";
                    interLine.AddError(Errors.Category.Serious, 1);
                    return interLine;
                }
            }
            else if (tokenKind == Tokenizer.TokenKinds.Comment)
            {
                interLine.Comment = token;
                return interLine;
            }
            else if (tokenKind != Tokenizer.TokenKinds.Empty)
            {
                // garbage data
                interLine.AddError(Errors.Category.Serious, 18);
                return interLine;
            }

            // If there's anything else, get it. If there's anything there,
            // it should be a comment.
            Tokenizer.GetNextToken(ref line, out token, out tokenKind);

            if (tokenKind == Tokenizer.TokenKinds.Comment)
            {
                interLine.Comment = token;
            }
            else if (tokenKind != Tokenizer.TokenKinds.Empty)
            {
                // error: invalid input after the operand
                Logger.Log("ERROR: EW.6 encountered", "Parser");
                interLine.AddError(Errors.Category.Warning, 6);
            }

            // process this line if it's an instruction
            if (interLine.OpCategory != null)
            {
                interLine.ProcessLine(ref symb);
            }

            Logger.Log("Finished parsing line " + lineNum, "Parser");

            return interLine;
        }

        /**
         * Parses the operation section of the line if it has an instruction.
         *
         * @param line current line to parse.
         * @param interLine the line as a single line in the intermediate file.
         * @param symb symbol table reference
         *
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 9, 2011
         * @modlog
         *  - April  9, 2011 - Mark - ParseInstruction properly parses instructions.
         *  - April  9, 2011 - Mark - Uses new ParseLiteralOperand format.
         *  - April  9, 2011 - Mark - Changed to use ValidOperandField.
         *  - April 10, 2011 - Mark - Handles CLRD and CLRT.
         *  - April 12, 2011 - Jacob - Factor out operand parsing.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private void ParseInstruction(ref string line, ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing instruction on line " + interLine.SourceLineNumber, "Parser");

            string token = "";
            Tokenizer.TokenKinds tokenKind = Tokenizer.TokenKinds.Empty;

            // checks for valid operands in all functions besides CLRD and CLRT
            if (OperandParser.ValidOperandField(line))
            {
                // get what should be the function name
                Tokenizer.GetNextToken(ref line, out token, out tokenKind);

                if (instructionList.IsInstruction(interLine.OpCategory, token))
                {
                    interLine.OpFunction = token;
                }
                else
                {
                    interLine.OpFunction = "_ERROR";
                }

                // get what should be the function operand
                OperandParser.ParseOperand(ref line, ref interLine, ref symb, 10);
            }
            else if (interLine.OpCategory.ToUpper() == "CNTL")
            {
                // token should be either CLRD or CLRT
                Tokenizer.GetNextToken(ref line, out token, out tokenKind);

                if (token.ToUpper() != "CLRD" && token.ToUpper() != "CLRT")
                {
                    interLine.OpOperand = "_ERROR";
                }

                interLine.OpFunction = token;
            }
            else
            {
                interLine.OpFunction = "_ERROR";
                interLine.OpOperand = "_ERROR";
            }

            Logger.Log("Finished parsing instruction on line " + interLine.SourceLineNumber, "Parser");
        }

        /**
         * Adds one to the location counter.
         * 
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 9, 2011
         * @modlog
         *  - April 9, 2011 - Mark - Correctly increments the location counter by 1.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void IncrementLocationCounter()
        {
            Logger.Log("Incrementing LC from: " + LC, "Parser");
            int tempLC = Convert.ToInt32(LC, 16);
            tempLC++;
            LC = Convert.ToString(tempLC, 16).ToUpper();
            Logger.Log("LC incremented to: " + LC, "Parser");
        }

        /**
         * Parses an entire source code file.
         *
         * @param path the path of the source file to parse.
         * @param interSource resultant intermediate file from this source
         * @param symb resultant symbol table from this source
         *
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 8, 2011
         * @modlog
         *  - April 9, 2011 -  Mark - Parses entire source code file from a string.
         *  - April 9, 2011 -  Mark - Reads source file in from a file.
         *  - April 9, 2011 -  Mark - Catches exceptions possibly raised by reading the file.
         *  - April 9, 2011 - Jacob - Adds symbols to the symbol table. Changed the parameters to 
         *      pass in an IntermediateFile and SymbolTable with an expected out value. Return type
         *      is now void.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void ParseSource(string path, out IntermediateFile interSource, out SymbolTable symb)
        {
            string[] sourceCode = new string[1];
            try
            {
                Logger.Log("Opening file: " + path, "Parser");
                sourceCode = File.ReadAllLines(path);
            }
            catch (FileNotFoundException ex)
            {
                Logger.Log("Failed to open file. Error: " + ex.Message, "Parser");
                Console.WriteLine("{0}\n{1}", ex.Message, "Exiting program");
                System.Environment.Exit(1);
            }

            interSource = new IntermediateFile();
            symb = new SymbolTable();

            // first line is expected to hold the start directive
            IntermediateLine line = ParseLine(sourceCode[0], 1, ref symb);
            interSource.AddLine(line);
            if (line.Directive.ToUpper() != "START")
            {
                // file must start with a valid start directive
                line.AddError(Errors.Category.Fatal, 2);
                return;
            }
            else if (HasFatalError(line))
            {
                return;
            }

            bool reachedEnd = false;

            // iterate the lines of the file
            for (short i = 2; i <= sourceCode.Length; i++)
            {
                // parse a line and create an intermediate version
                line = ParseLine(sourceCode[i - 1], i, ref symb);

                // if we're at the end and processing another line
                if (reachedEnd)
                {
                    line.AddError(Errors.Category.Fatal, 7);
                }

                // check the LC. at this point, the line will have incremented
                // already, so check if it's 1024 instead of 1023.
                if (Convert.ToInt32(Parser.LC, 16) > 1024)
                {
                    line.AddError(Errors.Category.Fatal, 3);
                }

                // add to source
                interSource.AddLine(line);

                // did we find le end?
                if (line.Directive == "END")
                {
                    reachedEnd = true;
                }

                // check for fatal errors
                if (HasFatalError(line))
                {
                    break;
                }
            }
        }

        /**
         * Checks the specified IntermediateLine to see if it has a fatal error.
         * This is used to determine whether or not the assembly should continue
         * after a line has been parsed.
         *
         * @param line the line to check for a fatal error
         * @return true if the indicated line has a fatal error, false otherwise
         *
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private bool HasFatalError(IntermediateLine line)
        {
            // check for fatal errors
            List<Errors.Error> errors = line.GetThreeErrors();
            bool hasFatal = false;

            foreach (Errors.Error e in errors)
            {
                if (e.category == Errors.Category.Fatal)
                {
                    hasFatal = true;
                    break;
                }
            }
            return hasFatal;
        }
    }
}
