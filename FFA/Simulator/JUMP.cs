using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator
{
    class JUMP
    {

        /**
         * Executes the JUMP instruction. Breaks the rest of the binary string apart and calls
         * the desired procedure.
         *
         * @refcode OP1
         * @errtest 
         * @errmsg
         * @author Jacob Peddicord
         * @creation May 20, 2011
         * @modlog 
         *  - May 22, 2011 - Andrew - Wasn't actually setting the new LC.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
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
            Runtime.GetInstance().LC = lc;
        }

        /**
         * Pulls the first item off of the test stack and sets the new LC
         * if it is equal to 0 (equal).
         *
         * @param addr the address to branch to if conditions are met
         * @param LC The current location counter to be modified if jumping
         * 
         * @refcode OP1.0
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Equal(int addr, ref int LC)
        {
            int i = -1;

            i = Memory.GetInstance().TestPop();

            if (i == 0)
            {
                LC = addr;
            }
        }

        /**
         * Pulls the first item off of the test stack and sets the new LC
         * if it is equal to 1 (less than).
         *
         * @param addr the address to branch to if conditions are met
         * @param LC The current location counter to be modified if jumping
         * 
         * @refcode OP1.2
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Less(int addr, ref int LC)
        {
            int i = -1;

            i = Memory.GetInstance().TestPop();

            if (i == 1)
            {
                LC = addr;
            }
        }

        /**
         * Pulls the first item off of the test stack and sets the new LC
         * if it is equal to 2 (greater than).
         *
         * @param addr the address to branch to if conditions are met
         * @param LC The current location counter to be modified if jumping
         *
         * @refcode OP1.3
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Greater(int addr, ref int LC)
        {
            int i = -1;

            i = Memory.GetInstance().TestPop();

            if (i == 2)
            {
                LC = addr;
            }
        }

        /**
         * Take the jump if there is nothing on the data stack.
         *
         * @param addr the address to branch to if conditions are met
         * @param LC The current location counter to be modified if jumping
         *
         * @refcode OP1.7
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Dnull(int addr, ref int LC)
        {
            if (Memory.GetInstance().GetDataStack().Length == 0)
            {
                LC = addr;
            }
        }

        /**
         * Take the jump if there is nothing on the test stack.
         *
         * @param addr the address to branch to if conditions are met
         * @param LC The current location counter to be modified if jumping
         *
         * @refcode OP1.6
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 20, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Tnull(int addr, ref int LC)
        {
            if (Memory.GetInstance().GetTestStack().Length == 0)
            {
                LC = addr;
            }
        }
    }
}
