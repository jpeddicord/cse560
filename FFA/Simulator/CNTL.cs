using System;
using System.Collections.Generic;
using System.Text;
using ErrCat = Assembler.Errors.Category;

namespace Simulator
{
    class CNTL
    {
        public static void Run(string function, string bin)
        {
            if (function == "HALT")
            {

            }
            else if (function == "DUMP")
            {

            }
            else if (function == "CLRD")
            {
                CNTL.Clrd();
            }
            else if (function == "CLRT")
            {
                CNTL.Clrt();
            }
            else if (function == "GOTO")
            {

            }
       }

        /**
         * Halts the program with a user specified halt code. This code must be in the range
         * 0 to 1023.
         *
         * @param code Code to displayed to screen as the halt code for the running program.
         *
         * @refcode OP0.0
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 18, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Halt(int code)
        {
            // TODO Throw error if out of range
            Console.WriteLine("Program exited with code: " + code);
            System.Environment.Exit(code);
        }

        /**
         * Dumps memory or stack contents based on flag given. 1 will print the contents of
         * the data and test stack. 2 will print the contents of memory. 3 will print both
         * the contents of the stacks and memory.
         *
         * @param flag Determines what information to dump.
         *
         * @refcode OP0.1
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 18, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Dump(short flag)
        {
            // User should only provide a 1, 2 or 3 as a dump flag
            switch (flag)
            {
                case 1:
                    {
                        dumpStack();
                    } break;
                case 2:
                    {
                        dumpMem();
                    } break;
                case 3:
                    {
                        dumpStack();
                        dumpMem();
                    } break;
                default:
                    {
                        throw new Assembler.ErrorException(ErrCat.Serious, 10);
                    }
            }

        }

        /**
         * Clears the contents of the data stack.
         *
         * @refcode OP0.2
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 18, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Clrd()
        {
            Memory.GetInstance().ClearData();
        }

        /**
         * Clears the contents of the test stack.
         *
         * @refcode OP0.3
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 18, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Clrt()
        {
            Memory.GetInstance().ClearTest();
        }

        /**
         * Sets the location counter to the address provided in the instruction.
         *
         * @param addr The address being jumped to.
         * @param LC Reference to the location counter to be changed to the address.
         *
         * @refcode OP0.4
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 18, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Goto(ref String addr, out int LC)
        {
            // convert memory address to int for comparison
            int address = Convert.ToInt32(addr, 16);

            // ensure that the new address is within range of memory, otherwise throw an exception
            if (1023 >= address || address >= 0)
            {
                LC = address;
            }
            else
            {        
                throw new Assembler.ErrorException(ErrCat.Fatal, 1);
            }
        }

        /**
         * Private procedure used by dump to output the contents of the stacks. This procedure
         * gets the stacks as arrays and outputs the items using spaces as padding for
         * better formatting.
         *
         * @refcode OP0.1
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 18, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void dumpStack()
        {
            // Get both stacks as Arrays
            int[] data = Memory.GetInstance().GetDataStack();
            int[] test = Memory.GetInstance().GetTestStack();

            // Print the "header" for data stack
            Console.Write("Data Stack: ");

            // Only attempt to print the first item in the data stack if it exists
            // Otherwise, pad with spaces to make the test stack line up correctly
            if (data.Length > 0)
            {
                Console.Write(Convert.ToString(data[0], 16).PadLeft(4, '0'));
            }
            else
            {
                Console.Write("    ");
            }

            // Print "header" for test stack
            Console.Write("  Test Stack: ");

            // Only attempt to print the first item in the test stack if it exists
            if (test.Length > 0)
            {
                Console.Write(Convert.ToString(test[0], 16).PadLeft(4, '0'));
            }

            Console.Write('\n');

            // Loop through until the end of both arrays has been reached
            for (int n = 1; n < data.Length || n < test.Length; n++)
            {
                // Print padding to line up data stack output
                Console.Write("            ");

                // If there is another item in data stack print it, otherwise
                // pad with spaces
                if (n < data.Length)
                {
                    Console.Write(Convert.ToString(data[n], 16).PadLeft(4, '0'));
                }
                else
                {
                    Console.Write("    ");
                }

                // If an item exists in the test stack, pad to line up test stack output
                // convert to hex and pad with 0s, then output.
                if (n < test.Length)
                {
                    Console.Write("              ");
                    Console.Write(Convert.ToString(test[n], 16).PadLeft(4, '0'));
                }

                Console.Write('\n');
            }
        }

        /**
         * Private procedure used by dump to output the contents of memory. The memory offset
         * is displayed along the left and starts at 0, incrementing by 16 (decimal) for
         * each row.
         *
         * @refcode OP0.1
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 18, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private static void dumpMem()
        {
            Memory m = Memory.GetInstance();

            // Handles each row of memory
            for (int i = 0; i <= 1023; i = i + 16)
            {
                // Print the memory offset on this row
                Console.Write(Convert.ToString(i, 16).PadLeft(3, '0'));

                // Prints each item in the row
                for (int j = 0; j < 16; j++)
                {
                    // Print the memory at memory offset + position in this row.
                    Console.Write(" " + Convert.ToString(m.GetWord(i + j), 16).PadLeft(4, '0'));
                }

                Console.Write("\n");
            }
        }
    }
}
