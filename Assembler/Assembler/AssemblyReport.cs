using System;

namespace Assembler
{
    public class AssemblyReport
    {
        public AssemblyReport()
        {

        }

        /**
         * Add an item to the assembly report.
         */
        public void Add(string lc, string objCode, char flag, int line, string source)
        {
            // TODO
        }

        public override string ToString()
        {
            return String.Format("[AssemblyReport]"); // TODO
        }
    }
}

