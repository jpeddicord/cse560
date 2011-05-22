using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErrCat = Assembler.Errors.Category; 

namespace Simulator
{
    class MOPER
    {
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

        public static void Add(int addr)
        {
            Memory m = Memory.GetInstance();

            int i = m.DataPopInt();
            int j = m.GetWordInt(addr);

            int val = i + j;

            m.DataPushInt(val);
        }

        public static void Sub(int addr)
        {
            Memory m = Memory.GetInstance();

            int i = m.DataPopInt();
            int j = m.GetWordInt(addr);

            int val = i - j;

            m.DataPushInt(val);
        }

        public static void Mul(int addr)
        {
            Memory m = Memory.GetInstance();

            int i = m.DataPopInt();
            int j = m.GetWordInt(addr);

            int val = i * j;

            m.DataPushInt(val);
        }

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

            m.DataPushInt(val);
        }

        public static void Or(int addr)
        {
            Memory m = Memory.GetInstance();

            int i = m.DataPopInt();
            int j = m.GetWordInt(addr);

            int val = i | j;

            m.DataPushInt(val);
        }

        public static void And(int addr)
        {
            Memory m = Memory.GetInstance();

            int i = m.DataPopInt();
            int j = m.GetWordInt(addr);

            int val = i & j;

            m.DataPushInt(val);
        }

        public static void Readn(int addr)
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

            Memory.GetInstance().SetWordInt(addr, i);
        }

        public static void Readc(int addr)
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

            Memory.GetInstance().SetWordInt(addr, i);
        }

        public static void Writen(int addr)
        {
            Memory m = Memory.GetInstance();
            int i = m.GetWordInt(addr);

            Console.WriteLine(i);
        }

        public static void Writec(int addr)
        {
            Memory m = Memory.GetInstance();

            int i = m.GetWord(addr);

            int char2 = i % 256;
            int char1 = i / 256;

            char c1 = (char)char1;
            char c2 = (char)char2;

            Console.Write(c1 + c2);
        }
    }
}
