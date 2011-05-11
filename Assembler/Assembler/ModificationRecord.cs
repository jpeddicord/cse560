using System;
using System.Collections.Generic;

namespace Assembler
{
    public struct Adjustment {
        public bool positive;
        public string label;
    }

    public class ModificationRecord
    {

        private string programName;

        private List<Adjustment> adjustments = new List<Adjustment>();

        public ModificationRecord(string programName)
        {
            this.programName = programName;
        }

        private string programLocation;
        public string ProgramLocation {
            get { return this.programLocation; }
            set { this.programLocation = value.PadLeft(4, '0'); }
        }

        private string word;
        public string Word {
            get { return this.word; }
            set { this.word = value; }
        }

        /**
         * Add an adjustment to this record.
         */
        public void AddAdjustment(bool positive, string label)
        {
            Adjustment adj;
            adj.positive = positive;
            adj.label = label;
            this.adjustments.Add(adj);
        }

        public override string ToString ()
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

