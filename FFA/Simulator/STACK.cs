using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator
{
    class STACK
    {
        /**
         * Executes the STACK instruction. Breaks the rest of the binary string apart and calls
         * the desired procedure providing information needed.
         *
         * @param function The STACK procedure to be called.
         * @param bin The binary representation of the instruction.
         * 
         * @refcode OP0
         * @errtest 
         * @errmsg
         * @author Jacob Peddicord
         * @creation May 20, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Run(string function, string bin)
        {
            // check if the literal flag is set
            bool literal = (bin[5] == '1');

            // pull the address out of the instruction
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

        /**
         * Pushes the element located at the memory address onto the stack. If
         * the literal flag is set then the value of addr is pushed instead.
         *
         * @param addr The address of the item to push, or the value to push if a literal
         * @param literal Boolean to indicate if addr is a literal
         * 
         * @refcode OP0.5
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 18, 2011
         * @modlog
         *  - May 21, 2011 - Jacob - Forgot about literal values in the instruction.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
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

        /**
         * Pops the top item off of the stack and stores it at the
         * specified memory address.
         *
         * @param addr The memory location to store the item from the stack.
         * 
         * @refcode OP0.6
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 18, 2011
         * @modlog 
         *  - May 22, 2011 - Andrew - Shortened the procedure a little bit.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Pop(int addr)
        {
            Memory m = Memory.GetInstance();
            int i = m.DataPop();
            m.SetWord(addr, i);
        }

        /**
         * Compares the top item of the stack with the value located at the specified
         * memory address, or with the value in addr if flagged as a literal. The item
         * popped off of the stack is not placed back on and will be lost. The value pushed
         * to the test stack will be based on how the item from the data stack (s) relates
         * to the item from memory or the literal value in addr (m). <br />
         * If s = m, push 0 <br />
         * If s &lt; m, push 1 <br />
         * If s &gt; m, push 2
         *
         * @refcode OP0.7
         * @errtest 
         * @errmsg
         * @author Andrew Buelow
         * @creation May 18, 2011
         * @modlog 
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Test(int addr, bool literal)
        {
            Memory m = Memory.GetInstance();
            int i, d = 0, w, data;
            
            // Pull the top value off of the data stack
            d = m.DataPopInt();

            // If literal boolean is set then use the addr as our comparison value.
            // Otherwise use it as an address and get the value located at addr.
            if (literal)
            {
                data = addr;
            }
            else
            {
                data = m.GetWord(addr);
            }

            // Convert from twos complement.
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
