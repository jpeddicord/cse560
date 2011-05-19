using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.WriteLine("Program exited with code: " + code);
            System.Environment.Exit(code);
        }

        public static void Dump(short flag)
        {
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
            string[] data = Memory.GetInstance().GetDataStack();
            short[] test = Memory.GetInstance().GetTestStack();

            Console.Write("Data Stack: ");

            if (data.Length > 0)
            {
                Console.Write(data[0]);
            }
            else
            {
                Console.Write("    ");
            }

            Console.Write("  Test Stack: ");

            if (test.Length > 0)
            {
                Console.Write(test[0]);
                Console.Write('\n');
            }

            int n = 1;

            while (n < data.Length || n < test.Length)
            {
                Console.Write("            ");

                if (n < data.Length)
                {
                    Console.Write(data[n]);
                    Console.Write("              ");
                }
                else
                {
                    Console.Write("                  ");
                }

                if (n < test.Length)
                {
                    Console.Write(Convert.ToString(test[n], 16).PadLeft(4, '0'));
                }

                Console.Write('\n');

                n++;
            }
        }

        private static void dumpMem()
        {
            
        }
    }
}
