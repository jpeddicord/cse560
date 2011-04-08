using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Assembler
{
    class Tokenizer
    {
        public enum TokenKinds
        {
            Label_Or_Command, Literal, Comment, Number, Empty, Error
        };

        public Tokenizer()
        {
            Trace.WriteLine(String.Format("{0} -> {1}", System.DateTime.Now, "Initializing tokenizer."), "Tokenizer");
        }
        /**
         * Returns the next token in the provided string.  The next token includes all
         * characters in the given string until the next ' ', ',' or when the end of the
         * string is reached.  TokenKind is determined based on the characters in the token.
         * Label_Or_Command - Contains only letters and numbers and begins with a letter.
         * Literal - A token that starts with "I=", "X=", "B=" or "C=".
         * Comment - A token that starts with ':'.
         * Number - Contains only numbers.
         * Empty - The token contains no text.
         * Error - Contains characters that do not belong in any of the other token kinds.
         * Once the token is found it is removed from the given string along with the
         * separator character.
         * 
         * @param Line The line of code from which to find the next token.
         * @param Token Used to store the value of the next token.
         * @param TokenKind Used to store the token kind of the next token.
         */
        public static void GetNextToken(ref string Line, ref string Token, TokenKinds TokenKind)
        {
            Line = Line.Trim();

            if (Line[0] == ':')
            {
                Token = Line;
                TokenKind = TokenKinds.Comment;
                Line = "";
            }
            else
            {
                string[] Tokens = { "", "Empty" };
                Tokens = Line.Split(new char[] { ' ', ',', '\t' }, 2);
                Token = Tokens[0];

                if (Tokens.Length > 1)
                {
                    Line = Tokens[1];
                }
                else
                {
                    Line = "";
                }

                GetTokenKind(Token, TokenKind);
            }
        }

        /**
         * DERp
         */
        private static void GetTokenKind(string Token, TokenKinds TokenKind)
        {
            Regex AlphaNumeric = new Regex("[^0-9a-zA-Z]");
            Regex Numeric = new Regex("[^0-9]");

            if (Token == "")
            {
                TokenKind = TokenKinds.Empty;
            }
            else if (!AlphaNumeric.IsMatch(Token) && Char.IsLetter(Token[0]))
            {
                TokenKind = TokenKinds.Label_Or_Command;
            }
            else if (!Numeric.IsMatch(Token))
            {
                TokenKind = TokenKinds.Number;
            }
            else if (Token.Length > 1 && (Token.Substring(0, 2) == "X=" ||
                                         Token.Substring(0, 2) == "B=" ||
                                         Token.Substring(0, 2) == "I=" ||
                                         Token.Substring(0, 2) == "C="))
            {
                TokenKind = TokenKinds.Literal;
            }
            else
            {
                TokenKind = TokenKinds.Error;
            }
        }
    }
}