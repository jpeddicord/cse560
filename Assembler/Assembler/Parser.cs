using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Assembler
{
    class Parser
    {
        Directives directiveList;
        Instructions instructionList;

        public Parser()
        {
            directiveList = Directives.GetInstance();
            instructionList = Instructions.GetInstance();

            Console.WriteLine(ParseLine(" MOPER WRITEN,RES  :output RES         3  F807   r", 2));
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
                }
                else if (directiveList.Contains(token)) 
                {
                    ParseInstruction(ref line, ref interLine);
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

        private void ParseDirective()
        {

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
                    }break;
            }

            litType = inOper.Substring(0, 2);
        }

        public IntermediateFile ParseSource()
        {
            return new IntermediateFile();
        }
    }
}
