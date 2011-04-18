using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assembler
{
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
            Logger288.Log("Parsing directive on line " + interLine.SourceLineNumber, "DirectiveParser");

            OperandParser.ParseOperand(ref line, ref interLine, ref symb, 16);

            // This will decide which directive is in this line and how it should
            // be handled by the parser.
            string currentDirective = interLine.Directive.ToUpper();
            switch (currentDirective)
            {
                case "START":
                    {
                        ParseStart(ref line, ref interLine, ref symb);
                    } break;
                case "RESET":
                    {
                        ParseReset(ref line, ref interLine, ref symb);
                    } break;
                case "EQU":
                    {
                        ParseEqu(ref line, ref interLine, ref symb);
                    } break;
                case "EQUE":
                    {
                        ParseEque(ref line, ref interLine, ref symb);
                    } break;
                case "ENTRY":
                    {
                    } break;
                case "EXTRN":
                    {
                    } break;
                case "END":
                    {
                        ParseEnd(ref line, ref interLine, ref symb);
                    } break;
                case "DAT":
                    {
                    } break;
                case "ADC":
                    {
                    } break;
                case "ADCE":
                    {
                    } break;
                case "NOP":
                    {
                    } break;
                default:
                    {
                    } break;
            }

            Logger288.Log("Finished parsing directive on line " + interLine.SourceLineNumber, "DirectiveParser");
        }

        /**
         * Parses the start directive, properly assigning the operand of start as the
         * starting location counter.
         *
         * @param line the line containing the start directive
         * @param interLine the intermediate line to process
         * @return the IntermediateLine of this line
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
        private static void ParseStart(ref string line, ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger288.Log("Parsing START directive", "DirectiveParser");

            // expecting operand to be the value of the location counter
            Parser.LC = interLine.DirectiveOperand;

            // update the symbol in the symbol table
            Symbol start = symb.RemoveSymbol(interLine.Label);
            start.usage = Usage.PRGMNAME;
            start.lc = null;
            symb.AddSymbol(start);

            Logger288.Log("Finished parsing START directive", "DirectiveParser");
        }

        /**
         * Parses the end directeive, ensuring that the end operand is the same as
         * the start directive's rlabel.
         *
         * @param line the line containing the end directive
         * @param interLine the intermediate line to process
         * @return The IntermediateLine of this line
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
        private static void ParseEnd(ref string line, ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger288.Log("Parsing END directive", "DirectiveParser");

            // check to see if the operand of the END directive matches the program name

            if (!(symb.ContainsSymbol(interLine.DirectiveOperand) && 
                symb.GetSymbol(interLine.DirectiveOperand).usage == Usage.PRGMNAME))
            {
                // error things
                interLine.AddError(Errors.Category.Fatal, 5);
            }

            Logger288.Log("Finished parsing END directive.", "DirectiveParser");
        }

        private static void ParseReset(ref string line, ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger288.Log("Parsing RESET directive", "DirectiveParser");

            // the operand of the RESET directive must either be an equated label
            // or a literal number.
            if ((symb.ContainsSymbol(interLine.DirectiveOperand) && 
                symb.GetSymbol(interLine.DirectiveOperand).usage == Usage.EQUATED) || 
                (interLine.DirectiveLitOperand == OperandParser.Literal.Number))
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

            Logger288.Log("Finished parsing RESET directive", "DirectiveParser");
        }

        private static void ParseEqu(ref string line, ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger288.Log("Parsing EQU directive", "DirectiveParser");

            if ((symb.ContainsSymbol(interLine.DirectiveOperand) &&
                symb.GetSymbol(interLine.DirectiveOperand).usage == Usage.EQUATED) ||
                (interLine.DirectiveLitOperand == OperandParser.Literal.Number))
            {
                symb.AddSymbol(interLine.Label, null, Usage.EQUATED, interLine.DirectiveOperand);
            }
            else
            {

            }

            Logger288.Log("Finished parsing EQU directive", "DirectiveParser");
        }

        private static void ParseEque(ref string line, ref IntermediateLine interLine, ref SymbolTable symb)
        {
            if ((symb.ContainsSymbol(interLine.DirectiveOperand) &&
                symb.GetSymbol(interLine.DirectiveOperand).usage == Usage.EQUATED) ||
                (interLine.DirectiveLitOperand == OperandParser.Literal.Number))
            {
                
            }
        }

        private static void ParseEntry(ref string line, ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger288.Log("Parsing ENTRY directive", "DirectiveParser");

            
            if (symb.ContainsSymbol(interLine.DirectiveOperand))
            {
                Symbol tempsym = symb.RemoveSymbol(interLine.DirectiveOperand);
                tempsym.usage = Usage.ENTRY;
                symb.AddSymbol(tempsym);
            }
            else
            {
                symb.AddSymbol(interLine.DirectiveOperand, null, Usage.ENTRY);
            }

            Logger288.Log("Finished parsing ENTRY directive", "DirectiveParser");
        }
    }
}
