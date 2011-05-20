using System;

namespace Simulator
{
    public class Runtime
    {
        private static Runtime inst = null;

        private string lc = "0000";

        private Runtime()
        {
        }

        public static Runtime GetInstance()
        {
            if (Runtime.inst == null)
            {
                Runtime.inst = new Runtime();
            }
            return Runtime.inst;
        }

        public string LC
        {
            get { return this.lc; }
            set
            { // TODO: error check
                this.lc = value.PadLeft(4, '0');
            }
        }

        public void IncrementLC()
        {
            int lc = Convert.ToInt32(this.lc, 16);
            this.lc = Convert.ToString(this.lc + 1, 16);
        }

        public void Run()
        {
            // get the data at the current LC
            var mem = Memory.GetInstance();
            var instr = Assembler.Instructions.GetInstance();
            string category, function;
            string bin = Convert.ToString(
                    Convert.ToInt32(mem.GetWord(this.lc), 16), 2);

            // look up the associated insruction
            instr.ReverseLookup(bin.Substring(0, 5), category, function);

            try
            {
                ProcessInstruction(category, function, bin);
            }
            catch (Assembler.ErrorException ex)
            {
                Console.WriteLine(String.Format("RUNTIME ERROR: {0}", ex));
                // TODO: abort?
            }
        }

        public void ProcessInstruction(string category, string function, string bin)
        {

        }
    }
}

