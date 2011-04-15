using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assembler
{
    class DirectiveParser
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
        public static void ParseDirective(ref string line, ref IntermediateLine interLine)
        {
            Logger288.Log("Parsing directive on line " + interLine.SourceLineNumber, "Parser");

            // NOP doesn't have an operand
            if (interLine.Directive == "NOP")
            {
                // TODO: insert and "parse" a MOPER ADD,=0, and increment LC
                return;
            }

            Logger288.Log("Parsing directive operand on line " + interLine.SourceLineNumber, "Parser");

            // get the operand of the directive
            OperandParser.ParseOperand(ref line, ref interLine);

            // RESET directive sets the LC
            if (interLine.Directive == "RESET")
            {
                //this.LC;
            }

            Logger288.Log("Finished parsing directive on line " + interLine.SourceLineNumber, "Parser");
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
        private IntermediateLine ParseStart(string line)
        {
            Logger288.Log("Parsing START directive", "Parser");

            Parser.LC = start.DirectiveOperand;
            start.ProgramCounter = Parser.LC;

            Logger288.Log("Finished parsing START directive", "Parser");

            return start;
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
        private IntermediateLine ParseEnd(string line, short lineNum)
        {
            Logger288.Log("Parsing END directive", "Parser");
            return new IntermediateLine(line, lineNum);
        }
    }
}
