using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Assembler
{
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
        string LC;

        /**
         * Creates a Parser with location counter set to 0, ready to parse a single soure file.
         */
        public Parser()
        {
            Trace.WriteLine("Creating Parser object.", "Parser");
            directiveList = Directives.GetInstance();
            instructionList = Instructions.GetInstance();
            LC = "0";
        }

        /**
         * Parses a single line of source code.
         * 
         * @param line current line to parse
         * @param lineNum line number of current line
         * @return the line to be parsed as a single line in the intermediate file
         */
        private IntermediateLine ParseLine(string line, short lineNum)
        {
            string token = "";
            Tokenizer.TokenKinds tokenKind = Tokenizer.TokenKinds.Empty;

            // make Intermediate version of this line to be returned
            IntermediateLine interLine = new IntermediateLine(line, lineNum);

            // check for a label at the beginning of the line
            if (char.IsLetter(line[0]))
            {
                Tokenizer.GetNextToken(ref line, ref token, ref tokenKind);
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

            /**
             * Get the next token, which will specify whether or not the line has a
             * directive or an instruction.
             **/
            Tokenizer.GetNextToken(ref line, ref token, ref tokenKind);

            if (tokenKind == Tokenizer.TokenKinds.Label_Or_Command)
            {
                // instruction
                if (instructionList.IsGroup(token))
                {
                    interLine.OpCategory = token;
                    ParseInstruction(ref line, ref interLine);
                    interLine.ProgramCounter = LC;
                    IncrementLocationCounter();
                }
                else if (directiveList.Contains(token))
                {
                    interLine.Directive = token;
                    ParseDirective(ref line, ref interLine);
                }
            }

            // If there's anything else, get it. If there's anything there,
            // it should be a comment.
            Tokenizer.GetNextToken(ref line, ref token, ref tokenKind);

            if (tokenKind == Tokenizer.TokenKinds.Comment)
            {
                interLine.Comment = token;
            }

            return interLine;
        }

        /**
         * Parses the operation section of the line if it has an instruction.
         * 
         * @param line current line to parse.
         * @param interLine the line as a single line in the intermediate file.
         */
        private void ParseInstruction(ref string line, ref IntermediateLine interLine)
        {
            string token = "";
            Tokenizer.TokenKinds tokenKind = Tokenizer.TokenKinds.Empty;

            if (ValidOperandField(line))
            {
                // get what should be the function name
                Tokenizer.GetNextToken(ref line, ref token, ref tokenKind);

                if (instructionList.IsInstruction(interLine.OpCategory, token))
                {
                    interLine.OpFunction = token;
                }
                else
                {
                    interLine.OpFunction = "_ERROR";
                }

                // get what should be the function operand
                Tokenizer.GetNextToken(ref line, ref token, ref tokenKind);

                if (tokenKind == Tokenizer.TokenKinds.Label_Or_Command)
                {
                    interLine.OpOperand = token;
                }
                else if (tokenKind == Tokenizer.TokenKinds.Literal)
                {
                    string operand = "";
                    string litOperand = "";
                    ParseLiteralOperand(token, ref operand, ref litOperand);
                    interLine.OpOperand = operand;
                    interLine.OpLitOperand = litOperand;
                }
                else if (tokenKind == Tokenizer.TokenKinds.Number)
                {
                    interLine.OpOperand = Convert.ToString(int.Parse(token), 16).ToUpper();
                }
                else
                {
                    interLine.OpOperand = "_ERROR";
                }
            }
            else
            {
                interLine.OpFunction = "_ERROR";
                interLine.OpOperand = "_ERROR";
            }

            
        }

        /**
         * Parses the operation section of the line if it has an instruction.
         * 
         * @param line current line to parse.
         * @param interLine the line as a single line in the intermediate file.
         */
        private void ParseDirective(ref string line, ref IntermediateLine interLine)
        {
            string token = "";
            Tokenizer.TokenKinds tokenKind = Tokenizer.TokenKinds.Empty;

            // get the operand of the directive
            Tokenizer.GetNextToken(ref line, ref token, ref tokenKind);

            if (tokenKind == Tokenizer.TokenKinds.Label_Or_Command)
            {
                interLine.DirectiveOperand = token;
            }
            else if (tokenKind == Tokenizer.TokenKinds.Number)
            {
                interLine.DirectiveOperand = Convert.ToString(int.Parse(token), 16).ToUpper();
            }
            else if (tokenKind == Tokenizer.TokenKinds.Literal)
            {
                string operand = "";
                string litOperand = "";
                ParseLiteralOperand(token, ref operand, ref litOperand);
                interLine.DirectiveOperand = operand;
                interLine.DirectiveLitOperand = litOperand;
            }
            else
            {
                interLine.DirectiveOperand = "_ERROR";
            }
        }

        /**
         * Parses a literal operand, e.g. X=, B=, I=, C=
         * 
         * @param inOper the operand to parse
         * @param outOper the numerical operand from the literal, that is, the part after the X=
         * @param litType the type of the operand, that is, X, B, etc.
         */
        private void ParseLiteralOperand(string inOper, ref string outOper, ref string litType)
        {
            switch (inOper[0])
            {
                case 'X':
                case 'x':
                    {
                        outOper = inOper.Substring(2);
                    } break;

                case 'B':
                case 'b':
                    {
                        outOper = Convert.ToString(Convert.ToInt32(inOper.Substring(2), 2), 16).ToUpper();
                    } break;

                case 'I':
                case 'i':
                    {
                        // outOper = 
                    } break;
            }

            litType = inOper.Substring(0, 2);
        }

        /**
         * Parses the start directive, properly assigning the operand of start as the
         * starting location counter.
         * 
         * @param line the line containing the start directive
         * @return the IntermediateLine of this line
         */
        private IntermediateLine ParseStart(string line)
        {
            IntermediateLine start = ParseLine(line, 1);
            LC = start.DirectiveOperand;
            start.ProgramCounter = LC;
            return start;
        }

        /**
         * Parses the end directeive, ensuring that the end operand is the same as
         * the start directive's rlabel.
         * 
         * @param line the line containing the end directive
         * @param lineNum the line number of this line in the source code.
         * @return The IntermediateLine of this line
         */
        private IntermediateLine ParseEnd(string line, short lineNum)
        {
            return new IntermediateLine(line, lineNum);
        }

        /**
         * Adds one to the location counter.
         */
        private void IncrementLocationCounter()
        {
            int tempLC = Convert.ToInt32(LC, 16);
            tempLC++;
            LC = Convert.ToString(tempLC, 16).ToUpper();
        }

        /**
         * Checks to see if the instruction's operand field has valid syntax.
         * 
         * @param line the line whose instruction's operand should be checked. <br />
         *             expected "FUNCTION,OPERAND : POSSIBLE COMMENTS"
         * @return true if the operand is valid <br />
         *         false if the operand is invalid
         */
        private bool ValidOperandField(string line)
        {
            string[] OperandParts = line.Split(new char[] { ',' }, 2);
            if (OperandParts.Length < 2 || OperandParts[0].Contains(" ") || OperandParts[1].StartsWith(" "))
            {
                return false;
            }

            return true;
        }

        /**
         * Parses an entire source code file.
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
                Trace.WriteLine("Opening file: " + path, "Parser");
                sourceCode = File.ReadAllLines(path);
            }
            catch (FileNotFoundException ex)
            {
                Trace.WriteLine("Failed to open file. Error: " + ex.Message, "Parser");
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
                    // this doesn't work with equated symbols, yet.
                    symb.AddSymbol(line.Label, line.ProgramCounter, Usage.ENTRY, "");
                }
            }
        }
    }
}
