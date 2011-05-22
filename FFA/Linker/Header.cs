using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linker
{
    class Header
    {
        public string ProgramName
        { get; set; }

        public int AssemblerLoadAddress
        { get; set; }

        public int ModuleLength
        { get; set; }

        public int LinkerLoadAddress
        { get; set; }

        public int ExecutionStartAddress
        { get; set; }

        public int TotalRecords
        { get; set; }

        public int TotalLinkingRecords
        { get; set; }

        public int TotalTextRecords
        { get; set; }

        public int TotalModifyRecords
        { get; set; }

        public string LinkingDate
        {
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
