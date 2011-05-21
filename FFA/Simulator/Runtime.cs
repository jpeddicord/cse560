using System;

namespace Simulator
{
    /**
     * Main program runtime.
     */
    public class Runtime
    {
        /**
         * The singleton instance of this class
         */
        private static Runtime inst = null;

        /**
         * The location counter
         */
        private int lc = 0;

        /**
         * Get the active Runtime instance.
         *
         * @return active instance
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 18, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static Runtime GetInstance()
        {
            if (Runtime.inst == null)
            {
                Runtime.inst = new Runtime();
            }
            return Runtime.inst;
        }

        /**
         * Location counter public property.
         */
        public int LC
        {
            get { return this.lc; }
            set
            { // TODO: error check
                this.lc = value;
            }
        }

        /**
         * Start running the program at the current LC. If an error is found,
         * print out a runtime error, but continue if possible. Will run the
         * program until it halts or a fatal condition is reached.
         *
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void Run()
        {
            // get the data at the current LC
            var mem = Memory.GetInstance();
            var instr = Assembler.Instructions.GetInstance();
            string category, function;

            while (true)
            {
                // get the binary string of this word
                string bin = Convert.ToString(mem.GetWord(this.lc), 2);
    
                // look up the associated insruction
                instr.ReverseLookup(bin.Substring(0, 5), out category, out function);

                int prevLC = this.lc;

                try
                {
                    ProcessInstruction(category, function, bin);
                }
                catch (Assembler.ErrorException ex)
                {
                    Console.WriteLine(String.Format("RUNTIME ERROR: {0}", ex));
                    // TODO: abort?
                }
#if !DEBUG
                catch (Exception)
                {
                    Console.WriteLine("An internal software error has occurred.");
                }
#endif

                // if the location counter wasn't changed by an instruction,
                // increment it.
                if (this.lc == prevLC)
                {
                    this.lc++;
                }
            }
        }

        /**
         * Process a single instruction.
         *
         * @param category Category name
         * @param function Function name
         * @param bin 16-bit binary string on the instruction line
         * @refcode S1-S4
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
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

