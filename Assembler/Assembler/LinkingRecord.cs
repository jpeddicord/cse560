using System;
namespace Assembler
{
    /**
     * A linking record for an object file. Specifies where an entry symbol is located.
     */
    public class LinkingRecord
    {
        /**
         * The stored program name.
         */
        private string programName;

        /**
         * Create a linking record for the given program.
         *
         * @param programName the program name to use
         * @refcode OB2
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
        public LinkingRecord(string programName)
        {
            this.programName = programName;
        }

        /**
         * The name of the entry to be given to the linker.
         */
        private string entryName;
        public string EntryName {
            get { return this.entryName; }
            set { this.entryName = value; }
        }

        /**
         * The location of the symbol in this object file. Hex.
         */
        private string programLocation;
        public string ProgramLocation {
            get { return this.programLocation; }
            set { this.programLocation = value.PadLeft(4, '0'); }
        }

        /**
         * Return this linking record as a string.
         *
         * @return this record as a string
         * @refcode OB2
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
            return String.Format("L:{0}:{1}:{2}",
                    this.EntryName,
                    this.ProgramLocation,
                    this.programName
            );
        }
    }
}

