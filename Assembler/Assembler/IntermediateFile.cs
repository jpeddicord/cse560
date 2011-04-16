using System.Collections.Generic;
using System.Diagnostics;

namespace Assembler
{

    class IntermediateFile
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
         * Constructs an intermediate file with the given name and no lines.
         * 
         * @param prgmName the name of the program that will be represented by
         *  this IntermediateFile
         * @refcode
         * @errtest
         * @errmsg
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         *  - April 10, 2011 - Mark - Add logging
         * @codestandard Mark
         * @teststandard Andrew
         */
        public IntermediateFile()
        {
            Logger288.Log("Creating IntermediateFile object", "IntermediateFile");
            this.allLines = new Dictionary<int, IntermediateLine>();
        }

        /**
         * Adds the indicated IntermediateLine to the file.
         * 
         * @param line the InterediateLine file to add
         * @refcode
         * @errtest
         * @errmsg
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark
         * @teststandard Andrew
         */
        public void AddLine(IntermediateLine line)
        {
            this.allLines.Add(int.Parse(line.SourceLineNumber), line);
        }

        /**
         * Returns the specified line in the IntermediateFile.
         * 
         * @param lineNumber the line number of the desired line
         * @return the lineNumberth line in the file
         * @refcode
         * @errtest
         * @errmsg
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark
         * @teststandard Andrew
         */
        public IntermediateLine Line(int lineNumber)
        {
            return allLines[lineNumber];
        }

        /**
         * Returns the string that represents this IntermediateFile.
         * 
         * @return the file formatted for the screen.  The format for this is as specified in
         *         IntermediateLine for each line in this file seperated by newlines.
         * @refcode
         * @errtest
         * @errmsg
         * @author Mark
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark
         * @teststandard Andrew
         */
        public override string ToString()
        {
            string output = "";

            for (int i = 1; i <= allLines.Count; i++)
            {
                output += allLines[i].ToString();
            }
            return output;
        }
    }
}

