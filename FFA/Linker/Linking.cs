using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linker
{
    /**
     * Linking record holding information parsed from the assembler
     * generated object file for use in the Linker.
     */
    class Linking
    {
        /**
         * The name of the entry specified by the linking record.
         */
        public string EntryName
        { get; set; }

        /**
         * The location of the entry specified by the linking record.
         */
        public int Location
        { get; set; }

        /**
         * The program that contains the entry specified by the linking record.
         */
        public string ProgramName
        { get; set; }
    }
}
