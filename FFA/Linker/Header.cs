using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linker
{
    /**
     * Linker header record used to hold information about the
     * program being linked.
     */
    class Header
    {
        /**
         * The name of the program.
         */
        public string ProgramName
        { get; set; }

        /**
         *  The load address assigned by the Assembler.
         */
        public int AssemblerLoadAddress
        { get; set; }

        /**
         * The length of the module.
         */
        public int ModuleLength
        { get; set; }

        /**
         * The load address computed by the Linker.
         */
        public int LinkerLoadAddress
        { get; set; }

        /**
         * the execution start address of the module.
         */
        public int ExecutionStartAddress
        { get; set; }

        /**
         * The total number of records in the module.
         */
        public int TotalRecords
        { get; set; }

        /**
         * The total number of linking records in the module.
         */
        public int TotalLinkingRecords
        { get; set; }

        /**
         * The total number of text records in the module.
         */
        public int TotalTextRecords
        { get; set; }

        /**
         * The total number of modify records in the module.
         */
        public int TotalModifyRecords
        { get; set; }

        /**
         * The date and time of the linking in Julian date format.
         * Date in format: yearday,hh,mm,ss
         * year = 4 digit year
         * day = 3 digit day of the year, e.g. Feb 2 is 033
         * hh = 2 digit hour in 24 hour format
         * mm = 2 digit minutes value
         * ss = 2 digit seconds value
         */
        public string LinkingDate
        {
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
         * Returns the header record as a string. Format follows the format given
         * in table LM1.
         * 
         * @return the header record in the specified format
         * 
         * @refcode
         *  LM1
         * @errtest
         * @errmsg
         * @author Mark Mathis
         * @creation May 22, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public override String ToString()
        {
            return String.Format(
                    "H:{0}:{1}:{2}:{3}:{4}:{5}:{6}:FFA-LLM:0288:{0}",
                    this.ProgramName,
                    Convert.ToString(this.LinkerLoadAddress,16).ToUpper().PadLeft(4,'0'),
                    Convert.ToString(this.ExecutionStartAddress,16).ToUpper().PadLeft(4,'0'),
                    Convert.ToString(this.ModuleLength,16).ToUpper().PadLeft(4,'0'),
                    this.LinkingDate,
                    Convert.ToString(this.TotalRecords,16).ToUpper().PadLeft(4,'0'),
                    Convert.ToString(this.TotalTextRecords,16).ToUpper().PadLeft(4,'0')
            );
        }
        }
    }
