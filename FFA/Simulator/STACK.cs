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
            // TODO Check that addr is in range.
            try
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
            catch (Exception)
            {
                // TODO Add error handling (too many items)
            }
        }

        public static void Pop(int addr)
        {
            // TODO Check that addr is in range.
            int i = 0;
            try
            {
                i = Memory.GetInstance().DataPop();
                Memory.GetInstance().SetWord(addr, i);
            }
            catch (Exception)
            {
                // TODO Add error handling (nothing on stack)
            }
        }

        public static void Test(int addr, bool literal)
        {
            Memory m = Memory.GetInstance();
            int i, d = 0, w;

            try
            {
                d = Assembler.BinaryHelper.ConvertNumber(m.DataPop(), 16);
            }
            catch (Exception)
            {
                // TODO Add error handling (nothing on stack)
            }

            int data;
            if (literal)
            {
                data = addr;
            }
            else
            {
                data = m.GetWord(addr);
            }
            // TODO: verify this:
            w = Assembler.BinaryHelper.ConvertNumber(data, 16);

            i = d.CompareTo(w);

            try
            {
                if (i < 0)
                {
                    m.TestPush(1); // FIXME: 2?
                }
                else if (i > 0)
                {
                    m.TestPush(2); // FIXME: 3?
                }
                else
                {
                    m.TestPush(0);
                }
            }
            catch (Exception)
            {
                //TODO Add error handling if too many items on test stack
            }
        }
    }
}
