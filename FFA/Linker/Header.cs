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
    }
}
