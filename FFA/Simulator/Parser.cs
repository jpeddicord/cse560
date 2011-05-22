using System;
using System.IO;
using System.Collections.Generic;
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
                            // any errors in the header record can cause the rest of the parsing to fail
                            try
                            {
                                ParseHeader(parts);
                            }
                            catch (Assembler.ErrorException ex)
                            {
                                Console.WriteLine(String.Format("Header parse error:\n    {0}", ex));
                                throw new Assembler.ErrorException(ErrCat.Fatal, 2);
                            }
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
                            "Parsing error on line {0}:\n    {1}",
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
         *  Valid header format
         * @errmsg
         *  Any errors as a result of an incorrect or improper header
         * @author Jacob Peddicord
         * @creation May 20, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void ParseHeader(string[] parts)
        {
            var errors = new List<Assembler.ErrorException>();
            // error checking
            if (parts.Length != 13)
            {
                throw new Assembler.ErrorException(ErrCat.Fatal, 3);
            }
            // check module name length
            if (parts[1].Length < 2)
            {
                errors.Add(new Assembler.ErrorException(ErrCat.Serious, 14));
            }
            // check load address length
            if (parts[2].Length != 4)
            {
                throw new Assembler.ErrorException(ErrCat.Fatal, 4);
            }
            // check exec start length
            if (parts[3].Length != 4)
            {
                throw new Assembler.ErrorException(ErrCat.Fatal, 5);
            }
            // check total length field length
            if (parts[4].Length != 4)
            {
                // TODO: FATAL
                throw new Assembler.ErrorException(ErrCat.Fatal, 6);
            }
            // check that a date exists
            if (parts[5].Length == 0)
            {
                errors.Add(new Assembler.ErrorException(ErrCat.Serious, 18));
            }// TODO: date field may not have colons...
            // check record number length
            if (parts[8].Length != 4)
            {
                errors.Add(new Assembler.ErrorException(ErrCat.Serious, 19));
            }
            // check text record count matches the total length
            if (parts[9] != parts[4])
            {
                errors.Add(new Assembler.ErrorException(ErrCat.Serious, 20));
            }
            // check FFA-LLM
            if (parts[10] != "FFA-LLM")
            {
                errors.Add(new Assembler.ErrorException(ErrCat.Serious, 21));
            }
            // check program name matches
            if (parts[11] != parts[1])
            {
                errors.Add(new Assembler.ErrorException(ErrCat.Warning, 3));
            }

            // set the program name
            this.programName = parts[1];

            // try to set the load address
            try
            {
                this.loadAddress = Convert.ToInt32(parts[2], 16);
            }
            catch (Exception)
            {
                throw new Assembler.ErrorException(ErrCat.Fatal, 4);
            }

            // try to set the execution start
            try
            {
                this.executionStart = Convert.ToInt32(parts[3], 16);
                Runtime.GetInstance().LC = this.executionStart;
            }
            catch (Exception)
            {
                throw new Assembler.ErrorException(ErrCat.Fatal, 5);
            }

            // try to set the total length
            try
            {
                this.totalLength = Convert.ToInt32(parts[4], 16);
            }
            catch (Exception)
            {
                throw new Assembler.ErrorException(ErrCat.Fatal, 6);
            }

            // try to set the total number of records
            try
            {
                this.totalRecords = Convert.ToInt32(parts[8], 16);
            }
            catch (Exception)
            {
                errors.Add(new Assembler.ErrorException(ErrCat.Serious, 19));
            }

            // print out any errors
            foreach (var err in errors)
            {
                Console.WriteLine(err);
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

            // set the memory
            var mem = Memory.GetInstance();
            mem.SetWord(Convert.ToInt32(parts[1], 16),
                        Convert.ToInt32(parts[2], 16));

            // validate the program name
            if (parts[3] != this.programName)
            {
                throw new Assembler.ErrorException(ErrCat.Warning, 3);
            }
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
                throw new Assembler.ErrorException(ErrCat.Warning, 3);
            }
        }
    }
}

