using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linker
{
    class Modify
    {
        public int Location
        { get; set; }

        public int Word
        { get; set; }

        private List<string> adjustments = new List<string>();

        public List<string> Adjustments
        {
            get { return adjustments; }
        }

        public void AddAdjustments(string sign, string label)
        {
            adjustments.Add(sign);
            adjustments.Add(label);
        }

        public string ProgramName
        { get; set; }
    }
}
