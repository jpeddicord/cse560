using System;

namespace Simulator
{
    public class Runtime
    {
        private static Runtime inst = null;

        private int lc = 0;

        private Runtime()
        {
        }

        public static Runtime GetInstance()
        {
            if (Runtime.inst == null)
            {
                Runtime.inst = new Runtime();
            }
            return Runtime.inst;
        }

        public int LC
        {
            get { return this.lc; }
            set
            { // TODO: error check
                this.lc = value;
            }
        }

        public void Run()
        {
            // get the data at the current LC
            var mem = Memory.GetInstance();
            var instr = Assembler.Instructions.GetInstance();
            string category, function;
            string bin = Convert.ToString(mem.GetWord(this.lc), 2);

            // look up the associated insruction
            instr.ReverseLookup(bin.Substring(0, 5), out category, out function);

            try
            {
                ProcessInstruction(category, function, bin);
            }
            catch (Assembler.ErrorException ex)
            {
                Console.WriteLine(String.Format("RUNTIME ERROR: {0}", ex));
                // TODO: abort?
            }
        }

        public void ProcessInstruction(string category, string function, string bin)
        {
            if (category == "CNTL")
            {
                if (function == "HALT")
                {

                }
                else if (function == "DUMP")
                {

                }
                else if (function == "CLRD")
                {
                    CNTL.Clrd();
                }
                else if (function == "CLRT")
                {
                    CNTL.Clrt();
                }
                else if (function == "GOTO")
                {

                }
            }
            else if (category == "STACK")
            {
                if (function == "PUSH")
                {

                }
                else if (function == "POP")
                {

                }
                else if (function == "TEST")
                {

                }
            }
            else if (category == "JUMP")
            {
                if (function == "=")
                {

                }
                else if (function == "^=")
                {

                }
                else if (function == "<")
                {

                }
                else if (function == ">")
                {

                }
                else if (function == "<=")
                {

                }
                else if (function == ">=")
                {

                }
                else if (function == "TNULL")
                {

                }
                else if (function == "DNULL")
                {

                }
            }
            else if (category == "SOPER")
            {
                if (function == "ADD")
                {

                }
                else if (function == "SUB")
                {

                }
                else if (function == "MUL")
                {

                }
                else if (function == "DIV")
                {

                }
                else if (function == "OR")
                {

                }
                else if (function == "AND")
                {

                }
                else if (function == "READN")
                {

                }
                else if (function == "READC")
                {

                }
                else if (function == "WRITEN")
                {

                }
                else if (function == "WRITEC")
                {

                }
            }
            else if (category == "MOPER")
            {
                if (function == "ADD")
                {

                }
                else if (function == "SUB")
                {

                }
                else if (function == "MUL")
                {

                }
                else if (function == "DIV")
                {

                }
                else if (function == "OR")
                {

                }
                else if (function == "AND")
                {

                }
                else if (function == "READN")
                {

                }
                else if (function == "READC")
                {

                }
                else if (function == "WRITEN")
                {

                }
                else if (function == "WRITEC")
                {

                }
            }
        }
    }
}

