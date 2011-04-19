using System;
using NUnit.Framework;

namespace Assembler
{

    /**
     * List of tests designed specifically to check correct functionality of procedures in the Tokenizer class.
     * See the test plan for more information.
     */
    [TestFixture]
    public class TokenizerTest
    {
        /**
         * [T1] Simple test to ensure the tokenizer can correctly return a token.
         */
        [Test]
        public void TokenizerSimple()
        {
            string Line = "MOPER ADD,2";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
            Assert.IsTrue(Token == "MOPER" && TokenKind == Tokenizer.TokenKinds.Label_Or_Command,
                "Token is 'MOPER' and of kind label.");
        }

        /**
         * [T2] Tests if the tokenizer will return a token when the line begins with lots of whitespace.
         */
        [Test]
        public void TokenizerWhiteSpaceBeginning()
        {
            string Line = "            MOPER ADD,2";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
            Assert.IsTrue(Token == "MOPER" && TokenKind == Tokenizer.TokenKinds.Label_Or_Command,
                "Line begins with lots of whitespace.");
        }

        /**
         * [T3] Tokenize an entire line and ensure all tokens are returned.
         */
        [Test]
        public void TokenizerEntireLine()
        {
            string Line = " MOPER ADD,2";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            string[] expected = { "MOPER", "ADD", "2" };
            string[] actual = new string[3];

            int i = 0;
            while (Line.Length > 0)
            {
                Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
                actual[i] = Token;
                i++;
            }

            Assert.AreEqual(expected, actual, "Tokenizing an entire line.");
        }

        /**
         * [T4] Tokenize an entire line with extra whitespace and ensure all tokens are returned.
         */
        [Test]
        public void TokenizerEntireLineWS()
        {
            string Line = "     SOPER        ADD,X=55        :I'm a comment    ";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            string[] expected = { "SOPER", "ADD", "X=55", ":I'm a comment" };
            string[] actual = new string[4];

            int i = 0;
            while (Line.Length > 0)
            {
                Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
                actual[i] = Token;
                i++;
            }

            Assert.AreEqual(expected, actual, "Tokenizing an entire line with extra whitespace.");
        }

        /**
         * [T5] Tokenize an entire line with mixed case to ensure tokens are correct.
         */
        [Test]
        public void TokenizerEntireLineMixedCaseToken()
        {
            string Line = " mOpER AdD,x=aBc4";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            string[] TokenExpected = { "mOpER", "AdD", "x=aBc4" };
            string[] TokenActual = new string[3];

            int i = 0;
            while (Line.Length > 0)
            {
                Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
                TokenActual[i] = Token;
                i++;
            }

            Assert.AreEqual(TokenExpected, TokenActual,
                            "Tokenize entire line with mized case.");
        }

        /**
         * [T6] Tokenize an entire line with mixed case to ensure both the token kinds are correct.
         */
        [Test]
        public void TokenizerEntireLineMixedCaseKind()
        {
            string Line = " mOpER AdD,x=aBc4";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            Tokenizer.TokenKinds[] KindExpected = { Tokenizer.TokenKinds.Label_Or_Command,
                                                    Tokenizer.TokenKinds.Label_Or_Command,
                                                    Tokenizer.TokenKinds.Literal };
            Tokenizer.TokenKinds[] KindActual = new Tokenizer.TokenKinds[3];

            int i = 0;
            while (Line.Length > 0)
            {
                Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
                KindActual[i] = TokenKind;
                i++;
            }

            Assert.AreEqual(KindExpected, KindActual,
                            "Tokenize entire line with mized case.");
        }

        /**
         * [T7] Test that it can correctly tokenize a Label_Or_Command.
         */
        [Test]
        public void TokenizerLOC()
        {
            string Line = " CNTL CLEAR";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
            Assert.IsTrue(Token == "CNTL" && TokenKind == Tokenizer.TokenKinds.Label_Or_Command,
                "Token is 'CNTL' and of kind label.");
        }

        /**
         * [T8] Test that it can correctly tokenize a Literal.
         */
        [Test]
        public void TokenizerLiteral()
        {
            string Line = "B=101101  :Comment";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
            Assert.IsTrue(Token == "B=101101" && TokenKind == Tokenizer.TokenKinds.Literal,
                "Token is 'B=101101' and of kind Literal.");
        }

        /**
         * [T9] Test that it can correctly tokenize a Comment.
         */
        [Test]
        public void TokenizerComment()
        {
            string Line = ": This is a comment with whitespace, and commas that shouldn't be removed.";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
            Assert.IsTrue(Token == ": This is a comment with whitespace, and commas that shouldn't be removed."
                && TokenKind == Tokenizer.TokenKinds.Comment,
                "Token is of kind Comment.");
        }

        /**
        * [T10] Test that it can correctly tokenize a Number.
        */
        [Test]
        public void TokenizerNumber()
        {
            string Line = "9283 X4";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
            Assert.IsTrue(Token == "9283" && TokenKind == Tokenizer.TokenKinds.Number,
                "Token is '9283' and of kind Number.");
        }

        /**
        * [T11] Test that it can correctly tokenize an Empty.
        */
        [Test]
        public void TokenizerEmpty()
        {
            string Line = "";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
            Assert.IsTrue(Token == "" && TokenKind == Tokenizer.TokenKinds.Empty,
                "Token is '' and of kind Empty.");
        }

        /**
        * [T12] Test that it can correctly tokenize an Error.
        */
        [Test]
        public void TokenizerError()
        {
            string Line = "Go_to MOPER ADD,5";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
            Assert.IsTrue(Token == "Go_to" && TokenKind == Tokenizer.TokenKinds.Error,
                "Token is 'Go_to' and of kind Error.");
        }

        /**
        * [T13] Test that it can correctly tokenize a Jump condition.
        */
        [Test]
        public void TokenizerJump()
        {
            string Line = ">=";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
            Assert.IsTrue(Token == ">=" && TokenKind == Tokenizer.TokenKinds.JumpCond,
                "Token is '>=' and of kind JumpCond.");
        }

        /**
        * [T14] Test that it can correctly tokenize an Expression.
        */
        [Test]
        public void TokenizerExpression()
        {
            string Line = "*+MUD-Dirt";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
            Assert.IsTrue(Token == "*+MUD-Dirt" && TokenKind == Tokenizer.TokenKinds.Expression,
                "Token is *+MUD-Dirt' and of kind Expression.");
        }

        /**
         * [T15] Test that character literals return the correct token for character literals with spaces
         */
        [Test]
        public void TokenizerLiteralCharacter()
        {
            string Line = "c=' '' : all sorts of breaking right here";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
            Assert.IsTrue(Token == "c=' ''" && TokenKind == Tokenizer.TokenKinds.Literal,
                "Token is \"c=' ''\" and of kind Literal. \"" + Token + "\"");
        }

        /**
         * [T16] Ensure negative numbers are processed as numbers (not expressions).
         */
        [Test]
        public void TokenizerNegativeNumber()
        {
            string Line = "-20";
            string Token;
            Tokenizer.TokenKinds TokenKind;
            Tokenizer.GetNextToken(ref Line, out Token, out TokenKind);
            Assert.IsTrue(Token == "-20" && TokenKind == Tokenizer.TokenKinds.Number,
                "Negative number is a number kind");
        }
    }
}

