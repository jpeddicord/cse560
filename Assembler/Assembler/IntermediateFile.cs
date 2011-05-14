using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Assembler
{
    /**
     * Used to hold all of the intermediate lines generated from a source text.
     */
    public class IntermediateFile : System.Collections.IEnumerable
    {
        /**
         * The dictionary that holds all intermediate lines of this source file.
         */
        private Dictionary<int, IntermediateLine> allLines;

        /**
         * Returns the total number of lines in this intermediate file (and the source code).
         */
        public int TotalLines
        {
            get { return allLines.Count; }
        }

        /**
         * The length of this module. (Final LC - Start LC) + 1.
         */
        private string length;

        /**
         * Returns the module length of this file
         */
        public string ModuleLength
        {
            get { return length; }
        }

        /**
         * Calculates the ModuleLength using the start directive's operand and the current
         * LC value according to the Parser.
         * 
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation May 14, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public void CalculateModuleLength()
        {
            int startLoc = Convert.ToInt32(allLines[1].DirectiveOperand, 16);
            int endLoc = Convert.ToInt32(Parser.LC, 16) - 1;
            int len = endLoc - startLoc + 1;
            length = Convert.ToString(len, 16).ToUpper();
        }

        /**
         * Constructs an intermediate file with the given name and no lines.
         * 
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 9, 2011
         * @modlog
         *  - April 10, 2011 - Mark - Add logging
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public IntermediateFile()
        {
            Logger.Log("Creating IntermediateFile object", "IntermediateFile");
            this.allLines = new Dictionary<int, IntermediateLine>();
        }

        /**
         * Adds the indicated IntermediateLine to the file.
         * 
         * @param line the InterediateLine file to add
         *
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public void AddLine(IntermediateLine line)
        {
            this.allLines.Add(line.SourceLineNumber, line);
        }

        /**
         * Returns the specified line in the IntermediateFile.
         * 
         * @param lineNumber the line number of the desired line
         * @return the lineNumberth line in the file
         *
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public IntermediateLine Line(int lineNumber)
        {
            return this.allLines[lineNumber];
        }

        /**
         * Returns the string that represents this IntermediateFile.
         * 
         * @return the file formatted for the screen.  The format for this is as specified in
         *         IntermediateLine for each line in this file seperated by newlines.
         *
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public override string ToString()
        {
            string output = "";

            for (int i = 1; i <= allLines.Count; i++)
            {
                output += allLines[i].ToString() + "-----\n";
            }
            return output;
        }

        /**
         * Support iterating this file line by line.
         */
        public System.Collections.IEnumerator GetEnumerator ()
        {
            for (int i = 1; i <= this.TotalLines; i++)
            {
                yield return this.allLines[i];
            }
        }
    }
}

