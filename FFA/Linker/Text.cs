using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linker
{
    /**
     * Holds text records for the linker.
     */
    class Text
    {
        /**
         * The location of the text record in the program.
         */
        public int Location
        { get; set; }

        /**
         * The memory word of the text record in the program.
         */
        public int Word
        { get; set; }

        /**
         * The relocation flag for this text record. Can be one of three things:
         * A=absolute, no relocations
         * R=relocatable, relocate by program name
         * M=multiple relocatable, relocate by program name and one or more externals
         */
        public char Flag
        { get; set; }

        /**
         * The number of multiple adjustments. Should be 0 if relocation flag is
         * A or R. Can be 1-15 base-10 if relocation flag is M.
         */
        public int Adjustments
        { get; set; }

        /**
         * The program that contains this text record.
         */
        public string ProgramName
        { get; set; }

        /**
         * Returns the text record as a string. Format follows the format given
         * in table LM2.
         * 
         * @return the text record in the specified format
         * 
         * @refcode
         *  LM2
         * @errtest
         * @errmsg
         * @author Mark Mathis
         * @creation May 22, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public override string ToString()
        {
            return string.Format("T:{0}:{1}:{2}",
                                 Convert.ToString(this.Location,16).ToUpper().PadLeft(4,'0'),
                                 Convert.ToString(this.Word,16).ToUpper().PadLeft(4,'0'),
                                 this.ProgramName
                                 );
        }
    }
}
