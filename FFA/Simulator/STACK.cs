using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator
{
    class STACK
    {
        public STACK()
        {
            // Logging?
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
            String s = "";
            try
            {
                s = Memory.GetInstance().DataPop();
                Memory.GetInstance().SetWord(Convert.ToInt32(addr, 16), s);
            }
            catch (Exception)
            {
                // TODO Add error handling (nothing on stack)
            }
        }

        public static void Test(string addr)
        {
            Memory m = Memory.GetInstance();
            String s = "", t;
            int i;

            try
            {
                s = m.DataPop();
            }
            catch (Exception)
            {
                // TODO Add error handling (nothing on stack)
            }

            t = m.GetWord(Convert.ToInt32(addr, 16));

            i = Convert.ToInt32(s, 16).CompareTo(Convert.ToInt32(t, 16));

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
            finally
            {
                m.DataPush(s);
            }
        }
    }
}
