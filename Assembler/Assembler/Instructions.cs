using System;
using System.IO;
using System.Collections.Generic;

namespace Assembler
{
    public class InstructionException : ApplicationException
    {
        public InstructionException() : base()
        {
        }
        public InstructionException(string s) : base(s)
        {
        }
        public InstructionException(string s, Exception ex) : base(s, ex)
        {
        }
    }

    /**
     * Contains instructions and their opcodes. Documentation needs to be written.
     */
    public class Instructions
    {
        /**
         * Singleton instance of this class.
         */
        private static Instructions instance;

        /**
         * DOCUMENT ME
         */
        private Dictionary<string, Dictionary<string, string>> instructions;

        /**
         * Create a translation object. Loads instruction translation data
         * from the given file in filename. The file should have lines of
         * the format:
         *     GROUP FUNCTION BITS
         * where GROUP is the instruction group, FUNCTION is the named
         * function, and BITS is the corresponding bytecode (as a string of
         * 0's and 1's) to the function.
         */
        private Instructions()
        {
            this.instructions = new Dictionary<string, Dictionary<string, string>>();
            
            // fill the instruction mapping with data from the file
            foreach (string line in Properties.Resources.instructions.Split('\n'))
            {
                string[] parts = line.Split(' ');
                this.instructions[parts[0]] = new Dictionary<string, string>();
                this.instructions[parts[0]][parts[1]] = parts[2];
            }
        }

        public static Instructions GetInstance()
        {
            if (Instructions.instance == null)
            {
                instance = new Instructions();
            }
            
            return instance;
        }

        public bool IsInstruction(string instrGroup, string function)
        {
            return this.instructions.ContainsKey(instrGroup) && this.instructions[instrGroup].ContainsKey(function);
        }

        public bool IsGroup(string instrGroup)
        {
            return this.instructions.ContainsKey(instrGroup);
        }

        /**
         * Get a bytecode string corresponding to the proided instruction
         * group and function. The returned string is guaranteed to be
         * 5 characters (bits) long.
         * 
         * @param instrGroup Instruction group of function
         * @param function Function of group to get bytecode for
         * @return string of five '0' and '1' characters
         */
        public string GetBytecodeString(string instrGroup, string function)
        {
            if (!this.instructions.ContainsKey(instrGroup))
            {
                throw new InstructionException("\"" + instrGroup + "\" is not a valid group.");
            }
            
            if (!this.instructions[instrGroup].ContainsKey(function))
            {
                throw new InstructionException("\"" + function + "\" is not a valid function for " + instrGroup);
            }
            
            return this.instructions[instrGroup][function];
        }
        
    }
}
