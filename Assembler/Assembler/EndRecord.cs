using System;
namespace Assembler
{
    public class EndRecord
    {
        private string programName;

        public EndRecord(string programName)
        {
            this.programName = programName;
        }

        public override string ToString()
        {
            return String.Format("E:{0}", this.programName);
        }
    }
}

