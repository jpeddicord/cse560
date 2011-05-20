using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assembler
{
    /**
     * Exception thrown when an invalid instruction is looked up.
     */
    public class InstructionException : ApplicationException
    {
        public InstructionException() : base() {}
        public InstructionException(string s) : base(s) {}
        public InstructionException(string s, Exception ex) : base(s, ex) {}
    }

    /**
     * Contains instructions and their opcodes.
     */
    public class Instructions
    {
        /**
         * Singleton instance of this class.
         */
        private static Instructions instance;

        /**
         * Stores the instruction group and function to bitcode map.
         * The key for the topmost dictionary is the group name, which
         * contains a map of function to bitcode (as a string).
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
         * @refcode
         * @errtest
         *  Invalid file formats
         * @errmsg
         *  Runtime errors about invalid indexes
         * @author Jacob Peddicord
         * @creation April 7, 2011
         * @modlog
         *  - April  8, 2011 - Jacob - Load from Resource instead of a stream
         *  - April 12, 2011 -  Mark - Things are now trimmed.
         * @codestandard Mark
         * @teststandard Andrew
         */
        private Instructions()
        {
            this.instructions = new Dictionary<string, Dictionary<string, string>>();
            
            // fill the instruction mapping with data from the file
            foreach (string line in Properties.Resources.instructions.Split('\n'))
            {
                if (line.Trim().Length > 0)
                {
                    string[] parts = line.Split(' ');
                    if (!this.instructions.ContainsKey(parts[0].Trim()))
                    {
                        this.instructions[parts[0].Trim()] = new Dictionary<string, string>();
                    }
                    this.instructions[parts[0].Trim()][parts[1].Trim()] = parts[2].Trim();
                }
            }
        }

        /**
         * Get the singleton instance of Instructions.
         *
         * @return the Instructions instance
         *
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation April 8, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public static Instructions GetInstance()
        {
            if (Instructions.instance == null)
            {
                Instructions.instance = new Instructions();
            }
            
            return Instructions.instance;
        }

        /**
         * Check to see if an instruction group and function pair are valid.
         *
         * @param instrGroup instruction group to check
         * @param function function name in group to check
         * @return whether the function is valid in the given group
         *
         * @refcode OP
         * @errtest
         *  Various cases of instruction/group combinations
         * @errmsg
         *  None
         * @author Jacob Peddicord
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public bool IsInstruction(string instrGroup, string function)
        {
            return this.instructions.ContainsKey(instrGroup.ToUpper())
                && this.instructions[instrGroup.ToUpper()].ContainsKey(function.ToUpper());
        }

        /**
         * Check to see if an instruction group exists.
         *
         * @param instrGroup instruction group to check
         * @return whether the group exists
         *
         * @refcode OP
         * @errtest
         *  Different instruction groups
         * @errmsg
         *  None
         * @author Jacob Peddicord
         * @creation April 9, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public bool IsGroup(string instrGroup)
        {
            return this.instructions.ContainsKey(instrGroup.ToUpper());
        }

        /**
         * Get a bytecode string corresponding to the provided instruction
         * group and function. The returned string is guaranteed to be
         * 5 characters (bits) long.
         * 
         * @param instrGroup Instruction group of function
         * @param function Function of group to get bytecode for
         * @return string of five '0' and '1' characters
         *
         * @refcode OP
         * @errtest
         *  Bytecode of several operations
         * @errmsg
         *  None
         * @author Jacob Peddicord
         * @creation April 7, 2011
         * @modlog
         *  - April 8, 2011 - Jacob - throw proper exceptions on invalid group/function pairs
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
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

        /**
         * The reverse of GetBytecodeString. Finds the matching category and
         * function for the given bits.
         *
         * @param bits 5 bits to perform a reverse lookup for
         * @param category Returned function category
         * @param function Returned function
         *
         * @refcode OP
         * @errtest
         *  N/A
         * @errmsg
         *  None
         * @author Jacob Peddicord
         * @creation May 19, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public void ReverseLookup(string bits, out string category, out string function)
        {
            category = "";
            function = "";
            foreach (var cat in this.instructions)
            {
                foreach (var func in cat.Value)
                {
                    if (func.Value == bits)
                    {
                        category = cat.Key;
                        function = func.Key;
                        break;
                    }
                }
            }
        }
    }
}
