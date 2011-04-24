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
         * @refcode DS2
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 9, 2011
         * @modlog
         *  - April  9, 2011 -  Mark - ParseDirective properly parses directives.
         *  - April  9, 2011 -  Mark - Uses new ParseLiteralOperand format.
         *  - April 12, 2011 - Jacob - Factor out operand parsing.
         *  - April 14, 2011 -  Mark - Moved into DirectiveParser class.
         *  - April 15, 2011 -  Mark - Factored out all parsing into separate methods.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void ParseDirective(ref string line, ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing directive on line " + interLine.SourceLineNumber, "DirectiveParser");

            if (interLine.Directive.ToUpper() != "NOP")
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
                        ParseAdc(ref interLine, ref symb);
                    } break;
                case "ADCE":
                    {
                        ParseAdce(ref interLine, ref symb);
                    } break;
                case "NOP":
                    {
                        interLine.NOPificate();
                        interLine.ProgramCounter = Parser.LC;
                        Parser.IncrementLocationCounter();
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
         * @refcode D1
         * @errtest
         *  N/A
         * @errmsg EF.06
         * @author Mark Mathis
         * @creation April 9, 2011
         * @modlog
         *  - April  9, 2011 - Mark - Parses the START directive, correctly setting the LC.
         *  - April 14, 2011 - Mark - Moved into DirectiveParser class.
         *  - April 17, 2011 - Mark - Catches errors in the start directive.
         *  - April 19, 2011 - Mark - Reports errors.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseStart(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing START directive", "DirectiveParser");

            // expecting operand to be the value of the location counter
            // check to see if operand is valid start value
            int startLC;
            if (interLine.DirectiveLitOperand == OperandParser.Literal.NUMBER)
            {
                string startOper = BinaryHelper.HexToInt(interLine.DirectiveOperand, 32).ToString();
                if (int.TryParse(startOper, out startLC))
                {
                    if (0 <= startLC && startLC <= 1023)
                    {
                        Parser.LC = interLine.DirectiveOperand;
                    }
                }
                else
                {
                    /*
                     * The operand could not be parsed as a number.
                     * This is here as well as below just in case the 
                     * Tokenizer has a hiccough and says that something
                     * that is not a number is a number.
                     */
                    Logger.Log("ERROR: EF.06 encountered", "DirectiveParser");
                    interLine.AddError(Errors.Category.Fatal, 6);
                    return;
                }

                // update the symbol in the symbol table
                Symbol start = symb.RemoveSymbol(interLine.Label);
                start.usage = Usage.PRGMNAME;
                start.lc = null;
                symb.AddSymbol(start);
            }
            else
            {
                // the operand could not be parsed as a number
                Logger.Log("ERROR: EF.06 encountered", "DirectiveParser");
                interLine.AddError(Errors.Category.Fatal, 6);
                return;
            }

            Logger.Log("Finished parsing START directive", "DirectiveParser");
        }

        /**
         * Parses the end directive, ensuring that the end operand is the same as
         * the start directive's rlabel.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         *
         * @refcode D7
         * @errtest
         *  N/A
         * @errmsg EF.05, EW.05
         * @author Mark Mathis
         * @creation April 9, 2011
         * @modlog
         *  - April  9, 2011 - Mark - Not sure if needed.
         *  - April 14, 2011 - Mark - Moved into DirectiveParser class.
         *  - April 17, 2011 - Mark - Actually checks that the operand is correct.
         *  - April 19, 2011 - Mark - Reports errors.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseEnd(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing END directive", "DirectiveParser");
            
            // check to see if this line has a label
            if (interLine.Label != null)
            {
                // the end directive should not have a label
                interLine.AddError(Errors.Category.Warning, 5);
            }

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
         * @refcode D2
         * @errtest
         *  N/A
         * @errmsg ES.08, ES.10, ES.25
         * @author Mark Mathis
         * @creation April 15, 2011
         * @modlog
         * - April 17, 2011 - Mark - Changes the location counter to the operand value.
         * - April 17, 2011 - Mark - Checks that the operand value is valid.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseReset(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing RESET directive", "DirectiveParser");

            if (interLine.Label != null)
            {
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
                        Logger.Log("ERROR: ES.08 encountered.", "DirectiveParser");
                        interLine.AddError(Errors.Category.Serious, 8);
                    }
                }
                else
                {
                    // error invalid value
                    Logger.Log("ERROR: ES.10 encountered.", "DirectiveParser");
                    interLine.AddError(Errors.Category.Serious, 10);
                }
            }
            else
            {
                // error, label is required for reset directive
                Logger.Log("ERROR: ES.25 encountered.", "DirectiveParser");
                interLine.AddError(Errors.Category.Serious,25);
            }

            Logger.Log("Finished parsing RESET directive", "DirectiveParser");
        }

        /**
         * Parses the equ directive, ensuring that the operand is a valid value for an
         * equated symbol.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         * @param maxOp maximum number of operations to process
         * 
         * @refcode D3
         * @errtest
         *  N/A
         * @errmsg ES.21, ES.22, ES.26
         * @author Mark Mathis
         * @creation April 15, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseEqu(ref IntermediateLine interLine, ref SymbolTable symb, int maxOp = 1)
        {
            Logger.Log("Parsing EQU directive", "DirectiveParser");
            bool success = true;

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
                        Logger.Log("ERROR: ES.26 encountered", "DirectiveParser");
                        interLine.AddError(Errors.Category.Serious, 26);
                        success = false;
                    }
                }
                else if (symb.ContainsSymbol(interLine.DirectiveOperand) &&
                    symb.GetSymbol(interLine.DirectiveOperand).usage == Usage.EQUATED)
                {
                    // do stuff with the symbol
                    equSym.usage = Usage.EQUATED;
                    equSym.val = symb.GetSymbol(interLine.DirectiveOperand).val;
                }
                else if (interLine.DirectiveLitOperand == OperandParser.Literal.EXPRESSION)
                {
                    string oper = interLine.DirectiveOperand;
                    success = OperandParser.ParseExpression(ref oper, OperandParser.Expressions.EQU, 
                                                  interLine, ref symb, maxOp);
                    equSym.usage = Usage.EQUATED;
                    equSym.val = oper;
                }
                else
                {
                    // error: invalid operand for equ
                    Logger.Log("ERROR: EW.21 encountered", "DirectiveParser");
                    interLine.AddError(Errors.Category.Serious, 21);
                    success = false;
                }

                if (success)
                {
                    // this needs to be checked here in case ParseExpression
                    // finds an error
                    int finval = Convert.ToInt32(equSym.val, 16);
                    if (!(0 <= finval && finval <= 1023))
                    {
                        // the final value of the equ is out of bounds
                        Logger.Log("ERROR: EW.22 encountered", "DirectiveParser");
                        interLine.AddError(Errors.Category.Serious, 22);
                        success = false;
                    }

                    symb.AddSymbol(equSym);
                }
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
         * @refcode D4
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 17, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseEque(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            // The expressions will be exactly the same for EQUe as
            // it will for EQU, EQUe can just have longer expressions
            ParseEqu(ref interLine, ref symb, 3);
        }

        /**
         * Parses the entry directive, ensuring that the operand is a valid value
         * for an equated symbol using extended expressions.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         * 
         * @refcode D5
         * @errtest
         *  N/A
         * @errmsg EW.05
         * @author Mark Mathis
         * @creation April 17, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseEntry(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing ENTRY directive", "DirectiveParser");

            // check for a label
            if (interLine.Label != null)
            {
                // entry doesn't expect a label
                Logger.Log("ERROR: EW.05 encountered", "DirectiveParser");
                interLine.AddError(Errors.Category.Warning, 5);
            }
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
         * @refcode D6
         * @errtest
         *  N/A
         * @errmsg ES.13, EW.05
         * @author Mark Mathis
         * @creation April 17, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseExtrn(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger.Log("Parsing EXTRN directive", "DirectiveParser");

            //check for label
            if (interLine.Label != null)
            {
                // extrn doesn't expect a label
                Logger.Log("ERROR: EW.05 encountered", "DirectiveParser");
                interLine.AddError(Errors.Category.Warning, 5);
            }

            if (!symb.ContainsSymbol(interLine.DirectiveOperand))
            {
                symb.AddSymbol(interLine.DirectiveOperand, null, Usage.EXTERNAL);
            }
            else
            {
                // cannot used variables defined in this program
                Logger.Log("ERROR: ES.13 encountered", "DirectiveParser");
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
         * @refcode D8
         * @errtest
         *  N/A
         * @errmsg ES.14
         * @author Mark Mathis
         * @creation April 18, 2011
         * @modlog
         *  - April 19, 2011 - Jacob - Fixed padding on values.
         *  - April 24, 2011 -  Mark - Commented out the part that put the value of the dat
         *                              into the symbol table until that can be made clearer
         *                              by Al.
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
                        symb.AddSymbol(interLine.Label, Parser.LC, Usage.LABEL); //,interLine.DirectiveOperand);
                    }
                    else
                    {
                        Symbol datSym = symb.RemoveSymbol(interLine.Label);
                        datSym.lc = Parser.LC;
                        //datSym.val = interLine.DirectiveOperand;
                        symb.AddSymbol(datSym);
                    }
                }

                string val = Convert.ToString(Convert.ToInt32(interLine.DirectiveOperand, 16), 2);

                // assumed to be in correct representation; always pad to the left
                interLine.Bytecode = val.PadLeft(16, '0');
            }
            else
            {
                // error: invalid operand type
                Logger.Log("ERROR: ES.14 encountered", "DirectiveParser");
                interLine.AddError(Errors.Category.Serious, 14);
                interLine.NOPificate();
            }

            // this should need to happen no matter what
            interLine.ProgramCounter = Parser.LC;
            Parser.IncrementLocationCounter();

            Logger.Log("Finished parsing DAT directive", "DirectiveParser");
        }

        /**
         * Parses the adc directive, ensuring that the operand has proper
         * syntax and type.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         * @param maxOper maximum number of operations to process
         * 
         * @refcode D8
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 18, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseAdc(ref IntermediateLine interLine, ref SymbolTable symb, int maxOper = 1)
        {
            if (interLine.Label == null)
            {
                string operand = interLine.DirectiveOperand;
                OperandParser.ParseExpression(ref operand, OperandParser.Expressions.ADC,
                                              interLine, ref symb, maxOper);
                interLine.DirectiveOperand = operand;
            }
            else if (symb.ContainsSymbol(interLine.Label))
            {
                Symbol adcSym = symb.RemoveSymbol(interLine.Label);
                string operand = interLine.DirectiveOperand;
                OperandParser.ParseExpression(ref operand, OperandParser.Expressions.ADC,
                                              interLine, ref symb, maxOper);
                adcSym.val = operand;
                symb.AddSymbol(adcSym);
            }

            interLine.ProgramCounter = Parser.LC;
            Parser.IncrementLocationCounter();
        }

        /**
         * Parses the adce directive, ensuring that the operand has proper
         * syntax and type.
         *
         * @param interLine the intermediate line to process
         * @param symb symbol table reference
         * 
         * @refcode D9
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 18, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void ParseAdce(ref IntermediateLine interLine, ref SymbolTable symb)
        {
            ParseAdc(ref interLine, ref symb, 3);
        }
    }
}
