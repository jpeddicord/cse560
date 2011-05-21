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
            if (function == "=")
            {
                throw new NotImplementedException();
            }
            else if (function == "^=")
            {
                throw new Assembler.ErrorException(Assembler.Errors.Category.Serious, 22);
            }
            else if (function == "<")
            {
                throw new NotImplementedException();
            }
            else if (function == ">")
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
            else if (function == "DNULL")
            {
                throw new NotImplementedException();
            }
        }

        public static void Equal(string addr, ref int LC)
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
                LC = Convert.ToInt32(addr, 16);
            }
        }

        public static void Less(string addr, ref int LC)
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
                LC = Convert.ToInt32(addr, 16);
            }
        }

        public static void Greater(string addr, ref int LC)
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
                LC = Convert.ToInt32(addr, 16);
            }
        }

        public static void Dnull(string addr, ref int LC)
        {
            if (Memory.GetInstance().GetDataStack().Length == 0)
            {
                LC = Convert.ToInt32(addr, 16);
            }
        }

        public static void Tnull(string addr, ref int LC)
        {
            if (Memory.GetInstance().GetTestStack().Length == 0)
            {
                LC = Convert.ToInt32(addr, 16);
            }
        }
    }
}
