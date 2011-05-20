using System;
using System.Collections.Generic;

namespace Assembler
{
    /**
     * An adjustment to be used in this record.
     */
    public struct Adjustment {
        /**
         * true if positive, false if negative.
         */
        public bool positive;

        /**
         * The label to associate with this adjustment.
         */
        public string label;
    }

    /**
     * A modification record. Specifies to the linker that a line should be modified.
     */
    public class ModificationRecord
    {
        /**
         * The stored program name.
         */
        private string programName;

        /**
         * The list of adjustments in this record.
         */
        private List<Adjustment> adjustments = new List<Adjustment>();

        /**
         * Create a modification record for the given program.
         *
         * @param programName the program name to use
         * @refcode OB4
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 10, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public ModificationRecord(string programName)
        {
            this.programName = programName;
        }

        /**
         * The location in the program that the modification(s) need to be performed.
         */
        private string programLocation;
        public string ProgramLocation {
            get { return this.programLocation; }
            set { this.programLocation = value.PadLeft(4, '0').ToUpper(); }
        }

        /**
         * The original instruction word without modifications.
         */
        private string word;
        public string Word {
            get { return this.word; }
            set { this.word = value.PadLeft(4, '0').ToUpper(); }
        }

        /**
         * Get the number of adjustments present.
         */
        public int Adjustments {
            get { return this.adjustments.Count; }
        }

        /**
         * Add an adjustment to this record.
         *
         * @param positive Whether the adjustment should be positive. Otherwise negative.
         * @param label The label to add as an adjustment.
         * @refcode OB4
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 10, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void AddAdjustment(bool positive, string label)
        {
            Adjustment adj;
            adj.positive = positive;
            adj.label = label;
            this.adjustments.Add(adj);
        }

        /**
         * Return this modification record as a string.
         *
         * @return this record as a string
         * @refcode OB4
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 10, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public override string ToString()
        {
            string adjstr = "";
            foreach (Adjustment adj in this.adjustments)
            {
                adjstr += (adj.positive ? "+" : "-") + ":" + adj.label + ":";
            }
            return String.Format("M:{0}:{1}:{2}{3}",
                    this.ProgramLocation,
                    this.Word,
                    adjstr,
                    this.programName
            );
        }

    }
}

