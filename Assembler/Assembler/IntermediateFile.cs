using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assembler
{

    class IntermediateFile
    {
        // The dictionary that holds all intermediate lines of this source file.
        private Dictionary<int, IntermediateLine> allLines;

        private string prgmName;

        public IntermediateFile(string prgmName)
        {
            this.allLines = new Dictionary<int, IntermediateLine>();
            this.prgmName = prgmName;
        }

        public void AddLine(IntermediateLine line)
        {
            this.allLines.Add(int.Parse(line.SourceLineNumber), line);
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

