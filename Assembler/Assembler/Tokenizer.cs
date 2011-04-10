using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Assembler
{

    /**
     * Tokenizer is used to get the next token from a line. A token is a group of characters separated from the
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
            Trace.WriteLine("Initializing tokenizer.", "Tokenizer");
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
         * @errtest 
         *          Tokenizer was tested using strings that were empty, contained extra whitespace,
         *          and mixed case letters.  For a full list of tests please look at the testing plan.
         * @author Andrew
         * @creation April 6, 2011
         * @modlog 
         *         *April 7, 2011 - Andrew - Tokenizer now returns a token kind.
         *         *April 7, 2011 - Andrew - Fixed a bug that would result in an infinite loop
         *         when the end of the line was reached.
         *         *April 8, 2011 - Mark - Changed Tokenizer to work with enumerated types.
         *         *April 8, 2011 - Andrew - Fixed an aray out of bounds exception that occured when
         *         tokenizer was given an empty string.
         * @teststandard Andrew Buelow
         *
         * @param Line The line of code from which to find the next token.
         * @param Token Used to store the value of the next token.
         * @param TokenKind Used to store the token kind of the next token.
         */
        public static void GetNextToken(ref string line, ref string token, ref TokenKinds tokenKind)
        {
            // Write to the log before attempting to remove the next token.
            Trace.WriteLine("Getting new token.", "Tokenizer");

            // Remove extra whitespace
            line = line.Trim();

            // If the next token is a comment, we don't need to split up the line and can return everything
            // else as the token
            if (line.Length > 0 && line[0] == ':')
            {
                token = line;
                tokenKind = TokenKinds.Comment;
                line = "";
            }
            else
            {
                /**
                 * Holds the two pieces of the string after it has been split at the next space, comma or tab.
                 * The first piece is the next token, the other is the rest of the line.
                 */
                string[] tokens = { "", "Empty" };

                // Split the string at the next space, comma or tab and store the first part as the token.
                tokens = line.Split(new char[] { ' ', ',', '\t' }, 2);
                token = tokens[0];

                // If we are at the end of a string the second item in the array will be empty so we need to
                // set the line to be an empty string.
                if (tokens.Length > 1)
                {
                    line = tokens[1];
                }
                else
                {
                    line = "";
                }

                // Determine the kind for this token.
                GetTokenKind(token, ref tokenKind);
            }

            //Trim spaces off the beginning of the returned line so the first character is the beginning of the next token.
            line = line.TrimStart();

            // Write to the log after the token has been successfully retrieved.
            Trace.WriteLine("Token acquired, returning.", "Tokenizer");
        }

        /**
         * Used to determine the kind for a specific token.  Token kinds are defined by
         * GetNextToken.  This procedure will return token kinds for any token except for
         * a comment.  This procedure does not check that all restrictions are met, only
         * that a general pattern is matched.  For example, if the token is a label, it
         * will not check that the label is shorter than the maximum label length.
         * 
         * @errtest 
         *          GetTokenKind was tested indirectly through GetToken.  If the correct token
         *          kind was not returned then there was an issue with this procedure.  As with
         *          GetToken, please see the testing plan for a full list of tests performed.
         * @author Andrew
         * @creation April 7, 2011
         * @modlog 
         *         *April 8, 2011 - Mark - Changed Tokenizer to work with enumerated types.
         *         *April 9, 2011 - Andrew - TokenKind was not case insensitive to literals, fixed.
         * @teststandard Andrew Buelow
         * 
         * @param Token Holds the token for which the kind is to be determined.
         * @param TokenKind Used to store the token kind of the specified token.
         */
        private static void GetTokenKind(string token, ref TokenKinds tokenKind)
        {
            /**
             * Regular expression used to determine if all characters in the token are
             * letters or numbers.
             */
            Regex alphaNumeric = new Regex("[^0-9a-zA-Z]");

            /**
             * Regular expression used to determine if all characters in the token are
             * numbers.
             */
            Regex numeric = new Regex("[^0-9]");

            // Convert to uppercase to give user flexibility.  Token is passed by value so
            // this change will not affect the token that is returned to the user.
            token = token.ToUpper();

            // Check that we weren't given an empty string first so we can look at
            // characters in the string later.
            if (token == "")
            {
                tokenKind = TokenKinds.Empty;
            } // Can only be a label or command if it begins with a letter and is all letters or numbers.
            else if (!alphaNumeric.IsMatch(token) && Char.IsLetter(token[0]))
            {
                tokenKind = TokenKinds.Label_Or_Command;
            } // Check if it is all numbers
            else if (!numeric.IsMatch(token))
            {
                tokenKind = TokenKinds.Number;
            } // Only looks for the four possible Literal flags.  Does not check format of following chars.
            else if (token.Length > 1 && (token.Substring(0, 2) == "X=" ||
                                         token.Substring(0, 2) == "B=" ||
                                         token.Substring(0, 2) == "I=" ||
                                         token.Substring(0, 2) == "C="))
            {
                tokenKind = TokenKinds.Literal;
            }
            else // If no other pattern is matched something is wrong in the token and we give an error.
            {
                tokenKind = TokenKinds.Error;
            }
        }
    }
}