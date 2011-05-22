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
                throw new Assembler.ErrorException(ErrCat.Warning, 1);
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
