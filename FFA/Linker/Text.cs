using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linker
{
    class Text
    {
        public int Location
        { get; set; }

        public int Word
        { get; set; }

        public char Flag
        { get; set; }

        public int Adjustments
        { get; set; }

        public string ProgramName
        { get; set; }

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
