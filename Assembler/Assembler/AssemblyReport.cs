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
            public List<string> errors;
        };

        private List<ReportItem> items = new List<ReportItem>();

        /**
         * Add an item to the assembly report.
         */
        public void Add(string lc, string objCode, char flag, int line, string source, List<string> errors = null)
        {
            ReportItem item;
            item.lc = lc;
            item.objCode = objCode;
            item.flag = flag;
            item.line = line;
            item.source = source;
            item.errors = errors;
            this.items.Add(item);
        }

        /**
         * Return the assembly report as a string.
         */
        public override string ToString()
        {
            string str = "LOC   OBJCODE   FLAG   LINE   SOURCE\n";
            foreach (ReportItem item in this.items)
            {
                str += String.Format("{0,-6}{1,-10}{2,-7}{3,-7}{4}\n",
                        item.lc,
                        item.objCode,
                        item.flag,
                        item.line,
                        item.source
                );
                if (item.errors != null)
                {
                    foreach (string err in item.errors)
                    {
                        str += String.Format(" --- ERROR: {0}\n", err);
                    }
                }
            }
            return str;
        }
    }
}

