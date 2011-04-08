using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Assembler
{
    class Parser
    {
        public Parser()
        {
           
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
                Tokenizer.GetNextToken(ref line, ref token, tokenKind);
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



            return interLine;
        }

        public IntermediateFile ParseSource()
        {
            return new IntermediateFile();
        }
    }
}
