using System;
namespace Assembler
{
    /**
     * The single header record for an object file.
     */
    public class HeaderRecord
    {
        /**
         * The stored program name.
         */
        private string programName;

        /**
         * Create a header record for the given program.
         *
         * @param programName the program name to use
         * @refcode OB1
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
        public HeaderRecord(string programName)
        {
            this.programName = programName;
        }

        /**
         * The stored assembly loading address. Hex.
         */
        private string loadAddress;
        public string LoadAddress {
            get { return this.loadAddress; }
            set { this.loadAddress = value.PadLeft(4, '0').ToUpper(); }
        }

        /**
         * The stored length of this module. Hex.
         */
        private string moduleLength;
        public string ModuleLength {
            get { return this.moduleLength; }
            set { this.moduleLength = value.PadLeft(4, '0').ToUpper(); }
        }

        /**
         * The start of execution for this module. Hex.
         */
        private string executionStart;
        public string ExecutionStart {
            get { return this.executionStart; }
            set { this.executionStart = value.PadLeft(4, '0').ToUpper(); }
        }

        /**
         * The date of assembly.
         */
        public string AssemblyDate {
            get
            {
                DateTime assTime = DateTime.Now;
                return String.Format("{0:D4}{1:D3},{2:D2},{3:D2},{4:D2}",
                                     assTime.Year,
                                     assTime.DayOfYear,
                                     assTime.Hour,
                                     assTime.Minute,
                                     assTime.Second);
            }
        }

        /**
         * The encoded version number of the assembler.
         */
        public string VersionInfo {
            get { return "9001"; }
        }

        /**
         * The total number of records in this module.
         */
        private string totalRecords;
        public string TotalRecords {
            get { return this.totalRecords; }
            set { this.totalRecords = value.PadLeft(4, '0').ToUpper(); }
        }

        /**
         * The total number of linking records in this module.
         */
        private string totalLinking;
        public string TotalLinking {
            get { return this.totalLinking; }
            set { this.totalLinking = value.PadLeft(4, '0').ToUpper(); }
        }


        /**
         * The total number of text records in this module.
         */
        private string totalText;
        public string TotalText {
            get { return this.totalText; }
            set { this.totalText = value.PadLeft(4, '0').ToUpper(); }
        }

        /**
         * The total number of modification records in this module.
         */
        private string totalModification;
        public string TotalModification {
            get { return this.totalModification; }
            set { this.totalModification = value.PadLeft(4, '0').ToUpper(); }
        }

        /**
         * Return this header record as a string.
         *
         * @return this record as a string
         * @refcode OB1
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

