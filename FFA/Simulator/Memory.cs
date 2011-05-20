using System;
using System.Collections.Generic;

namespace Simulator
{
    public class Memory
    {
        private static Memory inst = null;

        private int[] storage = new int[1024];

        private Stack<int> dataStack = new Stack<int>();

        private Stack<int> testStack = new Stack<int>();

        /**
         *
         */
        private Memory()
        {
            // fill each address with its location
            for (int i = 0; i < 1024; i++)
            {
                this.storage[i] = i;
            }
        }

        public static Memory GetInstance()
        {
            if (Memory.inst == null)
            {
                Memory.inst = new Memory();
            }
            return Memory.inst;
        }

        public int GetWord(int address)
        {
            return this.storage[address];
        }

        /**
         * Like GetWord, but converts from 16-bit 2's complement first.
         */
        public int GetWordInt(int address)
        {
            return Assembler.BinaryHelper.ConvertNumber(this.storage[address], 16);
        }

        public void SetWord(int address, int val)
        {
            this.storage[address] = val;
        }

        public void SetWordInt(int address, int val)
        {
            // if negative, convert to the appropriate 2's complement representation
            if (val < 0)
            {
                val = Assembler.BinaryHelper.ConvertNumber(val, 16);
            }
            this.storage[address] = val;
        }

        public void DataPush(int data)
        {
            if (this.dataStack.Count == 256)
            {
                throw new StackOverflowException();
            }
            this.dataStack.Push(data);
        }

        public int DataPop()
        {
            return this.dataStack.Pop();
        }

        public int DataSize()
        {
            return this.dataStack.Count;
        }

        public void ClearData()
        {
            this.dataStack.Clear();
        }

        public void TestPush(short data)
        {
            if (this.testStack.Count == 5)
            {
                throw new StackOverflowException();
            }
            this.testStack.Push(data);
        }

        public int TestPop()
        {
            return this.testStack.Pop();
        }

        public int TestSize()
        {
            return this.testStack.Count;
        }

        public void ClearTest()
        {
            this.testStack.Clear();
        }

        public int[] GetDataStack()
        {
            return this.dataStack.ToArray();
        }

        public int[] GetTestStack()
        {
            return this.testStack.ToArray();
        }
    }
}

