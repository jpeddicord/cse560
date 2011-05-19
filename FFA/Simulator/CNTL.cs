using System;
using System.Collections.Generic;
using System.Text;
using ErrCat = Assembler.Errors.Category;

namespace Simulator
{
    class CNTL
    {
        public CNTL()
        {
            // Possibly logging?
        }

        public static void Halt(int code)
        {
            // TODO Throw error if out of range
            Console.WriteLine("Program exited with code: " + code);
            System.Environment.Exit(code);
        }

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

        public static void Clrd()
        {
            Memory.GetInstance().ClearData();
        }

        public static void Clrt()
        {
            Memory.GetInstance().ClearTest();
        }

        public static void Goto(ref String addr, out String LC)
        {
            // convert memory address to int for comparison
            int address = Convert.ToInt32(addr, 16);

            // ensure that the new address is within range of memory, otherwise throw an exception
            if (1023 >= address || address >= 0)
            {
                LC = addr;
            }
            else
            {        
                throw new Assembler.ErrorException(ErrCat.Fatal, 1);
            }
        }

        private static void dumpStack()
        {
            // Get both stacks as Arrays
            string[] data = Memory.GetInstance().GetDataStack();
            short[] test = Memory.GetInstance().GetTestStack();

            // Print the "header" for data stack
            Console.Write("Data Stack: ");

            // Only attempt to print the first item in the data stack if it exists
            // Otherwise, pad with spaces to make the test stack line up correctly
            if (data.Length > 0)
            {
                Console.Write(data[0]);
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
                Console.Write(test[0]);
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
                    Console.Write(data[n]);
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

        private static void dumpMem()
        {
            // Handles each row of memory
            for (int i = 0; i <= 1023; i = i + 16)
            {
                // Print the memory offset on this row
                Console.Write(Convert.ToString(i, 16).PadLeft(3, '0'));

                // Prints each item in the row
                for (int j = 0; j < 16; j++)
                {
                    // Print the memory at memory offset + position in this row.
                    Console.Write(" " + Memory.GetInstance().GetWord(i + j));
                }

                Console.Write("\n");
            }
        }
    }
}
