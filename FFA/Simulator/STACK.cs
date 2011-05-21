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
            if (function == "PUSH")
            {
                throw new NotImplementedException();
            }
            else if (function == "POP")
            {
                throw new NotImplementedException();
            }
            else if (function == "TEST")
            {
                throw new NotImplementedException();
            }
        }

        public static void Push(string addr)
        {
            // TODO Check that addr is in range.
            try
            {
                Memory m = Memory.GetInstance();
                m.DataPush(m.GetWord(Convert.ToInt32(addr, 16)));
            }
            catch (Exception)
            {
                // TODO Add error handling (too many items)
            }
        }

        public static void Pop(string addr)
        {
            // TODO Check that addr is in range.
            int i = 0;
            try
            {
                i = Memory.GetInstance().DataPop();
                Memory.GetInstance().SetWord(Convert.ToInt32(addr, 16), i);
            }
            catch (Exception)
            {
                // TODO Add error handling (nothing on stack)
            }
        }

        public static void Test(string addr)
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

            w = Assembler.BinaryHelper.ConvertNumber(m.GetWord(Convert.ToInt32(addr, 16)), 16);

            i = d.CompareTo(w);

            try
            {
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
            catch (Exception)
            {
                //TODO Add error handling if too many items on test stack
            }
        }
    }
}
