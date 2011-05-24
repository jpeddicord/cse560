using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErrCat = Assembler.Errors.Category; 

namespace Simulator
{
    class MOPER
    {
        /**
         * Executes the MOPER instruction. Breaks the rest of the binary string apart and calls
         * the desired procedure.
         *
         * @param function The MOPER procedure to be called
         * @param bin The binary representation of the instruction.
         *
         * @refcode
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 22, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Run(string function, string bin)
        {
            bool character = (bin[5] == '1');
            int addr = Convert.ToInt32(bin.Substring(6), 2);

            if (function == "ADD")
            {
                MOPER.Add(addr);
            }
            else if (function == "SUB")
            {
                MOPER.Sub(addr);
            }
            else if (function == "MUL")
            {
                MOPER.Mul(addr);
            }
            else if (function == "DIV")
            {
                MOPER.Div(addr);
            }
            else if (function == "OR")
            {
                MOPER.Or(addr);
            }
            else if (function == "AND")
            {
                MOPER.And(addr);
            }
            else if (function == "READN")
            {
                if (character)
                {
                    MOPER.Readc(addr);
                }
                else
                {
                    MOPER.Readn(addr);
                }
            }
            else if (function == "READC")
            {
                if (character)
                {
                    MOPER.Readc(addr);
                }
                else
                {
                    MOPER.Readn(addr);
                }
            }
            else if (function == "WRITEN")
            {
                if (character)
                {
                    MOPER.Writec(addr);
                }
                else
                {
                    MOPER.Writen(addr);
                }
            }
            else if (function == "WRITEC")
            {
                if (character)
                {
                    MOPER.Writec(addr);
                }
                else
                {
                    MOPER.Writen(addr);
                }
            }
        }

        /**
         * Pops an item off of the data stack and adds it to the value stored at the given
         * memory address. The result is then pushed back to the stack.
         *
         * @param addr The location of the item to be added to the popped value.
         *
         * @refcode OP3.0
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 21, 2011
         * @modlog
         *  - May 23, 2011 - Andrew - Added The ability to recover items if there is overflow.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Add(int addr)
        {
            Memory m = Memory.GetInstance();

            int i = m.DataPopInt();
            int j = m.GetWordInt(addr);

            int val = i + j;

            try
            {
                m.DataPushInt(val);
            }
            catch (Exception)
            {
                // Put the popped value back if there is overflow.
                m.DataPushInt(i);

                throw new Assembler.ErrorException(ErrCat.Serious, 11);
            }
        }


        /**
         * Pops an item off of the data stack and subtracts the value stored at the given
         * memory address. The result is then pushed back to the stack.
         *
         * @param addr The location of the item to be subtracted from the popped value.
         *
         * @refcode OP3.1
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 21, 2011
         * @modlog
         *  - May 23, 2011 - Andrew - Added The ability to recover items if there is overflow.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Sub(int addr)
        {
            Memory m = Memory.GetInstance();

            int i = m.DataPopInt();
            int j = m.GetWordInt(addr);

            int val = i - j;

            try
            {
                m.DataPushInt(val);
            }
            catch (Exception)
            {
                // Put the popped value back if there is overflow.
                m.DataPushInt(i);

                throw new Assembler.ErrorException(ErrCat.Serious, 11);
            }
        }

        /**
         * Pops an item off of the data stack and multiplies it by the value stored at the given
         * memory address. The result is then pushed back to the stack.
         *
         * @param addr The location of the item to be multiplied the popped value.
         *
         * @refcode OP3.2
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 21, 2011
         * @modlog
         *  - May 23, 2011 - Andrew - Added The ability to recover items if there is overflow.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Mul(int addr)
        {
            Memory m = Memory.GetInstance();

            int i = m.DataPopInt();
            int j = m.GetWordInt(addr);

            int val = i * j;

            try
            {
                m.DataPushInt(val);
            }
            catch (Exception)
            {
                // Put the popped value back if there is overflow.
                m.DataPushInt(i);

                throw new Assembler.ErrorException(ErrCat.Serious, 11);
            }
        }

        /**
         * Pops an item off of the data stack and divides it by the value stored at the given
         * memory address. The result is then pushed back to the stack.
         *
         * @param addr The location of the item to divide the popped value.
         *
         * @refcode OP3.3
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 21, 2011
         * @modlog
         *  - May 23, 2011 - Andrew - Added The ability to recover items if there is overflow.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Div(int addr)
        {
            Memory m = Memory.GetInstance();

            int i = m.DataPopInt();
            int j = m.GetWordInt(addr);

            if (j == 0)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 12);
            }

            int val = i / j;

            try
            {
                m.DataPushInt(val);
            }
            catch (Exception)
            {
                // Put the popped value back if there is overflow.
                m.DataPushInt(i);

                throw new Assembler.ErrorException(ErrCat.Serious, 11);
            }
        }

        /**
         * Pops an item off of the data stack and arithmetic ors it with the value stored at the given
         * memory address. The result is then pushed back to the stack.
         *
         * @param addr The location of the item to be ored with the popped value.
         *
         * @refcode OP3.4
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 21, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Or(int addr)
        {
            Memory m = Memory.GetInstance();

            int i = m.DataPopInt();
            int j = m.GetWordInt(addr);

            int val = i | j;

            m.DataPushInt(val);
        }

        /**
         * Pops an item off of the data stack and arithmetic ands it with the value stored at the given
         * memory address. The result is then pushed back to the stack.
         *
         * @param addr The location of the item to be anded with the popped value.
         *
         * @refcode OP3.5
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 21, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void And(int addr)
        {
            Memory m = Memory.GetInstance();

            int i = m.DataPopInt();
            int j = m.GetWordInt(addr);

            int val = i & j;

            m.DataPushInt(val);
        }

        /**
         * Reads one number from a file or he console and stores it in memory.
         *
         * @param addr The location to store the number.
         *
         * @refcode OP3.6
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 21, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Readn(int addr)
        {
            string str = Console.ReadLine();
            int i = 0;
            
            // Ensure that the user entered some value
            if (str.Length == 0)
            {
               throw new Assembler.ErrorException(ErrCat.Serious, 23);
            }

            try
            {
                i = Convert.ToInt32(str);
            } // Make sure the value entered can be converted to a number
            catch (FormatException)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 25);
            } // and that it isn't larger than an int
            catch (OverflowException)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 24);
            }

            // Check that the value can fit in memory
            if (-32768 > i || i > 32767)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 24);
            }

            Memory.GetInstance().SetWordInt(addr, i);
        }

        /**
         * Reads one or two characters from a file or he console and stores it in memory.
         *
         * @param addr The location to store the character(s).
         *
         * @refcode OP3.6
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 21, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Readc(int addr)
        {
            string str = Console.ReadLine();
            int i = 0;

            // Ensures that something is entered
            if (str.Length == 0)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 23);
            } // Gives a warning if there are more than two characters.
            else if (str.Length > 2)
            {
                Console.WriteLine(String.Format("RUNTIME ERROR ON LC {1}: {0}\n", new Assembler.ErrorException(ErrCat.Warning, 1), Runtime.GetInstance().LC));
            }
            
            i = (int)str[0];

            // shifts the first character over by 8 bits so it is left aligned
            i *= 256;

            // adds on the second character if there is one
            if (str.Length == 2)
            {
                i += (int)str[1];
            }

            Memory.GetInstance().SetWordInt(addr, i);
        }

        /**
         * Reads one number from memory and displays it on the screen.
         *
         * @param addr The location in memory of the number to display.
         *
         * @refcode OP3.7
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 21, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Writen(int addr)
        {
            Memory m = Memory.GetInstance();
            int i = m.GetWordInt(addr);

            Console.Write(i);
            Console.Out.Flush();

        }

        /**
         * Reads one or two characters from memory and displays them on the screen.
         *
         * @param addr The location in memory of the character(s) to display.
         *
         * @refcode OP3.7
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 21, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Writec(int addr)
        {
            Memory m = Memory.GetInstance();

            int i = m.GetWord(addr);

            // get the last 8 bits (the second character)
            int char2 = i % 256;

            // shift it over for the other 8 bits (the first character)
            int char1 = i / 256;

            // Convert from integers to characters
            char c1 = (char)char1;
            char c2 = (char)char2;

            Console.Write(c1.ToString() + c2.ToString());
            Console.Out.Flush();

        }
    }
}
