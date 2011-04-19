using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assembler
{
    /**
     * Parses the operation section of the line if it has a directive.
     */
    static class DirectiveParser
    {
        /**
         * Parses the operation section of the line if it has a Directive.
         *
         * @param line current line to parse
         * @param interLine the line as a single line in the intermediate file
         * @param symb symbol table reference
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         *  - April  9, 2011 -  Mark - ParseDirective properly parses directives.
         *  - April  9, 2011 -  Mark - Uses new ParseLiteralOperand format.
         *  - April 12, 2011 - Jacob - Factor out operand parsing.
         *  - April 14, 2011 -  Mark - Moved into DirectiveParser class.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void ParseDirective(ref string line, ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing directive on line " + interLine.SourceLineNumber, "DirectiveParser");

            OperandParser.ParseOperand(ref line, ref interLine, ref symb, 16);

            // This will decide which directive is in this line and how it should
            // be handled by the parser.
            string currentDirective = interLine.Directive.ToUpper();
            switch (currentDirective)
            {
                case "START":
                    {
                        ParseStart(ref interLine, ref symb);
                    } break;
                case "RESET":
                    {
                        ParseReset(ref interLine, ref symb);
                    } break;
                case "EQU":
                    {
                        ParseEqu(ref interLine, ref symb);
                    } break;
                case "EQUE":
                    {
                         ParseEque(ref interLine, ref symb);
                    } break;
                case "ENTRY":
                    {
                        ParseEntry(ref interLine, ref symb);
                    } break;
                case "EXTRN":
                    {
                        ParseExtrn(ref interLine, ref symb);
                    } break;
                case "END":
                    {
                        ParseEnd(ref interLine, ref symb);
                    } break;
                case "DAT":
                    {
                        ParseDat(ref interLine, ref symb);
                    } break;
                case "ADC":
                    {
                        // ParseAdc(ref interLine, ref symb);
                    } break;
                case "ADCE":
                    {
                        // ParseAdce(ref interLine, ref symb);
                    } break;
                case "NOP":
                    {
                    } break;
                default:
                    {
                    } break;
            }

            Logger.Log("Finished parsing directive on line " + interLine.SourceLineNumber, "DirectiveParser");
        }

        /**
         * Parses the start directive, properly assigning the operand of start as the
         * starting location counter.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         *
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         *  - April  9, 2011 - Mark - Parses the START directive, correctly setting the LC.
         *  - April 14, 2011 - Mark - Moved into DirectiveParser class.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseStart(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing START directive", "DirectiveParser");

            // expecting operand to be the value of the location counter
            Parser.LC = interLine.DirectiveOperand;

            // update the symbol in the symbol table
            Symbol start = symb.RemoveSymbol(interLine.Label);
            start.usage = Usage.PRGMNAME;
            start.lc = null;
            symb.AddSymbol(start);

            Logger.Log("Finished parsing START directive", "DirectiveParser");
        }

        /**
         * Parses the end directive, ensuring that the end operand is the same as
         * the start directive's rlabel.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         *
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         *  - April  9, 2011 - Mark - Not sure if needed.
         *  - April 14, 2011 - Mark - Moved into DirectiveParser class.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseEnd(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing END directive", "DirectiveParser");

            // check to see if the operand of the END directive matches the program name

            if (!(symb.ContainsSymbol(interLine.DirectiveOperand) &&
                symb.GetSymbol(interLine.DirectiveOperand).usage == Usage.PRGMNAME))
            {
                // error things
                interLine.AddError(Errors.Category.Fatal, 5);
            }

            Logger.Log("Finished parsing END directive.", "DirectiveParser");
        }

		/**
         * Parses the reset directive, ensuring that the reset operand is a number
         * that is higher than any previously or currently used location counter.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 15, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseReset(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing RESET directive", "DirectiveParser");

            // the operand of the RESET directive must either be an equated label
            // or a literal number.
            if ((symb.ContainsSymbol(interLine.DirectiveOperand) &&
                symb.GetSymbol(interLine.DirectiveOperand).usage == Usage.EQUATED) ||
                (interLine.DirectiveLitOperand == OperandParser.Literal.NUMBER))
            {
                int curLC = Convert.ToInt32(Parser.LC, 16);
                int newLC = Convert.ToInt32(interLine.DirectiveOperand, 16);
                if (curLC < newLC)
                {
                    Parser.LC = interLine.DirectiveOperand;
                }
                else
                {
                    // error, attempt to use a previously used LC value
                    interLine.AddError(Errors.Category.Serious, 8);
                }
            }
            else
            {
                // error invalid value
                interLine.AddError(Errors.Category.Serious, 10);
            }

            Logger.Log("Finished parsing RESET directive", "DirectiveParser");
        }

        /**
         * Parses the equ directive, ensuring that the operand is a valid value for an
         * equated symbol.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 15, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseEqu(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing EQU directive", "DirectiveParser");

            if (symb.ContainsSymbol(interLine.Label))
            {
                Symbol equSym = symb.RemoveSymbol(interLine.Label);
                equSym.lc = null;
                if (interLine.DirectiveLitOperand == OperandParser.Literal.NUMBER)
                {
                    // check that number is in bounds
                    int num = BinaryHelper.HexToInt(interLine.DirectiveOperand, 10);
                    if (0 < num && num < 1023)
                    {
                        equSym.usage = Usage.EQUATED;
                        equSym.val = interLine.DirectiveOperand;
                    }
                    else
                    {
                        // error: out of bounds
                    }
                }
                else if (symb.ContainsSymbol(interLine.DirectiveOperand) &&
                    symb.GetSymbol(interLine.DirectiveOperand).usage == Usage.EQUATED)
                {
                    // do stuff with the symbol
                    string symVal = symb.GetSymbol(interLine.DirectiveOperand).val;
                    OperandParser.ParseExpression(ref symVal, OperandParser.Expressions.Operator, ref symb);
                    equSym.usage = Usage.EQUATED;
                    equSym.val = symVal;
                }
                else if (interLine.DirectiveLitOperand == OperandParser.Literal.EXPRESSION)
                {
                    string oper = interLine.DirectiveOperand;
                    OperandParser.ParseExpression(ref oper, OperandParser.Expressions.EQU, ref symb);
                    equSym.usage = Usage.EQUATED;
                    equSym.val = oper;
                }
                else
                {
                    // error: invalid operand for equ
                }
                symb.AddSymbol(equSym);
            }
            else
            {

            }

            Logger.Log("Finished parsing EQU directive", "DirectiveParser");
        }

        /**
         * Parses the eque directive, ensuring that the operand is a valid value for an
         * equated symbol using extended expressions.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 17, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseEque(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            string oper = interLine.DirectiveOperand;
            OperandParser.ParseExpression(ref oper, OperandParser.Expressions.EQU, ref symb, 3);
        }

        /**
         * Parses the entry directive, ensuring that the operand is a valid value
         * for an equated symbol using extended expressions.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 17, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseEntry(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing ENTRY directive", "DirectiveParser");


            if (symb.ContainsSymbol(interLine.DirectiveOperand) &&
                symb.GetSymbol(interLine.DirectiveOperand).usage != Usage.EQUATED)
            {
                Symbol tempsym = symb.RemoveSymbol(interLine.DirectiveOperand);
                tempsym.usage = Usage.ENTRY;
                symb.AddSymbol(tempsym);
            }
            else
            {
                symb.AddSymbol(interLine.DirectiveOperand, null, Usage.ENTRY);
            }

            Logger.Log("Finished parsing ENTRY directive", "DirectiveParser");
        }

        /**
         * Parses the extrn directive, ensuring that operand is correct.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 17, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseExtrn(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing EXTRN directive", "DirectiveParser");

            if (!symb.ContainsSymbol(interLine.DirectiveOperand))
            {
                symb.AddSymbol(interLine.DirectiveOperand, null, Usage.EXTERNAL);
            }
            else
            {
                interLine.AddError(Errors.Category.Serious, 13);
            }

            Logger.Log("Finished parsing EXTRN directive", "DirectiveParser");
        }

        /**
         * Parses the dat directive, ensuring that the operand has proper syntax
         * and is correctly assigned to the current word of memory.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 18, 2011
         * @modlog
         *  - April 19, 2011 - Jacob - Fixed padding on values.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseDat(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing DAT directive", "DirectiveParser");

            if (interLine.DirectiveLitOperand != OperandParser.Literal.NONE &&
                interLine.DirectiveLitOperand != OperandParser.Literal.EXPRESSION &&
                interLine.DirectiveLitOperand != OperandParser.Literal.NUMBER &&
                interLine.DirectiveLitOperand != OperandParser.Literal.UNKNOWN)
            {
                if (interLine.Label != null)
                {
                    if (!symb.ContainsSymbol(interLine.Label))
                    {
                        symb.AddSymbol(interLine.Label, Parser.LC, Usage.LABEL, interLine.DirectiveOperand);
                    }
                }

                string val = Convert.ToString(Convert.ToInt32(interLine.DirectiveOperand, 16), 2);

                // assumed to be in correct representation; always pad to the left
                interLine.Bytecode = val.PadLeft(16, '0');
            }
            else
            {
                // error: invalid operand type
                interLine.AddError(Errors.Category.Serious, 14);
                interLine.NOPificate();
            }

            Logger.Log("Finished parsing DAT directive", "DirectiveParser");
        }

        /**
         * Parses the adc directive, ensuring that the operand has proper
         * syntax and type.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 18, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseAdc(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            throw new NotImplementedException();
        }

        /**
         * Parses the adce directive, ensuring that the operand has proper
         * syntax and type.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 18, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseAdce(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            throw new NotImplementedException();
        }
    }
}
