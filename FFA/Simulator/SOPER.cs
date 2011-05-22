using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErrCat = Assembler.Errors.Category; 

namespace Simulator
{
    class SOPER
    {
        public static void Run(string function, string bin)
        {
            if (function == "ADD")
            {
                throw new NotImplementedException();
            }
            else if (function == "SUB")
            {
                throw new NotImplementedException();
            }
            else if (function == "MUL")
            {
                throw new NotImplementedException();
            }
            else if (function == "DIV")
            {
                throw new NotImplementedException();
            }
            else if (function == "OR")
            {
                throw new NotImplementedException();
            }
            else if (function == "AND")
            {
                throw new NotImplementedException();
            }
            else if (function == "READN")
            {
                throw new NotImplementedException();
            }
            else if (function == "READC")
            {
                throw new NotImplementedException();
            }
            else if (function == "WRITEN")
            {
                throw new NotImplementedException();
            }
            else if (function == "WRITEC")
            {
                throw new NotImplementedException();
            }
        }

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

        public static void Div(int n)
        {
            if (n > 1)
            {

                    Memory m = Memory.GetInstance();
                    int total = m.DataPopInt();

                    while (n > 1)
                    {
                        int i = m.DataPopInt();

                        if (i == 0)
                        {
                            throw new Assembler.ErrorException(ErrCat.Serious, 12);
                        }

                        total = total / i;
                    }

                    m.DataPushInt(total);

            }
        }

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

        public static void Readn(int n)
        {
            while (n > 0)
            {
                Console.Write("Enter an integer: ");
                string str = Console.ReadLine();
                int i = 0;

                if (str.Length == 0)
                {
                    throw new Assembler.ErrorException(ErrCat.Serious, 23);
                }

                try
                {
                    i = Convert.ToInt32(str);
                }
                catch (FormatException)
                {
                    throw new Assembler.ErrorException(ErrCat.Serious, 25);
                }
                catch (OverflowException)
                {
                    throw new Assembler.ErrorException(ErrCat.Serious, 24);
                }

                if (-32768 > i || i > 32767)
                {
                    throw new Assembler.ErrorException(ErrCat.Serious, 24);
                }

                Memory.GetInstance().DataPushInt(i);

                n--;
            }
        }

        public static void Readc(int n)
        {
            while (n > 0)
            {
                Console.Write("Enter 1 or 2 characters: ");
                string str = Console.ReadLine();
                int i = 0;

                if (str.Length == 0)
                {
                    throw new Assembler.ErrorException(ErrCat.Serious, 23);
                }
                else if (str.Length > 2)
                {
                    throw new Assembler.ErrorException(ErrCat.Warning, 1);
                }

                i = (int)str[0];

                i *= 256;

                if (str.Length == 2)
                {
                    i += (int)str[1];
                }              

                Memory.GetInstance().DataPushInt(i);

                n--;
            }
        }

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

        public static void Writec(int n)
        {
            Memory m = Memory.GetInstance();

            while (n > 0)
            {
                int i = m.DataPop();

                int char2 = i % 256;

                int char1 = i / 256;

                char c1 = (char)char1;
                char c2 = (char)char2;

                Console.Write(c1 + c2);

                n--;
            }
        }
    }
}
