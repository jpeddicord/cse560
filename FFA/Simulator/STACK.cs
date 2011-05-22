using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator
{
    class STACK
    {
        public static void Run(string function, string bin)
        {
            bool literal = (bin[5] == '1');
            int addr = Convert.ToInt32(bin.Substring(6), 2);
            if (function == "PUSH")
            {
                STACK.Push(addr, literal);
            }
            else if (function == "POP")
            {
                STACK.Pop(addr);
            }
            else if (function == "TEST")
            {
                STACK.Test(addr, literal);
            }
        }

        public static void Push(int addr, bool literal)
        {
            Memory m = Memory.GetInstance();
            if (literal)
            {
                m.DataPush(addr);
            }
            else
            {
                m.DataPush(m.GetWord(addr));
            }
        }

        public static void Pop(int addr)
        {
            int i = 0;

            i = Memory.GetInstance().DataPop();
            Memory.GetInstance().SetWord(addr, i);
        }

        public static void Test(int addr, bool literal)
        {
            Memory m = Memory.GetInstance();
            int i, d = 0, w;

            d = Assembler.BinaryHelper.ConvertNumber(m.DataPop(), 16);

            int data;
            if (literal)
            {
                data = addr;
            }
            else
            {
                data = m.GetWord(addr);
            }

            w = Assembler.BinaryHelper.ConvertNumber(data, 16);

            i = d.CompareTo(w);

            if (i < 0)
            {
                m.TestPush(1);
            }
            else if (i > 0)
            {
                m.TestPush(2);
            }
            else
            {
                m.TestPush(0);
            }

        }
    }
}
