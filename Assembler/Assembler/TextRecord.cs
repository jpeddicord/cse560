using System;
namespace Assembler
{
    public class TextRecord
    {

        private string programName;

        public TextRecord(string programName)
        {
            this.programName = programName;
        }

        private string programLocation;
        public string ProgramLocation {
            get { return this.programLocation; }
            set { this.programLocation = value.PadLeft(4, '0'); }
        }

        private string hexCode;
        public string HexCode {
            get { return this.hexCode; }
            set { this.hexCode = value.PadLeft(4, '0'); }
        }

        private string statusFlag;
        public string StatusFlag {
            get { return this.statusFlag; }
            set { this.statusFlag = value; }
        }

        private string adjustments;
        public string Adjustments {
            get { return this.adjustments; }
            set { this.adjustments = value; }
        }

        public override string ToString()
        {
            return String.Format("T:{0}:{1}:{2}:{3}:{4}",
                    this.ProgramLocation,
                    this.HexCode,
                    this.StatusFlag,
                    this.Adjustments,
                    this.programName
            );
        }
    }
}

