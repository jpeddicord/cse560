using System;
using System.Collections.Generic;

namespace Assembler
{
    /**
     * The assembly report for a single session.
     */
    public class AssemblyReport
    {
        /**
         * A line-item in the report.
         */
        struct ReportItem {
            /**
             * Location of this item
             */
            public string lc;

            /**
             * Object code of this line
             */
            public string objCode;

            /**
             * A/R/M flag
             */
            public char flag;

            /**
             * Source line number
             */
            public int line;

            /**
             * Original source line
             */
            public string source;
            
            /**
             * List of errors associated with this item
             */
            public List<Errors.Error> errors;
        };

        /**
         * The list of line-items in the report.
         */
        private List<ReportItem> items = new List<ReportItem>();

        /**
         * Add an item to the assembly report.
         *
         * @param lc location of item
         * @param objCode object code of line
         * @param flag A/R/M flag
         * @param line line number in original source
         * @param source original source line
         * @param errors list of errors encountered while processing this line
         * @refcode AO
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 12, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void Add(string lc, string objCode, char flag, int line, string source, List<Errors.Error> errors = null)
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
         *
         * @return string representation of the report
         * @refcode AO
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 12, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
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
                    foreach (Errors.Error err in item.errors)
                    {
                        str += String.Format(" --- ERROR: {0}\n", err);
                    }
                }
            }
            return str;
        }
    }
}

