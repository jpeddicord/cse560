using System;
using System.IO;
using ErrCat = Assembler.Errors.Category;

namespace Simulator
{
    /**
     * The loader file parser. Reads the data into simulated memory.
     */
    public class Parser
    {
        /**
         * The stored program name.
         */
        private string programName = "";

        /**
         * The loading address.
         */
        private int loadAddress = 0;

        /**
         * The start of execution.
         */
        private int executionStart = 0;

        /**
         * The total length of the program.
         */
        private int totalLength = 0;

        /**
         * The total number of records in the loader file.
         */
        private int totalRecords = 0;

        /**
         * Parse the given file, and load its contents into memory. If any
         * errors are found in the file, they will be displayed, but unless
         * they are fatal parsing will continue.
         *
         * @param filename The filename to parse and load
         * @return true if successful, false otherwise
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public bool ParseFile(string filename)
        {
            var file = new StreamReader(filename);

            string line;
            int lineNum = 1;
            bool reachedEnd = false;
            bool success = true;
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    if (line.Length > 0)
                    {
                        // don't allow garbage after the end
                        if (reachedEnd)
                        {
                            throw new Assembler.ErrorException(ErrCat.Serious, 9);
                        }

                        // get the parts of the string
                        string[] parts = line.Split(':');
    
                        // header record found
                        if (parts[0] == "H")
                        {
                            ParseHeader(parts);
                        }
                        // text record found
                        else if (parts[0] == "T")
                        {
                            ParseText(parts);
                        }
                        // end record found
                        else if (parts[0] == "E")
                        {
                            ParseEnd(parts);
                        }
                        else
                        {
                            // unknown record
                            throw new Assembler.ErrorException(ErrCat.Serious, 6);
                        }
                    }
                }
                catch (Assembler.ErrorException ex)
                {
                    Console.WriteLine(String.Format(
                            "Parsing error on line {0}:\n{1}",
                            lineNum, ex));
                    if (ex.err.category == ErrCat.Fatal)
                    {
                        Console.WriteLine("Aborting due to fatal errors.");
                        success = false;
                        break;
                    }
                }

                lineNum++;
            }

            file.Close();

            return success;
        }

        /**
         * Parse a header record line. Will set local members as appropriate.
         *
         * @param parts The line to parse, split by colon
         * @refcode LM1
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void ParseHeader(string[] parts)
        {
            if (parts.Length != 13)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 1);
            }
            for (int i = 0; i < 13; i++)
            {

            }
        }

        /**
         * Parse a text record and load its contents into the relevant address.
         *
         * @param parts The line to parse, split by colon
         * @refcode LM2
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void ParseText(string[] parts)
        {
            // check the overall record length
            if (parts.Length != 4)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 2);
            }
            // validate the memory address
            if (parts[1].Length != 4)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 7);
            }
            // validate the word
            if (parts[2].Length != 4)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 8);
            }
            // validate the program name
            if (parts[3] != this.programName)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 4);
            }

            // set the memory
            var mem = Memory.GetInstance();
            mem.SetWord(Convert.ToInt32(parts[1], 16),
                        Convert.ToInt32(parts[2], 16));
        }

        /**
         * Parse the end record.
         *
         * @param parts The line to parse, split by colon
         * @refcode LM3
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void ParseEnd(string[] parts)
        {
            if (parts.Length != 2)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 3);
            }
            if (parts[1] != this.programName)
            {
                throw new Assembler.ErrorException(ErrCat.Serious, 4);
            }
        }
    }
}

