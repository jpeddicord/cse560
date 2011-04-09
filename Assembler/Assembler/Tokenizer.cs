using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Assembler
{

    /**
     * Tokenizer is used to get the next token from a line. A token is a group of characters seperated from the
     * rest of the line by a space, comma or tab except in the case of a comment where the rest of the line after
     * the ':' is the token or in a line where no comma, space or tab is present in which case the whole line is
     * the token.  Tokens will be returned with a token kind of Label_Or_Command, Literal, Comment, Number, Empty
     * or Error.
     */
    class Tokenizer
    {
        /**
         * Enumeration for the six possible token kinds that can be found while tokenizing.
         */
        public enum TokenKinds
        {
            Label_Or_Command, Literal, Comment, Number, Empty, Error
        };

        public Tokenizer()
        {
            // Write a message to the log to confirm the tokenizer has been initialized
            Trace.WriteLine(String.Format("{0} -> {1}", System.DateTime.Now, "Initializing tokenizer."), "Tokenizer");
        }

        /**
         * Returns the next token in the provided string.  The next token includes all
         * characters in the given string until the next ' ', ',', '\t' or when the end of the
         * string is reached.  TokenKind is determined based on the characters in the token. <br />
         * Label_Or_Command - Contains only letters and numbers and begins with a letter. <br />
         * Literal - A token that starts with "I=", "X=", "B=" or "C=". <br />
         * Comment - A token that starts with ':'. <br />
         * Number - Contains only numbers. <br />
         * Empty - The token contains no text. <br />
         * Error - Contains characters that do not belong in any of the other token kinds. <br />
         * Once the token is found it is removed from the given string along with the
         * separator character.
         * 
         * @param Line The line of code from which to find the next token.
         * @param Token Used to store the value of the next token.
         * @param TokenKind Used to store the token kind of the next token.
         */
        public static void GetNextToken(ref string Line, ref string Token, ref TokenKinds TokenKind)
        {
            // Write to the log before attempting to remove the next token.
            Trace.WriteLine(String.Format("{0} -> {1}", System.DateTime.Now, "Getting new token."), "Tokenizer");

            // Remove extra whitespace
            Line = Line.Trim();

            // If the next token is a comment, we don't need to split up the line and can return everything
            // else as the token
            if (Line.Length > 0 && Line[0] == ':')
            {
                Token = Line;
                TokenKind = TokenKinds.Comment;
                Line = "";
            }
            else
            {
                /**
                 * Holds the two pieces of the string after it has been split at the next space, comma or tab.
                 * The first piece is the next token, the other is the rest of the line.
                 */
                string[] Tokens = { "", "Empty" };

                // Split the string at the next space, comma or tab and store the first part as the token.
                Tokens = Line.Split(new char[] { ' ', ',', '\t' }, 2);
                Token = Tokens[0];

                // If we are at the end of a string the second item in the array will be empty so we need to
                // set the line to be an empty string.
                if (Tokens.Length > 1)
                {
                    Line = Tokens[1];
                }
                else
                {
                    Line = "";
                }

                // Determine the kind for this token.
                GetTokenKind(Token, ref TokenKind);
            }

            // Write to the log after the token has been successfully retrieved.
            Trace.WriteLine(String.Format("{0} -> {1}", System.DateTime.Now, "Token acquired, returning."), "Tokenizer");
        }

        /**
         * Used to determine the kind for a specific token.  Token kinds are defined by
         * GetNextToken.  This procedure will return token kinds for any token except for
         * a comment.  This procedure does not check that all restrictions are met, only
         * that a general pattern is matched.  For example, if the token is a label, it
         * will not check that the label is shorter than the maximum label length.
         * 
         * @param Token Holds the token for which the kind is to be determined.
         * @param TokenKind Used to store the token kind of the specified token.
         */
        private static void GetTokenKind(string Token, ref TokenKinds TokenKind)
        {
            /**
             * Regular expression used to determine if all characters in the token are
             * letters or numbers.
             */
            Regex AlphaNumeric = new Regex("[^0-9a-zA-Z]");

            /**
             * Regular expression used to determine if all characters in the token are
             * numbers.
             */
            Regex Numeric = new Regex("[^0-9]");

            // Convert to uppercase to give user flexibility.  Token is passed by value so
            // this change will not affect the token that is returned to the user.
            Token = Token.ToUpper();

            // Check that we weren't given an empty string first so we can look at
            // characters in the string later.
            if (Token == "")
            {
                TokenKind = TokenKinds.Empty;
            } // Can only be a label or command if it begins with a letter and is all letters or numbers.
            else if (!AlphaNumeric.IsMatch(Token) && Char.IsLetter(Token[0]))
            {
                TokenKind = TokenKinds.Label_Or_Command;
            } // Check if it is all numbers
            else if (!Numeric.IsMatch(Token))
            {
                TokenKind = TokenKinds.Number;
            } // Only looks for the four possible Literal flags.  Does not check format of folloing chars.
            else if (Token.Length > 1 && (Token.Substring(0, 2) == "X=" ||
                                         Token.Substring(0, 2) == "B=" ||
                                         Token.Substring(0, 2) == "I=" ||
                                         Token.Substring(0, 2) == "C="))
            {
                TokenKind = TokenKinds.Literal;
            }
            else // If no other pattern is matched something is wrong in the token and we give an error.
            {
                TokenKind = TokenKinds.Error;
            }
        }
    }
}