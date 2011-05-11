using System;
namespace Assembler
{
    public class LinkingRecord
    {

        private string programName;

        public LinkingRecord(string programName)
        {
            this.programName = programName;
        }

        private string entryName;
        public string EntryName {
            get { return this.entryName; }
            set { this.entryName = value; }
        }

        private string programLocation;
        public string ProgramLocation {
            get { return this.programLocation; }
            set { this.programLocation = value.PadLeft(4, '0'); }
        }

        public override string ToString()
        {
            return String.Format("L:{0}:{1}:{2}",
                    this.EntryName,
                    this.ProgramLocation,
                    this.programName
            );
        }
    }
}

