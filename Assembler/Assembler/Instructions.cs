using System;
using System.IO;
using System.Collections.Generic;

namespace Assembler
{
	public class InstructionException : ApplicationException {}
	
	/**
	 * Opcode translation mechanism. Documentation needs to be written.
	 */
    public class Instructions
    {
        private Dictionary<string, Dictionary<string, string>> instructions;
		
		/**
		 * Create a translation object. Loads instruction translation data
		 * from the given file in filename. The file should have lines of
		 * the format:
		 *     GROUP FUNCTION BITS
		 * where GROUP is the instruction group, FUNCTION is the named
		 * function, and BITS is the corresponding bytecode (as a string of
		 * 0's and 1's) to the function.
		 * 
		 * @param filename file to load data from
		 */
        public Instructions (string filename)
        {
			this.instructions = new Dictionary<string, Dictionary<string, string>>();
			
			/** Responsible for reading the file specified. */
            TextReader tr = new StreamReader(filename);

			// fill the instruction mapping with data from the file
			/** Holds the current line in the instruction file. */
            string line;
            while ((line = tr.ReadLine()) != null)
			{
				/** Contains the current line split by spaces. Always length 3. */
                string[] parts = line.Split(' ');
				this.instructions[parts[0]] = new Dictionary<string, string>();
				this.instructions[parts[0]][parts[1]] = parts[2];
            }
			
			// close up
			tr.Close();
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
		public string GetBytecodeString (string instrGroup, string function)
		{
			if (!this.instructions.ContainsKey(instrGroup))
			{
				//throw InstructionException("\"" + instrGroup + "\" is not a valid group.");
			}
			
			if (!this.instructions[instrGroup].ContainsKey(function))
			{
				//throw InstructionException("\"" + function + "\" is not a valid function for " + instrGroup);
			}
			
			return this.instructions[instrGroup][function];
		}

    }
}
