using System;

namespace Simulator
{
    public class Runtime
    {
        private static Runtime inst = null;

        private int lc = 0;

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

        public int LC
        {
            get { return this.lc; }
            set
            { // TODO: error check
                this.lc = value;
            }
        }

        public void Run()
        {
            // get the data at the current LC
            var mem = Memory.GetInstance();
            var instr = Assembler.Instructions.GetInstance();
            string category, function;
            string bin = Convert.ToString(mem.GetWord(this.lc), 2);

            // look up the associated insruction
            instr.ReverseLookup(bin.Substring(0, 5), out category, out function);

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
            if (category == "CNTL")
            {
                CNTL.Run(function, bin);
            }
            else if (category == "STACK")
            {
                STACK.Run(function, bin);
            }
            else if (category == "JUMP")
            {
                JUMP.Run(function, bin);
            }
            else if (category == "SOPER")
            {
                SOPER.Run(function, bin);
            }
            else if (category == "MOPER")
            {
                //MOPER.Run(function, bin);
            }
        }
    }
}

