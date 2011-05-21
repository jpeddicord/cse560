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
            // TODO: LITERAL FLAG!
            int addr = Convert.ToInt32(bin.Substring(6), 2);
            if (function == "PUSH")
            {
                STACK.Push(addr);
            }
            else if (function == "POP")
            {
                STACK.Pop(addr);
            }
            else if (function == "TEST")
            {
                STACK.Test(addr);
            }
        }

// TODO: LITERAL FLAG
        public static void Push(int addr)
        {
            // TODO Check that addr is in range.
            try
            {
                Memory m = Memory.GetInstance();
                m.DataPush(m.GetWord(addr));
            }
            catch (Exception)
            {
                // TODO Add error handling (too many items)
            }
        }

// TODO: LITERAL FLAG
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
        
// TODO: LITERAL FLAG
        public static void Test(int addr)
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

            w = Assembler.BinaryHelper.ConvertNumber(m.GetWord(addr), 16);

            i = d.CompareTo(w);

            try
            {
                if (i < 0)
                {
                    m.TestPush(1); // FIXME: 3?
                }
                else if (i > 0)
                {
                    m.TestPush(2); // FIXME: 4?
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
