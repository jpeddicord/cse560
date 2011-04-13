using System;
using System.IO;
using System.Diagnostics;

namespace Assembler
{

    class Parser
    {
        /**
         * DOCUMENT MEEEEEE
         */
        public enum OperandType {
            elabel,
            mlabel,
            rlabel,
            olabel,
            n,
            nn,
            nnn,
            nmax,
            nnnnnn,
            none
        }

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
        string LC;

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
        private IntermediateLine ParseLine(string line, short lineNum)
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
                    ParseDirective(ref line, ref interLine);
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
            if (ValidOperandField(line))
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
                ParseOperand(ref line, ref interLine);
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
         * Parses the operation section of the line if it has an instruction.
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         *  - April 9, 2011 - Mark - ParseDirective properly parses directives.
         *  - April 9, 2011 - Mark - Uses new ParseLiteralOperand format.
         *  - April 12, 2011 - Jacob - Factor out operand parsing.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         * 
         * @param line current line to parse.
         * @param interLine the line as a single line in the intermediate file.
         */
        private void ParseDirective(ref string line, ref IntermediateLine interLine)
        {
            Logger288.Log("Parsing directive on line " + interLine.SourceLineNumber, "Parser");

            // NOP doesn't have an operand
            if (interLine.Directive == "NOP")
            {
                // TODO: insert and "parse" a MOPER ADD,=0, and increment LC
                return;
            }

            Logger288.Log("Parsing directive operand on line " + interLine.SourceLineNumber, "Parser");

            // get the operand of the directive
            ParseOperand(ref line, ref interLine);

            // RESET directive sets the LC
            if (interLine.Directive == "RESET")
            {
                //this.LC;
            }

            Logger288.Log("Finished parsing directive on line " + interLine.SourceLineNumber, "Parser");
        }

        /**
         * Parse an operand and fill the intermediate line with found data.
         * XXX: DOC
         */
        private void ParseOperand(ref string line, ref IntermediateLine interLine)
        {
            // get the next token
            string token;
            Tokenizer.TokenKinds tokenKind;
            Tokenizer.GetNextToken(ref line, out token, out tokenKind);

            string operand;
            string litOperand = null;

            // if it's a label, then just set the token directly
            if (tokenKind == Tokenizer.TokenKinds.Label_Or_Command)
            {
                operand = token;
            }
            // if it's a number, convert and store
            else if (tokenKind == Tokenizer.TokenKinds.Number)
            {
                operand = Convert.ToString(int.Parse(token), 16).ToUpper();
            }
            // do further processing for literals
            else if (tokenKind == Tokenizer.TokenKinds.Literal)
            {
                ParseLiteralOperand(token, out operand, out litOperand);
            }
            // anything else is invalid
            else
            {
                operand = "_ERROR";
            }

            // assign the directive fields if it's a directive
            if (interLine.Directive != null)
            {
                interLine.DirectiveOperand = operand;
                interLine.DirectiveLitOperand = litOperand;
            }
            // otherwise fill the default operands
            else
            {
                interLine.OpOperand = operand;
                interLine.OpLitOperand = litOperand;
            }
        }

        /**
         * Parses a literal operand, e.g. X=, B=, I=, C=
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         *  - April  9, 2011 -  Mark - Parses hex and binary literals.
         *  - April  9, 2011 -  Mark - Parameters changed. Return type changed to void.
         *  - April  9, 2011 -  Mark - Now parses integer literals.
         *  - April 10, 2011 - Jacob - Parses single character in accordance with instruction limits.
         *  - April 11, 2011 -  Mark - Checks that integer literals are in the proper range.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         * 
         * @param inOper the operand to parse
         * @param outOper the numerical operand from the literal, that is, the part after the X=
         *                  will be in hexadecimal
         * @param litType the type of the operand, that is, X, B, etc.
         */
        private void ParseLiteralOperand(string inOper, out string outOper, out string litType)
        {
            Logger288.Log("Parsing literal operand " + inOper, "Parser");

            int op;
            outOper = inOper.Substring(2);
            litType = inOper.Substring(0, 2);

            switch (inOper[0])
            {
                case 'X':
                case 'x':
                    {
                        Logger288.Log("literal operand is hex", "Parser");
                        op = Convert.ToInt32(inOper.Substring(2), 16);
                        if (op < 0)
                        {
                            op = BinaryHelper.ConvertNumber(op, 10);
                        }
                        outOper = Convert.ToString(op, 16).ToUpper();
                    } break;

                case 'B':
                case 'b':
                    {
                        Logger288.Log("literal operand is binary", "Parser");
                        op = Convert.ToInt32(inOper.Substring(2), 2);
                        if (op < 0)
                        {
                            op = BinaryHelper.ConvertNumber(op, 10);
                        }
                        outOper = Convert.ToString(op, 16).ToUpper();
                    } break;

                case 'I':
                case 'i':
                    {
                        Logger288.Log("literal operand is integer", "Parser");
                        op = Convert.ToInt32(inOper.Substring(2));
                        if (-512 < op && op < 511)
                        {
                            if (op < 0)
                            {
                                op = BinaryHelper.ConvertNumber(op, 10);
                            }
                            outOper = Convert.ToString(op, 16).ToUpper();
                        }
                        else
                        {
                            outOper = "_OUT OF BOUNDS";
                        }
                    } break;
                case 'C':
                case 'c':
                    {
                        Logger288.Log("literal operand is character", "Parser");
                        // fix this: parse integers more properly.
                        outOper = Convert.ToString(Convert.ToInt32(((int)inOper[3]) << 2), 16).ToUpper();
                    } break;
            }

            Logger288.Log(String.Format("Literal operand parsed as {0} {1}", litType, outOper), "Parser");
        }

        /**
         * Parses the start directive, properly assigning the operand of start as the
         * starting location counter.
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         *  - April 9, 2011 - Mark - Parses the START directive, correctly setting the LC.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         * 
         * @param line the line containing the start directive
         * @return the IntermediateLine of this line
         */
        private IntermediateLine ParseStart(string line)
        {
            Logger288.Log("Parsing START directive", "Parser");

            IntermediateLine start = ParseLine(line, 1);
            LC = start.DirectiveOperand;
            start.ProgramCounter = LC;

            Logger288.Log("Finished parsing START directive", "Parser");

            return start;
        }

        /**
         * Parses the end directeive, ensuring that the end operand is the same as
         * the start directive's rlabel.
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         *  - April 9, 2011 - Mark - Not sure if needed.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         * 
         * @param line the line containing the end directive
         * @param lineNum the line number of this line in the source code.
         * @return The IntermediateLine of this line
         */
        private IntermediateLine ParseEnd(string line, short lineNum)
        {
            Logger288.Log("Parsing END directive", "Parser");
            return new IntermediateLine(line, lineNum);
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
         * Checks to see if the instruction's operand field has valid syntax.
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         *  - April 9, 2011 - Mark - Correctly checks if the operand field has valid syntax.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         * 
         * @param line the line whose instruction's operand should be checked. <br />
         *             expected "FUNCTION,OPERAND : POSSIBLE COMMENTS"
         * @return true if the operand is valid <br />
         *         false if the operand is invalid
         */
        private bool ValidOperandField(string line)
        {
            Logger288.Log("Checking operand for invalid syntax", "Parser");
            string[] OperandParts = line.Split(new char[] { ',' }, 2);
            if (OperandParts.Length < 2 || OperandParts[0].Contains(" ") || OperandParts[1].StartsWith(" "))
            {
                Logger288.Log("Operand syntax invalid", "Parser");
                return false;
            }

            Logger288.Log("Operand syntax valid", "Parser");
            return true;
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
            IntermediateLine line = ParseStart(sourceCode[0]);
            symb.AddSymbol(line.Label, line.ProgramCounter, Usage.PRGMNAME, "");

            interSource = new IntermediateFile(line.Label);
            interSource.AddLine(line);

            // iterate the lines of the file
            for (short i = 2; i <= sourceCode.Length; i++)
            {
                // parse a line and create an intermediate version
                line = ParseLine(sourceCode[i - 1], i);
                interSource.AddLine(line);

                // add to the symbol table if we need to
                if (line.Label != null)
                {
                    // this doesn't work with equated symbols, yet. :(
                    symb.AddSymbol(line.Label, line.ProgramCounter, Usage.ENTRY, "");
                }
            }
        }
    }
}
