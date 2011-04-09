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
        Directives directiveList;
        Instructions instructionList;

        int LC;

        public Parser()
        {
            Trace.WriteLine(String.Format("{0} -> {1}", DateTime.Now, "Creating Parser object."), "Parser");
            directiveList = Directives.GetInstance();
            instructionList = Instructions.GetInstance();
            LC = 0;
        }

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
                    interLine.ProgramCounter = LC.ToString();
                    LC++;
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

        private void ParseInstruction(ref string line, ref IntermediateLine interLine)
        {
            string token = "";
            Tokenizer.TokenKinds tokenKind = Tokenizer.TokenKinds.Empty;

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

        private IntermediateLine ParseStart(string line, short lineNum)
        {
            IntermediateLine start = ParseLine(line, lineNum);
            LC = int.Parse(start.DirectiveOperand);
            return start;
        }

        private IntermediateLine ParseEnd(string line, short lineNum)
        {
            return new IntermediateLine(line, lineNum);
        }

        public IntermediateFile ParseSource(string path)
        {
            string[] sourceCode = new string[1];
            try
            {
                Trace.WriteLine(String.Format("{0} -> {1}", DateTime.Now, "Opening file: " + path), "Parser");
                sourceCode = File.ReadAllLines(path);
            }
            catch (FileNotFoundException ex)
            {
                Trace.WriteLine(String.Format("{0} -> {1}", DateTime.Now, "Failed to open file. Error: " + ex.Message));
                Console.WriteLine("{0}\n{1}", ex.Message, "Exiting program");
                System.Environment.Exit(1);
            }

            // first line is expected to hold the start directive
            IntermediateLine temp = ParseStart(sourceCode[0], 1);

            IntermediateFile interSource = new IntermediateFile(temp.Label);
            interSource.AddLine(temp);

            for (short i = 2; i < sourceCode.Length; i++)
            {
                temp = ParseLine(sourceCode[i - 1], i);
                interSource.AddLine(temp);
            }

            Console.WriteLine(interSource);
            return interSource;
        }
    }
}
