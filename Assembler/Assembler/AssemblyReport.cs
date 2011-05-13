using System;
using System.Collections.Generic;

namespace Assembler
{
    public class AssemblyReport
    {
        struct ReportItem {
            public string lc;
            public string objCode;
            public char flag;
            public int line;
            public string source;
        };

        private List<ReportItem> items = new List<ReportItem>();

        /**
         * Add an item to the assembly report.
         */
        public void Add(string lc, string objCode, char flag, int line, string source)
        {
            ReportItem item;
            item.lc = lc;
            item.objCode = objCode;
            item.flag = flag;
            item.line = line;
            item.source = source;
            this.items.Add(item);
        }

        /**
         * Return the assembly report as a string.
         */
        public override string ToString()
        {
            string str = "LOC   OBJCODE   FLAG   LINE   SOURCE";
            foreach (ReportItem item in this.items)
            {
                str += String.Format("{0,-6}{1,-10}{2,-7}{3,-7}{4}\n",
                        item.lc,
                        item.objCode,
                        item.flag,
                        item.line,
                        item.source
                );
            }
            return str;
        }
    }
}

