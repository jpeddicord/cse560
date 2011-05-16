using System;
namespace Assembler
{
    /**
     * The single end record for an object file.
     */
    public class EndRecord
    {
        /**
         * The stored program name.
         */
        private string programName;

        /**
         * Create an end record for the given program.
         *
         * @param programName the program name to use
         * @refcode OB5
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
        public EndRecord(string programName)
        {
            this.programName = programName;
        }

        /**
         * Return this ending record for the object file.
         *
         * @return this record as a string
         * @refcode OB5
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
            return String.Format("E:{0}", this.programName);
        }
    }
}

