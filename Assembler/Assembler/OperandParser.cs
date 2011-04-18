using System;

namespace Assembler
{
    /**
     * Parses the operand field into usable information by the assembler.
     */
    static class OperandParser
    {
        /**
         * Literal operand types.
         */
        public enum Literal
        {
            /**
             * No literal present.
             */
            NONE,

            /**
             * Unknown literal type. Indicates a parsing error.
             */
            UNKNOWN,

            /**
             * The operand is an expression.
             */
            EXPRESSION,

            /**
             * A non-prefixed number.
             */
            NUMBER,

            /**
             * I= (integer)
             */
            I,

            /**
             * X= (hex)
             */
            X,

            /**
             * B= (binary)
             */
            B,

            /**
             * C= (character)
             */
            C
        }

        public enum Expressions
        {
            Operator,
            EQUe,
            ADCe
        }

        /**
         * Parse an operand and fill the intermediate line with found data.
         * XXX: DOC
         */
        public static void ParseOperand(ref string line, ref IntermediateLine interLine, ref SymbolTable symb, int bits)
        {
            // get the next token
            string token;
            Tokenizer.TokenKinds tokenKind;
            Tokenizer.GetNextToken(ref line, out token, out tokenKind);

            string operand = null;
            Literal litOperand = Literal.NONE;

            // if it's a label, then just set the token directly
            if (tokenKind == Tokenizer.TokenKinds.Label_Or_Command)
            {
                litOperand = Literal.NONE;
                operand = token;
            }
            // if it's a number, convert to hex and store
            else if (tokenKind == Tokenizer.TokenKinds.Number)
            {
                litOperand = Literal.NUMBER;
                int op = Convert.ToInt32(token);
                op = BinaryHelper.ConvertNumber(op, bits);
                operand = Convert.ToString(op, 16).ToUpper();
            }
            // do further processing for literals
            else if (tokenKind == Tokenizer.TokenKinds.Literal)
            {
                ParseLiteralOperand(token, out operand, out litOperand, bits);
            }
            // the operand is an expression
            else if (tokenKind == Tokenizer.TokenKinds.Expression)
            {
                // check that it's valid

            }
            // anything else is invalid
            else
            {
                litOperand = Literal.NONE;
                operand = "_ERROR";
            }

            // assign the directive fields if it's a directive
            if (interLine.Directive != null)
            {
                interLine.DirectiveOperand = operand;
                interLine.DirectiveLitOperand = litOperand;
            }
            // otherwise fill the default operands
            else
            {
                interLine.OpOperand = operand;
                interLine.OpLitOperand = litOperand;
            }
        }

        /**
         * Parses a literal operand, e.g. X=, B=, I=, C=
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         *  - April  9, 2011 -  Mark - Parses hex and binary literals.
         *  - April  9, 2011 -  Mark - Parameters changed. Return type changed to void.
         *  - April  9, 2011 -  Mark - Now parses integer literals.
         *  - April 10, 2011 - Jacob - Parses single character in accordance with instruction limits.
         *  - April 11, 2011 -  Mark - Checks that integer literals are in the proper range.
         *  - April 14, 2011 -  Mark - Moved from Parser into OperandParser.
         *  - April 17, 2011 - Jacob - Pass in the bit length of integers to store in hex.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         * 
         * @param inOper the operand to parse
         * @param outOper the numerical operand from the literal, that is, the part after the X=
         *                  will be in hexadecimal
         * @param litType the type of the operand, that is, X, B, etc.
         * @param bits the bit length to store as in hex
         */
        private static void ParseLiteralOperand(string inOper, out string outOper, out Literal litType, int bits)
        {
            Logger.Log("Parsing literal operand " + inOper, "Parser");

            int op;
            inOper = inOper.ToUpper();
            outOper = inOper.Substring(2);
            litType = OperandParser.Literal.UNKNOWN;

            // TODO: bound-check these based on the passed-in bits
            switch (inOper[0])
            {
                case 'X':
                    {
                        Logger.Log("literal operand is hex", "Parser");
                        litType = Literal.X;
                        outOper = outOper.ToUpper();
                    } break;

                case 'B':
                    {
                        Logger.Log("literal operand is binary", "Parser");
                        litType = Literal.B;
                        op = Convert.ToInt32(inOper.Substring(2), 2);
                        op = BinaryHelper.ConvertNumber(op, bits);
                        outOper = Convert.ToString(op, 16).ToUpper();
                    } break;

                case 'I':
                    {
                        Logger.Log("literal operand is integer", "Parser");
                        litType = Literal.I;
                        op = Convert.ToInt32(inOper.Substring(2));
                        op = BinaryHelper.ConvertNumber(op, bits);
                        outOper = Convert.ToString(op, 16).ToUpper();
                    } break;
                case 'C':
                    {
                        Logger.Log("literal operand is character", "Parser");
                        litType = Literal.C;
                        // fix this: parse integers more properly.
                        outOper = Convert.ToString(Convert.ToInt32(((int)inOper[3]) << 2), 16).ToUpper();
                    } break;
            }

            Logger.Log(String.Format("Literal operand parsed as {0} {1}", litType, outOper), "Parser");
        }

        /**
         * Checks to see if the instruction's operand field has valid syntax.
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         *  - April  9, 2011 - Mark - Correctly checks if the operand field has valid syntax.
         *  - April 14, 2011 - Mark - Moved from Parser into OperandParser.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         * 
         * @param line the line whose instruction's operand should be checked. <br />
         *             expects line = "FUNCTION,OPERAND : POSSIBLE COMMENTS"
         * @return true if the operand is valid <br />
         *         false if the operand is invalid
         */
        public static bool ValidOperandField(string line)
        {
            Logger.Log("Checking operand for invalid syntax", "Parser");
            string[] OperandParts = line.Split(new char[] { ',' }, 2);
            if (OperandParts.Length < 2 || OperandParts[0].Contains(" ") || OperandParts[1].StartsWith(" "))
            {
                Logger.Log("Operand syntax invalid", "Parser");
                return false;
            }

            Logger.Log("Operand syntax valid", "Parser");
            return true;
        }

        /**
         * 
         */
        public static void ParseExpression(ref string operand, OperandParser.Expressions type, ref SymbolTable symb)
        {
            if (operand != null && operand.Length > 0)
            {
                switch (type)
                {
                    case OperandParser.Expressions.Operator:
                        {

                            char oprtr;
                            string opr2;
                            string star;

                            if (operand[0] == '*')
                            {
                                star = Parser.LC;
                                oprtr = operand[1];
                                opr2 = operand.Substring(2);

                                Tokenizer.TokenKinds valid;
                                Tokenizer.GetTokenKind(opr2, out valid);

                                if (valid == Tokenizer.TokenKinds.Label_Or_Command)
                                {
                                    if (2 <= opr2.Length && opr2.Length <= 32)
                                    {
                                        if (symb.ContainsSymbol(opr2) &&
                                            symb.GetSymbol(opr2).usage == Usage.EQUATED)
                                        {
                                            opr2 = Convert.ToInt32(symb.GetSymbol(opr2).val, 16).ToString();
                                        }
                                    }
                                    else
                                    {
                                        // error:label is too long
                                    }
                                }
                                else if (valid == Tokenizer.TokenKinds.Number)
                                {
                                    if (!(0 < Convert.ToInt32(opr2) && Convert.ToInt32(opr2) < 1023))
                                    {
                                        // error, the number is out of bounds
                                    }
                                }
                                else
                                {
                                    //error, must be number or previously equated symbol
                                    Console.WriteLine("WAAATTTTTT");
                                }

                                Tokenizer.GetTokenKind(opr2, out valid);

                                if (valid == Tokenizer.TokenKinds.Number)
                                {
                                    int result = -1;
                                    // if the method gets here, then it's using a number or
                                    // previously equated symbol that we can deal with
                                    switch (oprtr)
                                    {
                                        case '+':
                                            {
                                                result = Convert.ToInt32(star, 16) + Convert.ToInt32(opr2);
                                            } break;
                                        case '-':
                                            {
                                                result = Convert.ToInt32(star, 16) - Convert.ToInt32(opr2);
                                            } break;
                                        default:
                                            {
                                                // error invalid operator in expression
                                            } break;
                                    }

                                    if (0 <= result && result <= 1023)
                                    {
                                        operand = result.ToString();
                                    }
                                    else
                                    {
                                        // error: computation out of bounds
                                    }
                                }
                            }
                            else
                            {
                                //error invalid operand expression
                            }
                        } break;

                    case Expressions.EQUe:
                        {

                        } break;

                    case Expressions.ADCe:
                        {

                        } break;
                }
            }
        }
    }
}

