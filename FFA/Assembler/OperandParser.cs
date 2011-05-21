using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Assembler
{
    /**
     * Parses the operand field into usable information by the assembler.
     */
    public static class OperandParser
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

        /**
         * The possible types of expressions based on how they are being used.
         */
        public enum Expressions
        {
            /**
             * Can be used in instructions.
             */
            Operand,

            /**
             * Can be used in the EQU/EQUe directive.
             */
            EQU,

            /**
             * Can be used in the ADC/ADCe directive.
             */
            ADC
        }

        /**
         * Parse an operand and fill the intermediate line with found data.
         *
         * @param line the line (string) to parse
         * @param interLine the intermediate line object to fill
         * @param symb symbol table reference
         * @param bits number of bits to pad to, if applicable
         * @refcode S2.4
         * @errtest
         * @errmsg ES.15
         * @author Mark Mathis
         * @creation April 10, 2011
         * @modlog
         *  - April 18, 2011 - Jacob - Catch exceptions on parsing issues.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void ParseOperand(ref string line, ref IntermediateLine interLine, ref SymbolTable symb, int bits)
        {
            // get the next token
            string token;
            Tokenizer.TokenKinds tokenKind;
            Tokenizer.GetNextToken(ref line, out token, out tokenKind);

            Logger.Log("Parsing operand " + token + " of kind " + tokenKind.ToString(), "OperandParser");

            string operand = null;
            Literal litOperand = Literal.NONE;

            try {
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
                    operand = token;
                    litOperand = Literal.EXPRESSION;
                }
                // anything else is invalid
                else
                {
                    litOperand = Literal.NONE;
                    operand = "_ERROR";
                    interLine.AddError(Errors.Category.Serious, 15);
                }
            }
            catch (Exception)
            {
                litOperand = Literal.NONE;
                operand = "_ERROR";
                interLine.AddError(Errors.Category.Serious, 15);
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
         * @param inOper the operand to parse
         * @param outOper the numerical operand from the literal, that is, the part after the X=
         *                  will be in hexadecimal
         * @param litType the type of the operand, that is, X, B, etc.
         * @param bits the bit length to store as in hex
         * @refcode S2.4.2
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
         *  - April 18, 2011 - Jacob - Fix character parsing.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseLiteralOperand(string inOper, out string outOper, out Literal litType, int bits)
        {
            Logger.Log("Parsing literal operand " + inOper, "OperandParser");

            int op;
            outOper = inOper.Substring(2);
            litType = OperandParser.Literal.UNKNOWN;

            switch (inOper[0])
            {
                case 'X':
                case 'x':
                    {
                        Logger.Log("literal operand is hex", "Parser");
                        litType = Literal.X;
                        outOper = outOper.ToUpper();

                        try
                        {
                            Convert.ToInt32(outOper, 16);
                        }
                        catch (FormatException)
                        {
                            litType = Literal.UNKNOWN;
                        }
                    } break;

                case 'B':
                case 'b':
                    {
                        Logger.Log("literal operand is binary", "Parser");
                        litType = Literal.B;
                        op = Convert.ToInt32(inOper.Substring(2), 2);
                        outOper = Convert.ToString(op, 16).ToUpper();

                        try
                        {
                            Convert.ToInt32(outOper, 2);
                        }
                        catch (FormatException)
                        {
                            litType = Literal.UNKNOWN;
                        }

                    } break;

                case 'I':
                case 'i':
                    {
                        Logger.Log("literal operand is integer", "Parser");
                        litType = Literal.I;
                        op = Convert.ToInt32(inOper.Substring(2));
                        op = BinaryHelper.ConvertNumber(op, bits);
                        outOper = Convert.ToString(op, 16).ToUpper();

                        try
                        {
                            Convert.ToInt32(outOper);
                        }
                        catch (FormatException)
                        {
                            litType = Literal.UNKNOWN;
                        }

                    } break;
                case 'C':
                case 'c':
                    {
                        Logger.Log("literal operand is character: " + inOper, "Parser");
                        litType = Literal.C;
                        string str = inOper.Substring(2);
                        // ensure the first and last characters are quotes
                        if (str[0] != '\'' || str[str.Length - 1] != '\'')
                        {
                            throw new System.ArgumentException();
                        }
                        // strip them
                        str = str.Substring(1, str.Length - 2);
                        // two-character
                        if (str.Length == 2) {
                            // convert to a binary string
                            str = Convert.ToString((short) str[0], 2).PadLeft(8, '0') +
                                  Convert.ToString((short) str[1], 2).PadLeft(8, '0');
                            outOper = Convert.ToString(Convert.ToInt32(str, 2), 16);
                        }
                        // single character
                        else if (str.Length == 1) {
                            str = Convert.ToString((short) str[0], 2).PadLeft(8, '0');
                            outOper = Convert.ToString(Convert.ToInt32(str, 2), 16).PadRight(4, '0');
                        }
                        // anything else is invalid
                        else
                        {
                            throw new System.ArgumentException();
                        }
                    } break;
            }

            Logger.Log(String.Format("Literal operand parsed as {0} {1}", litType, outOper), "Parser");
        }

        /**
         * Checks to see if the instruction's operand field has valid syntax.
         * 
         * @refcode S2.4.0
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
         * Reads an expression and evaluates it as much as it can. If the parsing is
         * successful, the operand variable is replaced with the most fully parsed 
         * version of the expression possible.  If the expression cannot be parsed due
         * to an error in the expression, thie function returns false and an error is
         * added to the passed in IntermediateLine parameter.
         * 
         * @refcode EX1, EX2, EX3
         * @errtest N/A
         * @errmsg ES.2, ES.19, ES.20, ES.22, ES.23, ES.24, ES.27, ES.30, ES.31, ES.32, ES.33
         * @author Mark Mathis
         * @creation April 18, 2011
         * @modlog
         *  - April 18, 2011 - Mark - Parses operand expressions (EX1).
         *  - April 19, 2011 - Mark - Parses EQU expressions (EX2).
         *  - April 19, 2011 - Mark - Parses ADC expressions (EX3).
         *  - April 19, 2011 - Mark - Errors are caught and reported in all expressions.
         *  - May 13, 2011 - Mark - Added rec and modifications parameters.
         *  - May 14, 2011 - Mark - Now keeps track of adjustments required for pass 2.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         * 
         * @param operand the expression to parse, will contain the outgoing value of the expression
         * @param type the type of expression to be parsed
         * @param interLine the intermediate line containing this expression
         * @param symb the symbol table for this source code file
         * @param rec the modification record for keeping track of adjustments in instruction expressions
         *        and ADC/ADCe expressions
         * @param modifications the number of adjustments needed for a EQU/EQUe expression
         * @param maxOperators the maximum number of operators allowed in this type of expression
         * 
         * @return true if the expression is successfully parsed and evaluated.
         *         false if an error is found.
         */
        public static bool ParseExpression(ref string operand,
                                           OperandParser.Expressions type,
                                           IntermediateLine interLine,
                                           ref SymbolTable symb,
                                           ModificationRecord rec,
                                           out int modifications,
                                           int maxOperators = 1)
        {
            modifications = 0;
            if (operand != null && operand.Length > 0)
            {
                // if the expression is just a star, take care of it and return
                if (operand.Length == 1 && operand[0] == '*')
                {
                    modifications++;
                    rec.AddAdjustment(true, symb.ProgramName);
                    operand = interLine.ProgramCounter;
                    return true;
                }

                char[] validOperators = { '+', '-' };
                // if there are too many operators, give an error
                List<string> operands = operand.Split(validOperators, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (operands.Count - 1 > maxOperators)
                {
                    // error, too many operators
                    interLine.AddError(Errors.Category.Serious, 22);
                    return false;
                }

                // check for absolute ADC/ADCe
                bool absolute = false;
                if (operand[0] == '+' && type == Expressions.ADC)
                {
                    absolute = true;
                    operand = operand.Substring(1);
                }

                List<char> operators = new List<char>();

                int pos = operand.IndexOfAny(validOperators, 0);

                while (pos != -1)
                {
                    operators.Add(operand[pos]);
                    pos = operand.IndexOfAny(validOperators, pos + 1);
                }

                // check that we have the correct number of operands
                if (operators.Count != operands.Count - 1)
                {
                    // error, wrong number of operators
                    interLine.AddError(Errors.Category.Serious, 24);
                    return false;
                }

                // it can't always be that easy
                switch (type)
                {
                    case OperandParser.Expressions.Operand:
                        {

                            char oprtr;
                            string opr2;
                            string star;

                            if (operand[0] == '*')
                            {
                                star = interLine.ProgramCounter;
                                oprtr = operand[1];
                                opr2 = operand.Substring(2);
                                rec.AddAdjustment(true, symb.ProgramName);

                                Tokenizer.TokenKinds valid;
                                Tokenizer.GetTokenKind(opr2, out valid);

                                if (valid == Tokenizer.TokenKinds.Label_Or_Command)
                                {
                                    if (2 <= opr2.Length && opr2.Length <= 32)
                                    {
                                        if (symb.ContainsSymbol(opr2))
                                        {
                                            if (symb.GetSymbol(opr2).usage == Usage.EQUATED)
                                            {
                                                opr2 = Convert.ToInt32(symb.GetSymbol(opr2).val, 16).ToString();
                                            }
                                            else if (symb.GetSymbol(opr2).usage == Usage.LABEL)
                                            {
                                                opr2 = Convert.ToInt32(symb.GetSymbol(opr2).lc, 16).ToString();
                                                rec.AddAdjustment(oprtr == '+', symb.ProgramName);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // error:label is too long
                                        Logger.Log("ERROR: ES.2 encountered", "OperandParser");
                                        interLine.AddError(Errors.Category.Serious, 2);
                                        return false;
                                    }
                                }
                                else if (valid == Tokenizer.TokenKinds.Number)
                                {
                                    if (!(0 <= Convert.ToInt32(opr2) && Convert.ToInt32(opr2) <= 1023))
                                    {
                                        // error, the number is out of bounds
                                        Logger.Log("ERROR: ES.28 encountered", "OperandParser");
                                        interLine.AddError(Errors.Category.Serious, 27);
                                        return false;
                                    }
                                }
                                else if (opr2 == "*")
                                {
                                    // error: star used multiple times
                                    interLine.AddError(Errors.Category.Serious, 19);
                                    return false;
                                }
                                else
                                {
                                    //error, must be number or previously equated symbol
                                    Logger.Log("ERROR: ES.29 encountered", "OperandParser");
                                    interLine.AddError(Errors.Category.Serious, 27);
                                    return false;
                                }

                                Tokenizer.GetTokenKind(opr2, out valid);

                                if (valid == Tokenizer.TokenKinds.Number)
                                {
                                    int result = -1;
                                    // if the method gets here, then it's using a number or
                                    // symbol we can deal with
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
                                                Logger.Log("ERROR: ES.30 encountered", "OperandParser");
                                                interLine.AddError(Errors.Category.Serious, 30);
                                                return false;
                                            }
                                    }

                                    if (0 <= result && result <= 1023)
                                    {
                                        operand = Convert.ToString(result,16);
                                    }
                                    else
                                    {
                                        // error: computation out of bounds
                                        Logger.Log("ERROR: ES.27 encountered", "OperandParser");
                                        interLine.AddError(Errors.Category.Serious, 27);
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                //error invalid operand expression
                                Logger.Log("ERROR: ES.31 encountered", "OperandParser");
                                interLine.AddError(Errors.Category.Serious, 31);
                                return false;
                            }
                        } break;

                    case Expressions.EQU:
                        {
                            for (int i = 0; i < operands.Count; i++)
                            {
                                string label = operands[i];
                                Tokenizer.TokenKinds tokenkind;

                                Tokenizer.GetTokenKind(label, out tokenkind);

                                if (label == "*")
                                {
                                    if (i == 0)
                                    {
                                        operands[i] = Convert.ToInt32(Parser.LC, 16).ToString();
                                        modifications++;
                                    }
                                    else
                                    {
                                        // error, star must be first operand
                                        Logger.Log("ERROR: invalid star notation in expression.", "OperandParser");
                                        interLine.AddError(Errors.Category.Serious, 19);
                                        return false;
                                    }
                                }
                                else if (symb.ContainsSymbol(label))
                                {
                                    Symbol operSym = symb.GetSymbol(label);

                                    if (operSym.usage == Usage.EQUATED)
                                    {
                                        operands[i] = Convert.ToInt32(operSym.val, 16).ToString();
                                        modifications += operSym.relocations;
                                    }
                                    else if (operSym.usage == Usage.LABEL)
                                    {
                                        operands[i] = Convert.ToInt32(operSym.lc, 16).ToString();
                                        modifications++;
                                    }
                                    else
                                    {
                                        // error, can only use equated symbols or local references
                                        Logger.Log("ERROR: ES.32 encountered", "OperandParser");
                                        interLine.AddError(Errors.Category.Serious, 32);
                                        return false;
                                    }
                                }
                                else if (tokenkind == Tokenizer.TokenKinds.Number)
                                {
                                    // this needs to be caught, so the next else doesn't get
                                    // tripped. but there's nothing to do herp.
                                    // I will tell you stories of my people.
                                }
                                else
                                {
                                    // undefined symbol
                                    Logger.Log("ERROR: ES.20 encountered", "OperandParser");
                                    interLine.AddError(Errors.Category.Serious, 20);
                                    return false;
                                }
                            }

                            operands.Reverse();
                            operators.Reverse();

                            string possibleOperand = EvaluateExpression(new Stack<string>(operands), new Stack<char>(operators));

                            if (0 <= int.Parse(possibleOperand) && int.Parse(possibleOperand) <= 1023)
                            {
                                operand = Convert.ToString(int.Parse(possibleOperand), 16);
                            }
                            else
                            {
                                // error, calculation must be within the range of 0 to 1023
                                Logger.Log("ERROR: ES.27 encountered", "OperandParser");
                                interLine.AddError(Errors.Category.Serious, 27);
                                return false;
                            }
                        } break;

                    case Expressions.ADC:
                        {
                            bool noLabels = true;
                            for (int i = 0; i < operands.Count; i++)
                            {
                                string label = operands[i];

                                if (label == "*")
                                {
                                    noLabels = false;
                                    if (i == 0)
                                    {
                                        operands[i] = Convert.ToInt32(interLine.ProgramCounter,16).ToString();
                                        rec.AddAdjustment(true, symb.ProgramName);
                                        modifications++;
                                    }
                                    else
                                    {
                                        // error, star must be first operand
                                        Logger.Log("ERROR: ES.19 encountered", "OperandParser");
                                        interLine.AddError(Errors.Category.Serious, 19);
                                        return false;
                                    }
                                }
                                else if (symb.ContainsSymbol(label))
                                {
                                    noLabels = false;
                                    Symbol operSym = symb.GetSymbol(label);

                                    if (operSym.usage == Usage.LABEL)
                                    {
                                        operands[i] = Convert.ToInt32(operSym.lc, 16).ToString();
                                        modifications++;
                                        if (i > 0)
                                        {
                                            rec.AddAdjustment(operators[i - 1] == '+', symb.ProgramName);
                                        }
                                        else
                                        {
                                            rec.AddAdjustment(true, symb.ProgramName);
                                        }
                                    }
                                    else if (operSym.usage == Usage.EXTERNAL)
                                    {
                                        noLabels = false;
                                        operands[i] = "0";
                                        modifications++;
                                        if (i > 0)
                                        {
                                            rec.AddAdjustment(operators[i - 1] == '+', operSym.rlabel);
                                        }
                                        else
                                        {
                                            rec.AddAdjustment(true, operSym.rlabel);
                                        }
                                    }
                                    else
                                    {
                                        // error: symbols can only be external, local reference
                                        // in ADC/ADCe expressions
                                        Logger.Log("ERROR: ES.33 encountered", "OperandParser");
                                        interLine.AddError(Errors.Category.Serious, 33);
                                        return false;
                                    }
                                }
                            }

                            if (noLabels)
                            {
                                rec.AddAdjustment(true, symb.ProgramName);
                                modifications++;
                            }

                            bool allNumbers = true;

                            foreach (string op in operands)
                            {
                                Tokenizer.TokenKinds tokenKind;

                                Tokenizer.GetTokenKind(op, out tokenKind);

                                if (tokenKind == Tokenizer.TokenKinds.Number)
                                {
                                    allNumbers = allNumbers && true;
                                }
                                else
                                {
                                    allNumbers = false;
                                }
                            }

                            if (allNumbers)
                            {
                                operands.Reverse();
                                operators.Reverse();
                                operand = EvaluateExpression(new Stack<string>(operands), new Stack<char>(operators));
                                operand = Convert.ToString(Convert.ToInt32(operand), 16);
                            }
                            else
                            {
                                operand = operands[0];
                                for (int i = 0; i + 1 < operands.Count; i++)
                                {
                                    operand += operators[i] + operands[i + 1];
                                }
                            }
                            if (absolute)
                                modifications = 0;
                            } break;
                        }
                }
            return true;
            }

        /**
         * Does basic math between two stacks, one with operands and one with operators.
         * Allows arbitrarily long strings of expressions to be evaluated.
         * To compute 5+4-8, the stacks would be passed in like so: < /br>
         * operands: 5 4 8 < /br>
         * operators: + -
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark Mathis
         * @creation April 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         * 
         * @param operands a stack with the leftmost operand at the top of the stack
         * @param operators a stack with the leftmost operator at the top of the stack
         * 
         * @return returns the string version of the evaluated expression.
         */
        static string EvaluateExpression(Stack<string> operands, Stack<char> operators)
        {
            while (operators.Count > 0)
            {
                string op1 = operands.Pop();
                string op2 = operands.Pop();
                char op = operators.Pop();

                if (op == '+')
                {
                    op1 = (int.Parse(op1) + int.Parse(op2)).ToString();
                }
                else if (op == '-')
                {
                    op1 = (int.Parse(op1) - int.Parse(op2)).ToString();
                }
                else
                {
                    // error invalid operator
                    break;
                }
                operands.Push(op1);
            }
            return operands.Pop().ToString();
        }
    }
}

