using System;
namespace Assembler
{
    public class HeaderRecord
    {

        private string programName;

        public HeaderRecord(string programName)
        {
            this.programName = programName;
        }

        private string loadAddress;
        public string LoadAddress {
            get { return this.loadAddress; }
            set { this.loadAddress = value.PadLeft(4, '0'); }
        }

        private string moduleLength;
        public string ModuleLength {
            get { return this.moduleLength; }
            set { this.moduleLength = value.PadLeft(4, '0'); }
        }

        private string executionStart;
        public string ExecutionStart {
            get { return this.executionStart; }
            set { this.executionStart = value.PadLeft(4, '0'); }
        }

        public string AssemblyDate {
            get
            {
                DateTime assTime = DateTime.Now;
                return String.Format("{0:D4}{1:D3},{2:D2}:{3:D2}:{4:D2}",
                                     assTime.Year,
                                     assTime.DayOfYear,
                                     assTime.Hour,
                                     assTime.Minute,
                                     assTime.Second);
            }
        }

        public string VersionInfo {
            get { return "9001"; }
        }

        private string totalRecords;
        public string TotalRecords {
            get { return this.totalRecords; }
            set { this.totalRecords = value.PadLeft(4, '0'); }
        }

        private string totalLinking;
        public string TotalLinking {
            get { return this.totalLinking; }
            set { this.totalLinking = value.PadLeft(4, '0'); }
        }


        private string totalText;
        public string TotalText {
            get { return this.totalText; }
            set { this.totalText = value.PadLeft(4, '0'); }
        }

        private string totalModification;
        public string TotalModification {
            get { return this.totalModification; }
            set { this.totalModification = value.PadLeft(4, '0'); }
        }

        /**
         * Return this header record as a string.
         */
        public override String ToString()
        {
            return String.Format(
                    "H:{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:FFA-ASM:{0}",
                     this.programName,
                     this.LoadAddress,
                     this.ModuleLength,
                     this.ExecutionStart,
                     this.AssemblyDate,
                     this.VersionInfo,
                     this.TotalRecords,
                     this.TotalLinking,
                     this.TotalText,
                     this.TotalModification
            );
        }
    }
}

