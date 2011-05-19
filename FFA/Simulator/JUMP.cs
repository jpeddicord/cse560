using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator
{
    // TODO Add memory address checking for all of these
    class JUMP
    {

        public JUMP()
        {
            // Logging?
        }

        public static void Equal(string addr, ref string LC)
        {
            int i = -1;

            try
            {
                i = Memory.GetInstance().TestPop();
            }
            catch (Exception)
            {
                //TODO Nothing on test stack
            }

            if (i == 0)
            {
                LC = addr;
            }
        }

        public static void Less(string addr, ref string LC)
        {
            int i = -1;

            try
            {
                i = Memory.GetInstance().TestPop();
            }
            catch (Exception)
            {
                //TODO Nothing on test stack
            }

            if (i == 1)
            {
                LC = addr;
            }
        }

        public static void Greater(string addr, ref string LC)
        {
            int i = -1;

            try
            {
                i = Memory.GetInstance().TestPop();
            }
            catch (Exception)
            {
                //TODO Nothing on test stack
            }

            if (i == 2)
            {
                LC = addr;
            }
        }

        public static void Dnull(string addr, ref string LC)
        {
            if (Memory.GetInstance().GetDataStack().Length == 0)
            {
                LC = addr;
            }
        }

        public static void Tnull(string addr, ref string LC)
        {
            if (Memory.GetInstance().GetTestStack().Length == 0)
            {
                LC = addr;
            }
        }
    }
}
