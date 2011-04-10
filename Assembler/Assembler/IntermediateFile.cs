using System.Collections.Generic;
using System.Diagnostics;

namespace Assembler
{

    class IntermediateFile
    {
        // The dictionary that holds all intermediate lines of this source file.
        private Dictionary<int, IntermediateLine> allLines;

        public int TotalLines
        {
            get { return allLines.Count; }
        }

        private string prgmName;

        public IntermediateFile(string prgmName)
        {
            Trace.WriteLine("Creating IntermediateFile object for " + prgmName, "IntermediateFile");
            this.allLines = new Dictionary<int, IntermediateLine>();
            this.prgmName = prgmName;
        }

        public void AddLine(IntermediateLine line)
        {
            this.allLines.Add(int.Parse(line.SourceLineNumber), line);
        }

        public IntermediateLine Line(int lineNumber)
        {
            return allLines[lineNumber];
        }

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

