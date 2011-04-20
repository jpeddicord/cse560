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
            /**
             * Contains only letters and numbers and begins with a letter.
             */
            Label_Or_Command,

            /**
             * A token that starts with "I=", "X=", "B=", "C=".
             */
            Literal,

            /**
             * A token that starts with ':'.
             */
            Comment,

            /**
             * Contains only numbers.
             */
            Number,

            /**
             * The token contains no text.
             */
            Empty,

            /**
             * Contains characters that do not belong in any of the other token kinds.
             */
            Error,

            /**
             * One of the six jump conditions: =, ^=, &lt;, &gt;, &lt;=, and &gt;=
             */
            JumpCond,

            /**
             * Can contain labels, numbers and operands (+ or -) and * notation.
             */
            Expression
        };

        public Tokenizer()
        {
            // Write a message to the log to confirm the tokenizer has been initialized
            Logger.Log("Initializing tokenizer.", "Tokenizer");
        }

        /**
         * Returns the next token in the provided string.  The next token includes all
         * characters in the given string until the next ' ', ',', tab, or when the end of the
         * string is reached.  TokenKind is determined based on the characters in the token.
         * Once the token is found it is removed from the given string along with the
         * separator character.
         * 
         * @refcode N/A
         * @errtest 
         *          Tokenizer was tested using strings that were empty, contained extra whitespace,
         *          and mixed case letters.  For a full list of tests please look at the testing plan.
         * @errmsg N/A
         * @author Andrew
         * @creation April 6, 2011
         * @modlog 
         *  - April 7, 2011 - Andrew - Tokenizer now returns a token kind.
         *  - April 7, 2011 - Andrew - Fixed a bug that would result in an infinite loop
         *         when the end of the line was reached.
         *  - April 8, 2011 - Mark - Changed Tokenizer to work with enumerated types.
         *  - April 8, 2011 - Andrew - Fixed an aray out of bounds exception that occured when
         *         tokenizer was given an empty string.
         *  - April 16, 2011 - Andrew - Added Expression as a possible token kind.
         *  - April 18, 2011 - Andrew - Character literals would break when there was a space in the single quotes.
         *      Now using regular expressions to ensure only correct character literals are returned.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         *
         * @param line The line of code from which to find the next token.
         * @param token Used to store the value of the next token.
         * @param tokenKind Used to store the token kind of the next token.
         */
        public static void GetNextToken(ref string line, out string token, out TokenKinds tokenKind)
        {
            // Write to the log before attempting to remove the next token.
            Logger.Log("Getting new token.", "Tokenizer");

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
                string[] tokens;

                if (line.ToUpper().StartsWith("C='"))
                {
                    Regex charLiteral = new Regex("([Cc]='.{0,2}')");
                    Match match = charLiteral.Match(line);
                    if (match.Success)
                    {
                        token = match.Groups[1].Value;
                        tokenKind = TokenKinds.Literal;
                    }
                    else
                    {
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

                        tokenKind = TokenKinds.Error;
                    }

                }
                else
                {
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
                    GetTokenKind(token, out tokenKind);
                }
                    
            }

            //Trim spaces off the beginning of the returned line so the first character is the beginning of the next token.
            line = line.TrimStart();

            // Write to the log after the token has been successfully retrieved.
            Logger.Log("Token acquired, returning.", "Tokenizer");
        }

        /**
         * Used to determine the kind for a specific token.  Token kinds are defined by
         * GetNextToken.  This procedure will return token kinds for any token except for
         * a comment.  This procedure does not check that all restrictions are met, only
         * that a general pattern is matched.  For example, if the token is a label, it
         * will not check that the label is shorter than the maximum label length.
         * 
         * @refcode S2.4.2.1, S2.4.2.2, S2.4.2.3, S2.4.2.4
         * @errtest 
         *          GetTokenKind was tested indirectly through GetToken.  If the correct token
         *          kind was not returned then there was an issue with this procedure.  As with
         *          GetToken, please see the testing plan for a full list of tests performed.
         * @errmsg N/A
         * @author Andrew
         * @creation April 7, 2011
         * @modlog 
         *  - April 8, 2011 - Mark - Changed Tokenizer to work with enumerated types.
         *  - April 9, 2011 - Andrew - TokenKind was not case insensitive to literals, fixed.
         *  - April 10, 2011 - Andrew - Added token kind JumpCond as the jump conditions did
         *          not fit in the other token kinds.
         *  - April 10, 2011 - Andrew - Added the NOP flag "=0" to be considered a literal.
         *  - April 14, 2011 - Andrew - Due to a change in the specifications "=0" no longer
         *                  denotes a NOP, so this has been removed from the literals.
         *  - April 16, 2011 - Andrew - Added Expression as a possible token kind.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         * 
         * @param token Holds the token for which the kind is to be determined.
         * @param tokenKind Used to store the token kind of the specified token.
         */
        public static void GetTokenKind(string token, out TokenKinds tokenKind)
        {
            /**
             * Regular expression used to determine if all characters in the token are
             * letters or numbers.
             */
            Regex alphaNumeric = new Regex(@"[^0-9a-zA-Z]");

            /**
             * Regular expression used to determine if all characters in the token are
             * numbers.
             */
            Regex numeric = new Regex(@"^[\-\+]?\d+$");

            /**
             * Regular expression used to determine if all characters in the token match
             * those of an expression.  Expressions can have alphanumeric characters as
             * well as '+', '-' and '*'. Note: '*' is not used for multiplication, it is
             * used in start notation (see web documentation).
             */
            Regex expression = new Regex(@"[^0-9A-Za-z\*\+\-]");

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
            else if (numeric.IsMatch(token))
            {
                tokenKind = TokenKinds.Number;
            } // Check if the token contains characters only found in expressions.
            else if (!expression.IsMatch(token))
            {
                tokenKind = TokenKinds.Expression;
            } // Only looks for the four possible Literal flags.  Does not check format of following chars.
            else if (token.Length > 1 && (token.Substring(0, 2) == "X=" ||
                                         token.Substring(0, 2) == "B=" ||
                                         token.Substring(0, 2) == "I=" ||
                                         token.Substring(0, 2) == "C="))
            {
                tokenKind = TokenKinds.Literal;
            } // Determines if the token is one of the six jump conditions
            else if (token == "=" || token == "^=" || token == "<" ||
                     token == ">" || token == "<=" || token == ">=")
            {
                tokenKind = TokenKinds.JumpCond;
            }
            else // If no other pattern is matched something is wrong in the token and we give an error.
            {
                tokenKind = TokenKinds.Error;
            }
        }
    }
}