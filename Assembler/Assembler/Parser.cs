﻿using System;
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
            Trace.WriteLine(String.Format("{0} -> {1}", System.DateTime.Now, "Initializing parser."), "Parser");

            string test = "      Jacob is a whore! :and this is a comment";
            string Token = "";
            string TokenKind = "";

            while (test.Length > 0)
            {
                Tokenizer.GetNextToken(ref test, ref Token, ref TokenKind);
                Console.WriteLine(Token + " - " + TokenKind);
            } 
        }
    }
}
