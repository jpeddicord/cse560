using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linker
{
    class Modify
    {
        /**
         * The location of the memory word to be modified
         * by the modify record.
         */
        public int Location
        { get; set; }

        /**
         * The memory word to be modified by the modify record.
         */
        public int Word
        { get; set; }

        /**
         * The adjustments specified by the modify record.
         */
        private List<string> adjustments = new List<string>();

        /**
         * The adjustments specified by the modify record.
         */
        public List<string> Adjustments
        { get { return adjustments; } }

        /**
         * Adds an adjustment to the modify record.
         * 
         * @param sign the sign of the adjustment, either '+' or '-'
         * @param label the label by which the word should be modified
         * 
         * @refcode
         *  OB4
         * @errtest
         * @errmsg
         * @author Mark Mathis
         * @creation May 21, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void AddAdjustments(string sign, string label)
        {
            adjustments.Add(sign);
            adjustments.Add(label);
        }

        /**
         * The program that contains this modify record.
         */
        public string ProgramName
        { get; set; }
    }
}
