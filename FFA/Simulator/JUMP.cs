using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator
{
    // TODO Add memory address checking for all of these
    class JUMP
    {

        public static void Run(string function, string bin)
        {
            // save the LC for operations
            var runtime = Runtime.GetInstance();
            int lc = runtime.LC;

            // get the jump destination
            int dest = Convert.ToInt32(bin.Substring(6), 2);

            // see which type of jump to perform
            if (function == "=")
            {
                JUMP.Equal(dest, ref lc);
            }
            else if (function == "^=")
            {
                throw new Assembler.ErrorException(Assembler.Errors.Category.Serious, 22);
            }
            else if (function == "<")
            {
                JUMP.Less(dest, ref lc);
            }
            else if (function == ">")
            {
                JUMP.Greater(dest, ref lc);
            }
            else if (function == "<=")
            {
                throw new Assembler.ErrorException(Assembler.Errors.Category.Serious, 22);
            }
            else if (function == ">=")
            {
                throw new Assembler.ErrorException(Assembler.Errors.Category.Serious, 22);
            }
            else if (function == "TNULL")
            {
                JUMP.Tnull(dest, ref lc);
            }
            else if (function == "DNULL")
            {
                JUMP.Dnull(dest, ref lc);
            }

            // set the LC
        }

        public static void Equal(int addr, ref int LC)
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

        public static void Less(int addr, ref int LC)
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

        public static void Greater(int addr, ref int LC)
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

        public static void Dnull(int addr, ref int LC)
        {
            if (Memory.GetInstance().GetDataStack().Length == 0)
            {
                LC = addr;
            }
        }

        public static void Tnull(int addr, ref int LC)
        {
            if (Memory.GetInstance().GetTestStack().Length == 0)
            {
                LC = addr;
            }
        }
    }
}
