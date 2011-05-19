using System;
using System.Collections.Generic;

namespace Simulator
{
    public class Memory
    {
        private static Memory inst = null;

        private string[] storage = new string[1024];

        private Stack<string> dataStack = new Stack<string>();

        private Stack<short> testStack = new Stack<short>();

        /**
         *
         */
        private Memory()
        {
            // fill each address with its hex location
            for (int i = 0; i < 1024; i++)
            {
                this.storage[i] = Convert.ToString(i, 16).PadLeft(4, '0');
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

        public string GetWord(int address)
        {
            return this.storage[address];
        }

        public string GetWord(string hexAddress)
        {
            return this.GetWord(Convert.ToInt32(hexAddress, 16));
        }

        public void SetWord(int address, string val)
        {
            this.storage[address] = val.PadLeft(4, '0');
        }

        public void SetWord(string hexAddress, string val)
        {
            this.SetWord(Convert.ToInt32(hexAddress, 16), val);
        }

        public void DataPush(string data)
        {
            if (this.dataStack.Count == 256)
            {
                throw new StackOverflowException();
            }
            this.dataStack.Push(data);
        }

        public string DataPop()
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

        public short TestPop()
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

        public string[] GetDataStack()
        {
            return this.dataStack.ToArray();
        }

        public short[] GetTestStack()
        {
            return this.testStack.ToArray();
        }
    }
}

