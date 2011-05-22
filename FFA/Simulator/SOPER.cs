using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErrCat = Assembler.Errors.Category; 

namespace Simulator
{
    class SOPER
    {
        /**
         * Executes the SOPER instruction. Breaks the rest of the binary string apart and calls
         * the desired procedure.
         *
         * @param function The SOPER procedure to be called
         * @param bin The binary representation of the instruction.
         *
         * @refcode OP2
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
            // determine if it would be a number or character read/write
            bool character = (bin[5] == '0');

            // Use the last 8 bits for the number
            int num = Convert.ToInt32(bin.Substring(8), 2);

            if (function == "ADD")
            {
                SOPER.Add(num);
            }
            else if (function == "SUB")
            {
                SOPER.Sub(num);
            }
            else if (function == "MUL")
            {
                SOPER.Mul(num);
            }
            else if (function == "DIV")
            {
                SOPER.Div(num);
            }
            else if (function == "OR")
            {
                SOPER.Or(num);
            }
            else if (function == "AND")
            {
                SOPER.And(num);
            }
            else if (function == "READN")
            {
                if (character)
                {
                    SOPER.Readc(num);
                }
                else
                {
                    SOPER.Readn(num);
                }
            }
            else if (function == "READC")
            {
                if (character)
                {
                    SOPER.Readc(num);
                }
                else
                {
                    SOPER.Readn(num);
                }
            }
            else if (function == "WRITEN")
            {
                if (character)
                {
                    SOPER.Writec(num);
                }
                else
                {
                    SOPER.Writen(num);
                }
            }
            else if (function == "WRITEC")
            {
                if (character)
                {
                    SOPER.Writec(num);
                }
                else
                {
                    SOPER.Writen(num);
                }
            }
        }

        /**
         * Pops n items off of the stack and adds them then pushes the result.
         *
         * @param n The number of items to add.
         *
         * @refcode OP2.0
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Add(int n)
        {
            if (n > 1)
            {
                    Memory m = Memory.GetInstance();
                    int total = 0;
                    while (n > 0)
                    {
                        total = total + m.DataPopInt();
                    }

                    m.DataPushInt(total);
            }
        }

        /**
         * Pops n items off of the stack and subtracts them then pushes the result.
         *
         * @param n The number of items to subtract.
         *
         * @refcode OP2.1
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Sub(int n)
        {
            if (n > 1)
            {

                    Memory m = Memory.GetInstance();
                    int total = m.DataPopInt();

                    while (n > 1)
                    {
                        total = total - m.DataPopInt();
                    }

                    m.DataPushInt(total);

            }
        }

        /**
         * Pops n items off of the stack and multiplies them then pushes the result.
         *
         * @param n The number of items to multiply.
         *
         * @refcode OP2.2
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Mul(int n)
        {
            if (n > 1)
            {

                Memory m = Memory.GetInstance();
                int total = 1;
                while (n > 0)
                {
                    total = total * m.DataPopInt();
                }

                m.DataPushInt(total);

            }
        }

        /**
         * Pops n items off of the stack and divides them then pushes the result.
         *
         * @param n The number of items to divide.
         *
         * @refcode OP2.3
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Div(int n)
        {
            if (n > 1)
            {

                    Memory m = Memory.GetInstance();
                    int total = m.DataPopInt();

                    while (n > 1)
                    {
                        int i = m.DataPopInt();

                        // Check to ensure we never divide by zero, throw an error if we do
                        if (i == 0)
                        {
                            throw new Assembler.ErrorException(ErrCat.Serious, 12);
                        }

                        total = total / i;
                    }

                    m.DataPushInt(total);

            }
        }

        /**
         * Pops n items off of the stack and arithmetic ors them then pushes the result.
         *
         * @param n The number of items to or.
         *
         * @refcode OP2.4
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Or(int n)
        {
            if (n > 1)
            {

                    Memory m = Memory.GetInstance();
                    int total = m.DataPopInt();

                    while (n > 1)
                    {
                        total = total | m.DataPopInt();
                    }

                    m.DataPushInt(total);
            }
        }

        /**
         * Pops n items off of the stack and arithmetic ands them then pushes the result.
         *
         * @param n The number of items to and.
         *
         * @refcode OP2.5
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void And(int n)
        {
            if (n > 1)
            {
                    Memory m = Memory.GetInstance();
                    int total = m.DataPopInt();

                    while (n > 1)
                    {
                        total = total & m.DataPopInt();
                    }

                    m.DataPushInt(total);
            }
        }

        /**
         * Reads n number of integers (decimal) from a file or the console and pushes them
         * on the data stack.
         *
         * @param n The number of items to read from the file.
         *
         * @refcode OP2.6
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Readn(int n)
        {
            while (n > 0)
            {
                Console.Write("Enter an integer: ");
                string str = Console.ReadLine();
                int i = 0;

                // Ensure something is read in
                if (str.Length == 0)
                {
                    throw new Assembler.ErrorException(ErrCat.Serious, 23);
                }

                try
                {
                    i = Convert.ToInt32(str);
                } // Catch if the user gives something that isn't a number
                catch (FormatException)
                {
                    throw new Assembler.ErrorException(ErrCat.Serious, 25);
                } // Catch if the given value is too large to fit in an integer
                catch (OverflowException)
                {
                    throw new Assembler.ErrorException(ErrCat.Serious, 24);
                }

                // Make sure the value can fit in memory
                if (-32768 > i || i > 32767)
                {
                    throw new Assembler.ErrorException(ErrCat.Serious, 24);
                }

                Memory.GetInstance().DataPushInt(i);

                n--;
            }
        }

        /**
         * Reads n number of characters from a file or the console and pushes them
         * on the data stack.
         *
         * @param n The number of items to read from the file.
         *
         * @refcode OP2.6
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Readc(int n)
        {
            while (n > 0)
            {
                Console.Write("Enter 1 or 2 characters: ");
                string str = Console.ReadLine();
                int i = 0;

                // make sure something is inputted
                if (str.Length == 0)
                {
                    throw new Assembler.ErrorException(ErrCat.Serious, 23);
                } // Give a warning if they enter more than two characters
                else if (str.Length > 2)
                {
                    Console.WriteLine(String.Format("RUNTIME ERROR: {0}", new Assembler.ErrorException(ErrCat.Warning, 1)));
                }

                i = (int)str[0];

                // Moves the bits over by 8 places so we can add on the other character if it exists, otherwise
                // this will just ensure the character is left aligned.
                i *= 256;

                if (str.Length == 2)
                {
                    i += (int)str[1];
                }              

                Memory.GetInstance().DataPushInt(i);

                n--;
            }
        }

        /**
         * Pops n number of items off of the stack as integers and displays them to the screen.
         *
         * @param n The number of items to pop and display.
         *
         * @refcode OP2.7
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Writen(int n)
        {
            Memory m = Memory.GetInstance();

            while (n > 0)
            {
                int i = m.DataPopInt();

                Console.Write(i);

                n--;
            }
        }

        /**
         * Pops n number of items off of the stack as characters and displays them to the screen.
         *
         * @param n The number of items to pop and display.
         *
         * @refcode OP2.7
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Writec(int n)
        {
            Memory m = Memory.GetInstance();

            while (n > 0)
            {
                int i = m.DataPop();

                // get the first character by removing the last 8 bits
                int char2 = i % 256;

                // shift the bits over 8 places
                int char1 = i / 256;

                // convert from integers to characters
                char c1 = (char)char1;
                char c2 = (char)char2;

                Console.Write(c1.ToString() + c2.ToString());

                n--;
            }
        }
    }
}
