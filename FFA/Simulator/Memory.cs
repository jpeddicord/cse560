using System;
namespace Simulator
{
    public class Memory
    {
        private string[] storage = new string[1024];

        /**
         *
         */
        public Memory()
        {
            // fill each address with its hex location
            for (int i = 0; i < 1024; i++)
            {
                this.storage[i] = Convert.ToString(i, 16).PadLeft(4, '0');
            }
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

    }
}

