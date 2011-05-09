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
            set { this.programLocation = value; }
        }

        private string word;
        public string Word {
            get { return this.word; }
            set { this.word = value; }
        }

        public void AddAdjustment(bool positive, string label)
        {
            // TODO
        }

        public override string ToString ()
        {
            string adjstr = "";
            foreach (Adjustment adj in this.adjustments)
            {
                adjstr += (adj.positive ? "+" : "-") + ":" + adj.label;
            }
            return String.Format(
                    this.ProgramLocation,
                    this.Word,
                    adjstr,
                    this.programName
            );
        }

    }
}

