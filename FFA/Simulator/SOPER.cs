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
            bool character = (bin[5] == '1');
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
                    Console.WriteLine(String.Format("RUNTIME ERROR: {0}", new Assembler.ErrorException(ErrCat.Warning, 1)));
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
