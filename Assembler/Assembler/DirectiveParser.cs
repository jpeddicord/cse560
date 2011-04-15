﻿using System;
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
         * 
         * @param line current line to parse.
         * @param interLine the line as a single line in the intermediate file.
         */
        public static void ParseDirective(ref string line, ref IntermediateLine interLine, ref SymbolTable symb)
        {
            Logger288.Log("Parsing directive on line " + interLine.SourceLineNumber, "DirectiveParser");

            // This will decide which directive is in this line and how it should
            // be handled by the parser.
            string currentDirective = interLine.Directive.ToUpper();
            switch (currentDirective)
            {
                case "START":
                    {
                        ParseStart(ref line, ref interLine);
                        Symbol start = symb.RemoveSymbol(interLine.Label);
                        start.usage = Usage.PRGMNAME;
                        symb.AddSymbol(start);
                    } break;
                case "RESET":
                    {
                    } break;
                case "EQU":
                    {
                    } break;
                case "EQUE":
                    {
                    } break;
                case "ENTRY":
                    {
                    } break;
                case "EXTRN":
                    {
                    } break;
                case "END":
                    {
                        ParseEnd(ref line, ref interLine);
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

            }

            // NOP doesn't have an operand
            if (interLine.Directive == "NOP")
            {
                // TODO: insert and "parse" a MOPER ADD,=0, and increment LC
                return;
            }

            Logger288.Log("Parsing directive operand on line " + interLine.SourceLineNumber, "DirectiveParser");

            // get the operand of the directive
            //OperandParser.ParseOperand(ref line, ref interLine);

            // RESET directive sets the LC
            if (interLine.Directive == "RESET")
            {
                //this.LC;
            }

            Logger288.Log("Finished parsing directive on line " + interLine.SourceLineNumber, "DirectiveParser");
        }

        /**
         * Parses the start directive, properly assigning the operand of start as the
         * starting location counter.
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
         * 
         * @param line the line containing the start directive
         * @return the IntermediateLine of this line
         */
        private static void ParseStart(ref string line, ref IntermediateLine interLine)
        {
            Logger288.Log("Parsing START directive", "DirectiveParser");

            OperandParser.ParseOperand(ref line, ref interLine);
            Parser.LC = interLine.DirectiveOperand;
            interLine.ProgramCounter = Parser.LC;

            Logger288.Log("Finished parsing START directive", "DirectiveParser");
        }

        /**
         * Parses the end directeive, ensuring that the end operand is the same as
         * the start directive's rlabel.
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
         * 
         * @param line the line containing the end directive
         * @param lineNum the line number of this line in the source code.
         * @return The IntermediateLine of this line
         */
        private static void ParseEnd(ref string line, ref IntermediateLine interLine)
        {
            Logger288.Log("Parsing END directive", "DirectiveParser");
            OperandParser.ParseOperand(ref line, ref interLine);

            Logger288.Log("Finished parsing END directive.", "DirectiveParser");
        }
    }
}
