using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

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
         *     \code GROUP FUNCTION BITS \endcode
         * where GROUP is the instruction group, FUNCTION is the named
         * function, and BITS is the corresponding bytecode (as a string of
         * 0's and 1's) to the function.
         *
         * @refcode ???
         * @errtest Things
         * @errmsg More things
         * @creation Someday
         * @modlog
         *  Jacob, Tuesday, Fixed the herp to the derp
         * @codestandard Mark
         * @teststandard Andrew
         */
        private Instructions()
        {
            Trace.WriteLine("Creating instance of Instructions.", "Instructions");
            this.instructions = new Dictionary<string, Dictionary<string, string>>();
            
            // fill the instruction mapping with data from the file
            foreach (string line in Properties.Resources.instructions.Split('\n'))
            {
                string[] parts = line.Split(' ');
                if (!this.instructions.ContainsKey(parts[0]))
                {
                    this.instructions[parts[0]] = new Dictionary<string, string>();
                }
                this.instructions[parts[0]][parts[1]] = parts[2];
            }
        }

        public static Instructions GetInstance()
        {
            Trace.WriteLine("Request of instance of Instructions.", "Instructions");
            if (Instructions.instance == null)
            {
                Instructions.instance = new Instructions();
            }
            
            return Instructions.instance;
        }

        public bool IsInstruction(string instrGroup, string function)
        {
            Trace.WriteLine(String.Format("Check if {0} is valid function in {1} category.",
                function, instrGroup), "Instructions");

            return this.instructions.ContainsKey(instrGroup.ToUpper())
                && this.instructions[instrGroup.ToUpper()].ContainsKey(function.ToUpper());
        }

        public bool IsGroup(string instrGroup)
        {
            Trace.WriteLine(String.Format("Check if {0} is valid instruction category", 
                instrGroup), "Instructions");

            return this.instructions.ContainsKey(instrGroup.ToUpper());
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
            instrGroup = instrGroup.ToUpper();
            function = function.ToUpper();
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
