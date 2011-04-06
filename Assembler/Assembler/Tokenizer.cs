using System;

class Tokenizer
{
    /**
     * Returns the next token in the provided string.  The next token includes all
     * characters in the given string until the next ' ', ',' or when the end of the
     * string is reached.  TokenKind is determined based on the characters in the token.
     * Label_Or_Command - Contains only letters and numbers with at least one letter.
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
    public static void GetNextToken(ref string Line, ref string Token, ref string TokenKind)
    {
        string[] Tokens = new string[2];

        Tokens = Line.Split(new char[] { ' ', ',' }, 2);
        Token = Tokens[0];
        Line = Tokens[1];
    }

    /**
     * TODO: Delete this.  Only being used to test stuff currently.
     */
    public static void Main(string[] args)
    {
        //Testing getNextToken
        string ohGodWhatShouldICallIt = "Th,E quick brown fox died.";
        string Token = "";
        string TokenKind = "";
        GetNextToken(ref ohGodWhatShouldICallIt, ref Token, ref TokenKind);
        Console.WriteLine(Token + '\n' + ohGodWhatShouldICallIt);

        /*
         * TODO: Need to write method(s) to determine the token type.
         */
    }
}