using System;
using System.IO;
using System.Diagnostics;

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
         * Creates a Parser with location counter set to 0, ready to parse a single soure file.
         */
        public Parser()
        {
            Logger288.Log("Creating Parser object.", "Parser");
            directiveList = Directives.GetInstance();
            instructionList = Instructions.GetInstance();
            LC = "0";
        }

        /**
         * Parses a single line of source code.
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
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
         * 
         * @param line current line to parse
         * @param lineNum line number of current line
         * @return the line to be parsed as a single line in the intermediate file
         */
        private IntermediateLine ParseLine(string line, short lineNum, ref SymbolTable symb)
        {
            Logger288.Log("Parsing line " + lineNum, "Parser");
            string token = "";
            Tokenizer.TokenKinds tokenKind = Tokenizer.TokenKinds.Empty;

            // make Intermediate version of this line to be returned
            IntermediateLine interLine = new IntermediateLine(line, lineNum);

            // check for a label at the beginning of the line
            if (line.Length > 0 && char.IsLetter(line[0]))
            {
                Logger288.Log(String.Format("Line {0} has a label, parsing label", lineNum), "Parser");
                Tokenizer.GetNextToken(ref line, out token, out tokenKind);
                if (tokenKind == Tokenizer.TokenKinds.Label_Or_Command
                    && (2 <= token.Length && token.Length <= 32))
                {
                    interLine.Label = token;
                    symb.AddSymbol(token, LC, Usage.ENTRY);
                }
                else
                {
                    interLine.Label = "_ERROR";
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
                    ParseInstruction(ref line, ref interLine);
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
                    interLine.OpCategory = "_ERROR";
                }
            }
            else if (tokenKind == Tokenizer.TokenKinds.Comment)
            {
                interLine.Comment = token;
                return interLine;
            }

            // If there's anything else, get it. If there's anything there,
            // it should be a comment.
            Tokenizer.GetNextToken(ref line, out token, out tokenKind);

            if (tokenKind == Tokenizer.TokenKinds.Comment)
            {
                interLine.Comment = token;
            }

            Logger288.Log("Finished parsing line " + lineNum, "Parser");

            return interLine;
        }

        /**
         * Parses the operation section of the line if it has an instruction.
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         *  - April  9, 2011 - Mark - ParseInstruction properly parses instructions.
         *  - April  9, 2011 - Mark - Uses new ParseLiteralOperand format.
         *  - April  9, 2011 - Mark - Changed to use ValidOperandField.
         *  - April 10, 2011 - Mark - Handles CLRD and CLRT.
         *  - April 12, 2011 - Jacob - Factor out operand parsing.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         * 
         * @param line current line to parse.
         * @param interLine the line as a single line in the intermediate file.
         */
        private void ParseInstruction(ref string line, ref IntermediateLine interLine)
        {
            Logger288.Log("Parsing instruction on line " + interLine.SourceLineNumber, "Parser");

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
                OperandParser.ParseOperand(ref line, ref interLine);
            }
            else if (interLine.OpCategory.ToUpper() == "CNTL")
            {
                // token should be either CLRD or CLRT
                Tokenizer.GetNextToken(ref line, out token, out tokenKind);

                if (token.ToUpper().Equals("CLRD") || token.ToUpper().Equals("CLRT"))
                {
                    interLine.OpFunction = token;
                }
                else
                {
                    interLine.OpFunction = "_ERROR";
                    interLine.OpOperand = "_ERROR";
                }
            }
            else
            {
                interLine.OpFunction = "_ERROR";
                interLine.OpOperand = "_ERROR";
            }

            Logger288.Log("Finished parsing instruction on line " + interLine.SourceLineNumber, "Parser");
        }

        /**
         * Adds one to the location counter.
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         *  - April 9, 2011 - Mark - Correctly increments the location counter by 1.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private void IncrementLocationCounter()
        {
            Logger288.Log("Incrementing LC from: " + LC, "Parser");
            int tempLC = Convert.ToInt32(LC, 16);
            tempLC++;
            LC = Convert.ToString(tempLC, 16).ToUpper();
            Logger288.Log("LC incremented to: " + LC, "Parser");
        }

        /**
         * Parses an entire source code file.
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
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
         * 
         * @param path the path of the source file to parse.
         * @param interSource resultant intermediate file from this source
         * @param symb resultant symbol table from this source
         */
        public void ParseSource(string path, out IntermediateFile interSource, out SymbolTable symb)
        {
            string[] sourceCode = new string[1];
            try
            {
                Logger288.Log("Opening file: " + path, "Parser");
                sourceCode = File.ReadAllLines(path);
            }
            catch (FileNotFoundException ex)
            {
                Logger288.Log("Failed to open file. Error: " + ex.Message, "Parser");
                Console.WriteLine("{0}\n{1}", ex.Message, "Exiting program");
                System.Environment.Exit(1);
            }


            symb = new SymbolTable();

            // first line is expected to hold the start directive
            IntermediateLine line = ParseLine(sourceCode[0], 1, ref symb);

            interSource = new IntermediateFile(line.Label);
            interSource.AddLine(line);

            // iterate the lines of the file
            for (short i = 2; i <= sourceCode.Length; i++)
            {
                // parse a line and create an intermediate version
                line = ParseLine(sourceCode[i - 1], i, ref symb);
                interSource.AddLine(line);
            }
        }
    }
}
