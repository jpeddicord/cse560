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

        public static bool Debug { get; set; }

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
            {
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
                string bin = Convert.ToString(mem.GetWord(this.lc), 2).PadLeft(16, '0');
    
                // look up the associated instruction
                instr.ReverseLookup(bin.Substring(0, 5), out category, out function);

                int prevLC = this.lc;

                try
                {
                    if (Runtime.Debug)
                    {
                        Console.WriteLine();
                        PrintDebug(true);
                    }

                    ProcessInstruction(category, function, bin);

                    if (Runtime.Debug)
                    {
                        PrintDebug(false);
                        Console.WriteLine();
                    }

                    // if the location counter wasn't changed by an instruction,
                    // increment it.
                    if (this.lc == prevLC)
                    {
                        this.lc++;
                    }
                }
                catch (Assembler.ErrorException ex)
                {
                    Console.WriteLine(String.Format("RUNTIME ERROR: {0}", ex));

                    Console.WriteLine();
                    PrintDebug(false);
                    Console.WriteLine();

                    // break on fatal errors
                    if (ex.err.category == Assembler.Errors.Category.Fatal)
                    {
                        break;
                    }
                    // otherwise just go to the next line
                    else
                    {
                        this.lc++;
                    }
                }
#if !DEBUG
                catch (Exception)
                {
                    Console.WriteLine("Unknown runtime error on LC " + this.lc);
                    break;
                }
#endif

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

        public void PrintDebug(Boolean before)
        {
            var mem = Memory.GetInstance();
            var instr = Assembler.Instructions.GetInstance();
            string bin = Convert.ToString(mem.GetWord(this.lc), 2).PadLeft(16, '0');
            string category, function;
            instr.ReverseLookup(bin.Substring(0, 5), out category, out function);

            if (before)
            {
                Console.WriteLine("---Before Simulation--- ---Before Simulation--- ---Before Simulation---");
            }
            else
            {
                Console.WriteLine("===After Simulation=== ===After Simulation=== ===After Simulation===");
            }

            Console.WriteLine(" LC = {0,4}  MEM = {1}  Op-code = {2}  Function = {3}  S = {4}",
                    this.lc,
                    bin,
                    bin.Substring(0, 2),
                    bin.Substring(2, 3),
                    bin.Substring(6));
            Console.WriteLine(" Category: {0,8}  Function: {1}",
                    category, function);
            Console.WriteLine(" S = {0}  M(S) = {1}",
                    Convert.ToInt32(bin.Substring(6), 2),
                    mem.GetWord(Convert.ToInt32(bin.Substring(6), 2)));
            Console.WriteLine(" Top of data stack = {0}",
                    (mem.DataSize() > 0 ? Convert.ToString(mem.GetDataStack()[0]) : "empty"));
            Console.WriteLine(" Top of test stack = {0}",
                    (mem.TestSize() > 0 ? Convert.ToString(mem.GetTestStack()[0]) : "empty"));

            if (before)
            {
                Console.WriteLine("---Before Simulation--- ---Before Simulation--- ---Before Simulation---\n");
            }
            else
            {
                Console.WriteLine("===After Simulation=== ===After Simulation=== ===After Simulation===\n");
            }
        }
    }
}

