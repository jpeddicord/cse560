using System;

namespace Assembler
{
    static class OperandParser
    {
        /**
         * Document!
         */
        public enum Type {
            elabel,
            mlabel,
            rlabel,
            olabel,
            n,
            nn,
            nnn,
            nmax,
            nnnnnn,
            none
        }

        /**
         * Check if a directive operand fits within the given type.
         * TODO: DOC
         */
        public static bool IsValidDirectiveOperand(string lit, Type type)
        {
            // bail early if passed in an error literal
            if (lit == "_ERROR")
            {
                return false;
            }

            return true;
        }

        /**
         * Check if an instruction operand fits within the given type.
         */
        public static bool IsValidInstructionOperand(string lit, Type type)
        {
            // TODO
            return true;
        }

        /**
* Parse an operand and fill the intermediate line with found data.
* XXX: DOC
*/
        public static void ParseOperand(ref string line, ref IntermediateLine interLine)
        {
            // get the next token
            string token;
            Tokenizer.TokenKinds tokenKind;
            Tokenizer.GetNextToken(ref line, out token, out tokenKind);

            string operand;
            string litOperand = null;

            // if it's a label, then just set the token directly
            if (tokenKind == Tokenizer.TokenKinds.Label_Or_Command)
            {
                operand = token;
            }
            // if it's a number, convert to hex and store
            else if (tokenKind == Tokenizer.TokenKinds.Number)
            {
                operand = Convert.ToString(int.Parse(token), 16).ToUpper();
            }
            // do further processing for literals
            else if (tokenKind == Tokenizer.TokenKinds.Literal)
            {
                ParseLiteralOperand(token, out operand, out litOperand);
            }
            // anything else is invalid
            else
            {
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
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         * 
         * @param inOper the operand to parse
         * @param outOper the numerical operand from the literal, that is, the part after the X=
         *                  will be in hexadecimal
         * @param litType the type of the operand, that is, X, B, etc.
         */
        public static void ParseLiteralOperand(string inOper, out string outOper, out string litType)
        {
            Logger288.Log("Parsing literal operand " + inOper, "Parser");

            int op;
            outOper = inOper.Substring(2);
            litType = inOper.Substring(0, 2);

            switch (inOper[0])
            {
                case 'X':
                case 'x':
                    {
                        Logger288.Log("literal operand is hex", "Parser");
                        op = Convert.ToInt32(inOper.Substring(2), 16);
                        if (op < 0)
                        {
                            op = BinaryHelper.ConvertNumber(op, 10);
                        }
                        outOper = Convert.ToString(op, 16).ToUpper();
                    } break;

                case 'B':
                case 'b':
                    {
                        Logger288.Log("literal operand is binary", "Parser");
                        op = Convert.ToInt32(inOper.Substring(2), 2);
                        if (op < 0)
                        {
                            op = BinaryHelper.ConvertNumber(op, 10);
                        }
                        outOper = Convert.ToString(op, 16).ToUpper();
                    } break;

                case 'I':
                case 'i':
                    {
                        Logger288.Log("literal operand is integer", "Parser");
                        op = Convert.ToInt32(inOper.Substring(2));
                        if (-512 < op && op < 511)
                        {
                            if (op < 0)
                            {
                                op = BinaryHelper.ConvertNumber(op);
                            }
                            outOper = Convert.ToString(op, 16).ToUpper();
                        }
                        else
                        {
                            outOper = "_OUT OF BOUNDS";
                        }
                    } break;
                case 'C':
                case 'c':
                    {
                        Logger288.Log("literal operand is character", "Parser");
                        // fix this: parse integers more properly.
                        outOper = Convert.ToString(Convert.ToInt32(((int)inOper[3]) << 2), 16).ToUpper();
                    } break;
            }

            Logger288.Log(String.Format("Literal operand parsed as {0} {1}", litType, outOper), "Parser");
        }
    }
}

