using System;
using System.Collections.Generic;
using ErrCat = Assembler.Errors.Category; 

namespace Simulator
{
    /**
     * Memory representation.
     */
    public class Memory
    {
        /**
         * The singleton Memory instance.
         */
        private static Memory inst = null;

        /**
         * Main memory storage.
         */
        private int[] storage = new int[1024];

        /**
         * The data stack.
         */
        private Stack<int> dataStack = new Stack<int>();

        /**
         * The test stack.
         */
        private Stack<int> testStack = new Stack<int>();

        /**
         * Create a new instance of memory and fill each location with its
         * address. Since addresses are below 1024, each filled word will
         * always be a CNTL HALT, with the exit code being the address.
         *
         * @refcode H2
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private Memory()
        {
            // fill each address with its location
            for (int i = 0; i < 1024; i++)
            {
                this.storage[i] = i;
            }
        }

        /**
         * Get the single Memory instance.
         *
         * @return active Memory instance
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static Memory GetInstance()
        {
            if (Memory.inst == null)
            {
                Memory.inst = new Memory();
            }
            return Memory.inst;
        }

        /**
         * Get a word of memory at the given address.
         *
         * @param address location to retrieve memory for
         * @return 16-bit integer of memory stored at that location
         * @refcode H2
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public int GetWord(int address)
        {
            if (0 > address || address > 1023)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 26);
            }
            return this.storage[address];
        }

        /**
         * Like GetWord, but converts from 16-bit 2's complement first.
         *
         * @param address location to retrieve memory for
         * @return 2's complement representation of memory
         * @refcode H2
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public int GetWordInt(int address)
        {
            if (0 > address || address > 1023)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 26);
            }
            return Assembler.BinaryHelper.ConvertNumber(this.storage[address], 16);
        }

        /**
         * Set a word of memory at a given address.
         *
         * @param address location to retrieve memory for
         * @param val value to set the word to
         * @refcode H2
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void SetWord(int address, int val)
        {
            if (0 > val || val > 65535)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 31);
            }
            else if (0 > address || address > 1023)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 26);
            }
            this.storage[address] = val;
        }

        /**
         * Like SetWord, but converts to 16-bit 2's complement representation
         * first before storing into memory.
         *
         * @param address location to retrieve memory for
         * @param val integer value to convert and set the word to
         * @refcode H2
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void SetWordInt(int address, int val)
        {
            if (32767 < val || val < -32768)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 11);
            }
            else if (0 > address || address > 1023)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 26);
            }
            
            // if negative, convert to the appropriate 2's complement representation
            if (val < 0)
            {
                val = Assembler.BinaryHelper.ConvertNumber(val, 16);
            }
            this.storage[address] = val;
        }

        /**
         * Push a single element onto the data stack.
         *
         * @param data Element to push onto the stack
         * @refcode H3.1
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void DataPush(int data)
        {
            if (this.dataStack.Count == 256)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 27);
            }
            else if (0 > data || data > 65535)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 11);
            }
            this.dataStack.Push(data);
        }

        /**
         * Like DataPush, but converts to 16-bit 2's complement representation
         * first before adding to the stack.
         *
         * @param data Element to push onto the stack
         * @refcode H3.1
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Andrew Buelow
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void DataPushInt(int data)
        {
            if (this.dataStack.Count == 256)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 27);
            }
            else if (32767 < data || data < -32768)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 11);
            }

            if (data < 0)
            {
                data = Assembler.BinaryHelper.ConvertNumber(data, 16);
            }

            this.dataStack.Push(data);
        }

        /**
         * Pop a single item off of the data stack.
         *
         * @return the popped element
         * @refcode H3.1
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public int DataPop()
        {
            if (this.dataStack.Count == 0)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 29);
            }
            return this.dataStack.Pop();
        }

        /**
         * Pop a single item off of the data stack.
         *
         * @return the popped element
         * @refcode H3.1
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public int DataPopInt()
        {
            if (this.dataStack.Count == 0)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 29);
            }
            return Assembler.BinaryHelper.ConvertNumber(this.dataStack.Pop(), 16);
        }

        /**
         * Get the size of the data stack.
         *
         * @return size of the data stack
         * @refcode H3.1
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public int DataSize()
        {
            return this.dataStack.Count;
        }

        /**
         * Clear the data stack.
         *
         * @refcode H3.1
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void ClearData()
        {
            this.dataStack.Clear();
        }

        /**
         * Push an element onto the test stack.
         *
         * @param data integer to push onto the stack
         * @refcode H3.2
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void TestPush(int data)
        {
            if (this.testStack.Count == 5)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 28);
            }
            this.testStack.Push(data);
        }

        /**
         * Pop an item off of the test stack.
         *
         * @return popped item
         * @refcode H3.2
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public int TestPop()
        {
            if (this.testStack.Count == 0)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 30);
            }
            return this.testStack.Pop();
        }

        /**
         * Get the size of the test stack.
         *
         * @return the size of the test stack
         * @refcode H3.2
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public int TestSize()
        {
            return this.testStack.Count;
        }

        /**
         * Clear the test stack.
         *
         * @refcode H3.2
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void ClearTest()
        {
            this.testStack.Clear();
        }

        /**
         * Get the data stack as an array.
         *
         * @return data stack represented as an array
         * @refcode H3.1
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Andrew Buelow
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public int[] GetDataStack()
        {
            return this.dataStack.ToArray();
        }

        /**
         * Get the test stack as an array.
         *
         * @return test stack represented as an array
         * @refcode H3.2
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Andrew Buelow
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public int[] GetTestStack()
        {
            return this.testStack.ToArray();
        }
    }
}

