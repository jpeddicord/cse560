using System;
namespace Assembler
{
    /**
     * A text record for the object file.
     */
    public class TextRecord
    {
        /**
         * The stored program name.
         */
        private string programName;

        /**
         * Create a text record for the given program.
         *
         * @param programName the program name to use
         * @refcode OB3
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
        public TextRecord(string programName)
        {
            this.programName = programName;
        }

        /**
         * The location of this record in the program. Hex LC.
         */
        private string programLocation;
        public string ProgramLocation {
            get { return this.programLocation; }
            set { this.programLocation = value.PadLeft(4, '0'); }
        }

        /**
         * The hex bytecode for this record.
         */
        private string hexCode;
        public string HexCode {
            get { return this.hexCode; }
            set { this.hexCode = value.PadLeft(4, '0'); }
        }

        /**
         * The A/R/M status flag for this record.
         */
        private char statusFlag;
        public char StatusFlag {
            get { return this.statusFlag; }
            set { this.statusFlag = value; }
        }

        /**
         * If StatusFlag is M, this stores the number of adjustments required. Hex.
         */
        private string adjustments;
        public string Adjustments {
            get { return this.adjustments; }
            set { this.adjustments = value; }
        }

        /**
         * Render this record as a string.
         *
         * @return this record as a string
         * @refcode OB3
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

