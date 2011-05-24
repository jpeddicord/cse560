using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Error = Assembler.ErrorException;
using ErrCat = Assembler.Errors.Category;

namespace Linker
{
    /**
     * Parser for the linker. It reads in object files that have ben output by the
     * FFA Assembler and prepares them to be linked by the Linker.
     */
    class Parser
    {
        /**
         * The file currently being parsed.
         */
        private int fileNum;

        /**
         * The address to be used any time the Linker calculated address is needed.
         */
        private int address;

        /**
         * The symbol table used by the Linker to keep program names and entries in order.
         */
        private SymbolTable symb;

        /**
         * Allows the Parser to print error messages.
         */
        private Assembler.Errors errPrinter = Assembler.Errors.GetInstance();

        /**
         * Parses a single FFA object file. This file is the output of the FFA assembler.
         * 
         * @param filename the filename of the file to parse
         * @param Module the module that represents the file that is read in
         * @param symb the linker symbol table
         * @param fileNum how many files have been parsed before this file
         * @param startAddress the linker calculated start address of this module
         * 
         * @refcode
         *  OB1, OB2, OB3, OB4, OB5
         * @errtest
         * @errmsg
         *  ES.01, ES.35, ES.55
         * @author Mark Mathis
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void ParseFile(string filename, out Module mod, SymbolTable symb, int fileNum, ref int startAddress)
        {
            // instantiate mod right away since it must be instantiated before
            // this method exits
            mod = new Module();

            // keep track of which file we are parsing
            this.fileNum = fileNum;

            // keep track of which linker computed address we are on
            this.address = startAddress;

            StreamReader file = null;
            try
            {
                file = new StreamReader(filename);
            }
            catch (Exception)
            {
                // error, input file cannot be opened
                throw new Error(ErrCat.Serious, 55);
            }

            this.symb = symb;
            int lineNum = 1;
            string rec;
            bool endReached = false;
            while ((rec = file.ReadLine()) != null)
            {
                try
                {
                    // ignore empty lines
                    if (rec.Trim().Length == 0)
                    {
                        continue;
                    }

                    if (endReached)
                    {
                        // error, input present after the end record
                        throw new Error(ErrCat.Serious, 1);
                    }
    
                    // process different types of records
                    if (rec[0] == 'H')
                    {
                        ParseHeader(rec, out mod);
                    }
                    else if (rec[0] == 'L')
                    {
                        ParseLink(rec, mod);
                    }
                    else if (rec[0] == 'T')
                    {
                        ParseText(rec, mod);
                    }
                    else if (rec[0] == 'M')
                    {
                        ParseModify(rec, mod);
                    }
                    else if (rec[0] == 'E')
                    {
                        endReached = ParseEnd(rec, mod);
                    }                    
                    else
                    {
                        // invalid record or garbage data
                        throw new Error(ErrCat.Serious, 33);
                    }
                }
                catch (Error ex)
                {
                    Console.WriteLine(String.Format(
                            "Parsing error on line {0} of {1}:\n{2}",
                            lineNum, filename, ex));
                    if (ex.err.category == ErrCat.Fatal)
                    {
                        throw ex;
                    }
                }

                lineNum++;
            }
            startAddress = address;
        }

        /**
         * Parses a single FFA object file header record. This should be the first line of
         * the assembler output.
         * 
         * @param rec the header record to be parsed
         * @param mod the module this header record will be a part of
         * 
         * @refcode
         *  OB1
         * @errtest
         * @errmsg
         *  EW.01, EW.02, EW.03, EW.04, EW.05, EW.06, EW.07, EW.08, EW.09, 
         *  ES.02, ES.03, ES.04, ES.05, ES.06, ES.07, ES.08, EF.01, EF.02, 
         *  EF.03, EF.04, EF.05, EF.06, EF.07, EF.08
         * @author Mark Mathis
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void ParseHeader(string rec, out Module mod)
        {
            string[] field = rec.Split(':');
            Header head = new Header();

            // check that the header record contains the correct number of fields
            if (field.Length != 13)
            {
                // error, wrong number of fields in header record
                throw new Error(ErrCat.Fatal, 1);
            }

            // check that program name is valid
            string prgmName = field[1];

            /* Regular expression used to determine if all characters in the token are
             * letters or numbers. */
            Regex alphaNumeric = new Regex(@"[^0-9a-zA-Z]");

            if (2 <= prgmName.Length && prgmName.Length <= 32)
            {
                if (!(!alphaNumeric.IsMatch(prgmName) && char.IsLetter(prgmName[0])))
                {
                    // program name is not a valid label
                    errPrinter.PrintError(ErrCat.Serious, 2);
                }
            }
            else
            {
                // program name is not the right length
                errPrinter.PrintError(ErrCat.Serious, 2);
            }

            // add program name to the linking header record
            head.ProgramName = prgmName;


            // get the Assembler assigned program load address
            // see if it is properly formatted
            string assLoad = field[2].ToUpper();

            // check length, should be 4 digit hex number
            if (assLoad.Length != 4)
            {
                // error, wrong length
                errPrinter.PrintError(ErrCat.Serious, 3);
            }

            //check that it is valid hex
            int assLoadVal = 0;
            try
            {
                assLoadVal = Convert.ToInt32(assLoad, 16);
            }
            catch (FormatException)
            {
                // error, not valid hex
                throw new Error(ErrCat.Fatal, 2);
            }
            
            // check that it is in the correct range
            if (assLoadVal < 0 || assLoadVal > 1023)
            {
                // error, must be between 0 and 1023
                throw new Error(ErrCat.Fatal, 3);
            }

            // add assembler load address to linking header record
            head.AssemblerLoadAddress = assLoadVal;


            // get the module length
            string modLen = field[3].ToUpper();

            // check that it is a 4 digit hex
            if (modLen.Length != 4)
            {
                // error, wrong length
                errPrinter.PrintError(ErrCat.Serious, 4);
            }

            // check that it is valid hex
            int modLenVal = 0;
            try
            {
                modLenVal = Convert.ToInt32(modLen, 16);
            }
            catch (FormatException)
            {
                // error, not valid hex
                throw new Error(ErrCat.Fatal, 4);
            }

            // check that it is in the correct range
            if (modLenVal < 0 || modLenVal > 1024)
            {
                // error, must be between 0 and 1024
                throw new Error(ErrCat.Fatal, 5);
            }

            // add module length to linking header record
            head.ModuleLength = modLenVal;


            // get the execution start address
            string execAdd = field[4].ToUpper();

            // check length, should be 4 digit hex number
            if (execAdd.Length != 4)
            {
                // error, wrong length
                errPrinter.PrintError(ErrCat.Serious, 5);
            }

            //check that it is valid hex
            int execAddVal = 0;
            try
            {
                execAddVal = Convert.ToInt32(execAdd, 16);
            }
            catch (FormatException)
            {
                // error, not valid hex
                throw new Error(ErrCat.Fatal, 6);
            }

            // check that it is in the correct range
            if (execAddVal < 0 || execAddVal > 1023)
            {
                // error, must be between 0 and 1023
                throw new Error(ErrCat.Fatal, 7);
            }

            // add execution start address to linking header record
            head.ExecutionStartAddress = execAddVal;

            // get date and time of assembly
            string dateAndTime = field[5];

            // check that it is the proper length
            if (dateAndTime.Length != 16)
            {
                // error?, not the proper length
                errPrinter.PrintError(ErrCat.Warning, 1);
            }


            // get version number of assembler that assembled this header record
            string verNum = field[6].ToUpper();

            // check that it's the proper length, 4 digits
            if (verNum.Length != 4)
            {
                // error?, not the proper length
                errPrinter.PrintError(ErrCat.Warning, 2);
            }


            // get the total number of records in the object file
            string totalRec = field[7].ToUpper();

            // check length, should be 4 digit hex number
            if (totalRec.Length != 4)
            {
                // error, wrong length
                errPrinter.PrintError(ErrCat.Warning, 3);
            }

            //check that it is valid hex
            int totalRecVal = 0;
            try
            {
                totalRecVal = Convert.ToInt32(totalRec, 16);
            }
            catch (FormatException)
            {
                // error, not valid hex
                errPrinter.PrintError(ErrCat.Warning, 4);
            }

            // add the total number of records to the linker header record
            head.TotalRecords = totalRecVal;


            // get the number of linking records in the object file
            string linkRec = field[8].ToUpper();

            // check length, should be 4 digit hex number
            if (linkRec.Length != 4)
            {
                // error, wrong length
                errPrinter.PrintError(ErrCat.Warning, 5);
            }


            // check that it is valid hex
            int linkRecVal = 0;
            try
            {
                linkRecVal = Convert.ToInt32(linkRec, 16);
            }
            catch (FormatException)
            {
                // error, not valid hex
                errPrinter.PrintError(ErrCat.Warning, 6);
            }

            // add the total number of linking records to the linker header record
            head.TotalLinkingRecords = linkRecVal;


            // get the number of text records in the object file
            string textRec = field[9].ToUpper();

            // check length, should be 4 digit hex number
            if (textRec.Length != 4)
            {
                // error, wrong length
                errPrinter.PrintError(ErrCat.Serious, 6);
            }

            // check that it is valid hex
            int textRecVal = 0;
            try
            {
                textRecVal = Convert.ToInt32(textRec, 16);
            }
            catch (FormatException)
            {
                // error, not valid hex
                errPrinter.PrintError(ErrCat.Serious, 7);
            }

            // check that the number is in the proper range
            if (textRecVal < 0 || textRecVal > modLenVal)
            {
                // error, can only have 0 to module length text records
                throw new Error(ErrCat.Fatal, 8);
            }

            // add the total number of text records to the linker header record
            head.TotalTextRecords = textRecVal;


            // get the number of modify records in the object file
            string modRec = field[10].ToUpper();

            // check length, should be 4 digit hex number
            if (modRec.Length != 4)
            {
                // error, wrong length
                errPrinter.PrintError(ErrCat.Warning, 7);
            }

            // check that it is valid hex
            int modRecVal = 0;
            try
            {
                modRecVal = Convert.ToInt32(modRec, 16);
            }
            catch (FormatException)
            {
                // error, not valid hex
                errPrinter.PrintError(ErrCat.Warning, 8);
            }

            // add the total number of modify records to the linker header record
            head.TotalModifyRecords = modRecVal;

            
            // check that the assembler name is correct
            if (field[11].ToUpper() != "FFA-ASM")
            {
                // error? wrong assembler?
                errPrinter.PrintError(ErrCat.Warning, 9);
            }


            // check that the program name at the end of
            // the record is the same as at the beginning
            if (field[12] != prgmName)
            {
                // error, program names do not match
                errPrinter.PrintError(ErrCat.Serious, 8);
            }

            if (fileNum == 0)
            {
                head.LinkerLoadAddress = 0;
                address = modLenVal;
            }
            else
            {
                head.LinkerLoadAddress = address;
                address += modLenVal;
            }

            mod = new Module(head);
            symb.AddSymbol(mod.ModuleName, Assembler.Usage.PRGMNAME, mod.RelocateValue);
        }

        /**
         * Parses a single linking record.
         * 
         * @param rec the linking record to be parsed
         * @param mod the module this linking record will be a part of
         * 
         * @refcode
         *  OB2
         * @errtest
         * @errmsg
         *  EW.10, ES.09, ES.10, ES.11, ES.12, ES.13, ES.14, ES.15
         * @author Mark Mathis
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void ParseLink(string rec, Module mod)
        {
            string[] field = rec.Split(':');
            Linking linkRecord = new Linking();

            // check if record has the correct number of fields
            if (field.Length != 4)
            {
                // error, wrong number of fields
                errPrinter.PrintError(ErrCat.Serious, 9);
            }

            // check that Entry name is valid
            string entry = field[1];

            /* Regular expression used to determine if all characters in the token are
             * letters or numbers. */
            Regex alphaNumeric = new Regex(@"[^0-9a-zA-Z]");

            if (2 <= entry.Length && entry.Length <= 32)
            {
                if (!(!alphaNumeric.IsMatch(entry) && char.IsLetter(entry[0])))
                {
                    // entry name is not a valid label
                    errPrinter.PrintError(ErrCat.Serious, 10);
                }
            }
            else
            {
                // entry name is not the right length
                errPrinter.PrintError(ErrCat.Serious, 10);
            }

            // add entry name to linker linking record
            linkRecord.EntryName = entry;


            // get the location within the program
            string prgmLoc = field[2].ToUpper();

            // check length, should be 4 digit hex number
            if (prgmLoc.Length != 4)
            {
                // error, wrong length
                errPrinter.PrintError(ErrCat.Warning, 10);
            }

            //check that it is valid hex
            int prgmLocVal = 0;
            try
            {
                prgmLocVal = Convert.ToInt32(prgmLoc, 16);
            }
            catch (FormatException)
            {
                // error, not valid hex
                errPrinter.PrintError(ErrCat.Serious, 11);
            }

            // check that it is in the correct range
            if (prgmLocVal < 0 || prgmLocVal > 1023)
            {
                // error, must be between 0 and 1023
                errPrinter.PrintError(ErrCat.Serious, 12);
            }

            // add location to linking header record
            linkRecord.Location = prgmLocVal;


            // make sure the program name is properly formatted
            // and in the symbol table
            string pgmName = field[3];

            if (2 <= pgmName.Length && pgmName.Length <= 32)
            {
                if (!(!alphaNumeric.IsMatch(pgmName) && char.IsLetter(pgmName[0])))
                {
                    // program name is not a valid label
                    throw new Error(ErrCat.Serious, 13);
                }
            }
            else
            {
                // program name is not the right length
                throw new Error(ErrCat.Serious, 13);
            }

            // check that program name is in symbol table
            if (mod.ModuleName != pgmName)
            {
                // error, program name at end of linking record must match program name
                // of program being parsed
                throw new Error(ErrCat.Serious, 14);
            }

            if (!symb.ContainsSymbol(pgmName))
            {
                // error, program name not in the symbol table
                errPrinter.PrintError(ErrCat.Serious, 15);
            }

            // add entry to symbol table
            mod.AddRecord(linkRecord);
        }

        /**
         * Parses a single text record.
         * 
         * @param rec the text record to be parsed
         * @param mod the module this text record will be a part of
         * 
         * @refcode
         *  OB3
         * @errtest
         * @errmsg
         *  EW.11, EW.12, EW.13, EW.14, ES.16, ES.17, ES.18, ES.19, ES.20, ES.21, ES.22, ES.23
         * @author Mark Mathis
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void ParseText(string rec, Module mod)
        {
            string[] field = rec.Split(':');
            Text textRecord = new Text();

            // check that the record has the correct number of fields
            if (field.Length != 6)
            {
                // error, wrong number of fields
                throw new Error(ErrCat.Serious, 16);
            }

            // check program assigned location
            string progLoc = field[1].ToUpper();

            // check length, should be 4 digit hex number
            if (progLoc.Length != 4)
            {
                // error, wrong length
                errPrinter.PrintError(ErrCat.Warning, 11);
            }

            //check that it is valid hex
            int progLocVal = 0;
            try
            {
                progLocVal = Convert.ToInt32(progLoc, 16);
            }
            catch (FormatException)
            {
                // error, not valid hex
                errPrinter.PrintError(ErrCat.Serious, 17);
            }

            // check that it is in the correct range
            if (progLocVal < 0 || progLocVal > 1023)
            {
                // error, must be between 0 and 1023
                errPrinter.PrintError(ErrCat.Serious, 18);
            }

            // add program location to the linker text record
            textRecord.Location = progLocVal;


            // check the hex code of the instruction
            string hexCode = field[2].ToUpper();

            // check length, should be 4 digit hex number
            if (hexCode.Length < 4)
            {
                // error, too short
                errPrinter.PrintError(ErrCat.Warning, 12);
            }
            else if (hexCode.Length > 4)
            {
                // error, too long
                errPrinter.PrintError(ErrCat.Serious, 19);
                hexCode = "8000";
            }

            //check that it is valid hex
            int hexCodeVal = 0;
            try
            {
                hexCodeVal = Convert.ToInt32(hexCode, 16);
            }
            catch (FormatException)
            {
                // error, not valid hex
                errPrinter.PrintError(ErrCat.Serious, 20);
                hexCodeVal = Convert.ToInt32("8000", 16);
            }

            // add hex code of instruction to linker text record
            textRecord.Word = hexCodeVal;

            // check address status flag
            string addStatus = field[3].ToUpper();

            // check length of flag, should be 1
            if (addStatus.Length != 1)
            {
                // error wrong length
                errPrinter.PrintError(ErrCat.Serious, 21);
                addStatus = "A";
            }

            // check that the flag is a valid value
            if (!(addStatus[0] == 'A' || addStatus[0] == 'R' || addStatus[0] == 'M'))
            {
                // error, invalid value for status flag
                errPrinter.PrintError(ErrCat.Serious, 21);
                addStatus = "A";
            }

            // add status flag to linker text record
            textRecord.Flag = addStatus[0];


            // check number of M adjustments needed
            string mAdj = field[4].ToUpper();

            // check that it is the correct length, should be 1
            if (mAdj.Length != 1)
            {
                // error wrong length
                errPrinter.PrintError(ErrCat.Warning, 13);
                mAdj = "1";
            }

            // check that it is valid hex
            int mAdjVal = 0;
            try
            {
                mAdjVal = Convert.ToInt32(mAdj, 16);
            }
            catch (FormatException)
            {
                // error, not valid hex
                errPrinter.PrintError(ErrCat.Warning, 14);
                mAdjVal = 1;
            }
            // if it's valid then it should be in the correct range
            // since one hex digit can only be 0-15
            
            // add number of adjustments to the linker text record
            textRecord.Adjustments = mAdjVal;


            // check the program name
            string prgmName = field[5];

            // check that the program name is in the symbol table
            if (mod.ModuleName != prgmName)
            {
                // error, program name at end of text record must match
                // program name of program being parsed
                throw new Error(ErrCat.Serious, 22);
            }

            if (!symb.ContainsSymbol(prgmName))
            {
                // error, program name is not in symbol table
                throw new Error(ErrCat.Serious, 23);
            }

            
            mod.AddRecord(textRecord);
        }

        /**
         * Parses a single modify record.
         * 
         * @param rec the modify record to be parsed
         * @param mod the module this text record will be a part of
         * 
         * @refcode
         *  OB4
         * @errtest
         * @errmsg
         *  EW.15, EW.16, EW.17, ES.24, ES.25, ES.26, ES.27, ES.28, ES.29, ES.30, ES.31, ES.32
         * @author Mark Mathis
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public void ParseModify(string rec, Module mod)
        {
            string[] field = rec.Split(':');
            Modify modRecord = new Modify();

            // check the location
            string loc = field[1];

            // check the length of the location
            if (loc.Length != 4)
            {
                // error, incorrect length
                errPrinter.PrintError(ErrCat.Warning, 15);
            }

            // check that it is a valid hex string
            int locVal = 0;
            try
            {
                locVal = Convert.ToInt32(loc, 16);
            }
            catch (FormatException)
            {
                // error, not valid hex
                throw new Error(ErrCat.Serious, 24);
            }

            // check that the location value is in the proper range
            if (locVal < 0 || locVal > 1023)
            {
                // error, location invalid
                throw new Error(ErrCat.Serious, 25);
            }
            
            // add  location to the linker modification record
            modRecord.Location = locVal;

            
            // check the hex code of the word
            string hex = field[2];

            // check the length of the hex code
            if (hex.Length < 4)
            {
                // error, too short
                errPrinter.PrintError(ErrCat.Warning, 16);
            }
            else if (hex.Length > 4)
            {
                // error, too long
                throw new Error(ErrCat.Serious, 26);
            }

            // check that it is a valid hex string
            int hexVal = 0;
            try
            {
                hexVal = Convert.ToInt32(hex, 16);
            }
            catch (FormatException)
            {
                // error, not valid hex
                throw new Error(ErrCat.Serious, 27);
            }

            // add the hex code of the word to be modified to the linker modification record
            modRecord.Word = hexVal;


            /* Regular expression used to determine if all characters in the token are
             * letters or numbers. */
            Regex alphaNumeric = new Regex(@"[^0-9a-zA-Z]");

            // go through the modifications and make sure they are formatted correctly
            string sign = field[3];

            // check that sign is a + or -
            if (!(sign == "+" || sign == "-"))
            {
                // error, sign must be a + or -
                throw new Error(ErrCat.Serious, 28);
            }

            int i = 4;
            while (sign == "+" || sign == "-")
            {
                string label = field[i++];

                // check that label is valid label
                if (2 <= label.Length && label.Length <= 32)
                {
                    if (!(!alphaNumeric.IsMatch(label) && char.IsLetter(label[0])))
                    {
                        // label is not a valid label
                        errPrinter.PrintError(ErrCat.Serious, 29);
                    }
                }
                else
                {
                    // label is not the right length
                    errPrinter.PrintError(ErrCat.Serious, 29);
                }

                // add adjustments to the linker modification record
                modRecord.AddAdjustments(sign, label);

                // get next sign
                try
                {
                    sign = field[i++];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new Error(ErrCat.Serious, 32);
                }
            }
            // reached end of modification record, or there was an error
            // in the format of modifications

            // check that label is valid label
            if (2 <= sign.Length && sign.Length <= 32)
            {
                if (!(!alphaNumeric.IsMatch(sign) && char.IsLetter(sign[0])))
                {
                    // label is not a valid label
                    errPrinter.PrintError(ErrCat.Serious, 29);
                }
            }
            else
            {
                // label is not the right length
                errPrinter.PrintError(ErrCat.Serious, 29);
            }

            // check if the label is in the symbol table
            // if it is, it's probably not an error
            // if it isn't, it could be an error: say something about it
            if (mod.ModuleName != sign)
            {
                // error, program name at the end of modification record must match
                // program name of program being parsed
                throw new Error(ErrCat.Serious, 30);
            }

            if (!symb.ContainsSymbol(sign))
            {
                // error, something isn't in the symbol table
                throw new Error(ErrCat.Serious, 31);
            }

            //check to see that the modify record doesn't have too many adjustments
            if (modRecord.Adjustments.Count > 15)
            {
                // error, can only have 15 adjustments in a modify record
                errPrinter.PrintError(ErrCat.Warning, 17);
            }

            // add modification record to module
            mod.AddRecord(modRecord);
        }

        /**
         * Parses a single end record.
         * 
         * @param rec the end record to be parsed
         * @param mod the module this end record will be a part of
         * 
         * @return true if the end record is successfully parsed
         *         false if parsing the end record is unsuccessful
         * 
         * @refcode
         *  OB5
         * @errtest
         * @errmsg
         *  EW.18, ES.34, ES.35, ES.36
         * @author Mark Mathis
         * @creation May 19, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public bool ParseEnd(string rec, Module mod)
        {
            string[] field = rec.Split(':');

            // check that the record has the correct number of fields
            if (field.Length != 2)
            {
                // record has the wrong number of fields
                errPrinter.PrintError(ErrCat.Warning, 18);
            }

            // check that program name is valid
            string prgmName = field[1];

            /* Regular expression used to determine if all characters in the token are
             * letters or numbers. */
            Regex alphaNumeric = new Regex(@"[^0-9a-zA-Z]");

            if (2 <= prgmName.Length && prgmName.Length <= 32)
            {
                if (!(!alphaNumeric.IsMatch(prgmName) && char.IsLetter(prgmName[0])))
                {
                    // program name is not a valid label
                    errPrinter.PrintError(ErrCat.Serious, 34);
                }
            }
            else
            {
                // program name is not the right length
                errPrinter.PrintError(ErrCat.Serious, 34);
            }


            // check that this program name is in the symbol table.
            if (mod.ModuleName != prgmName)
            {
                // error, must have the same program name at the end of the record as the
                // module currently being parsed
                errPrinter.PrintError(ErrCat.Serious, 35);
            }

            if (!symb.ContainsSymbol(prgmName))
            {
                // error, program name not in symbol table
                errPrinter.PrintError(ErrCat.Serious, 36);
            }

            // since end record has been reached there should be no more input
            // check to see if the correct number of records has been found
            CheckRecords(mod);

            // add linking records to the symbol table since all the data we
            // need should be there at this point
            DoLinkingRecords(mod);

            return true;
        }

        /**
         * Checks to see if the number of records actually parsed matches the number
         * of records reported by the header record.
         * 
         * @param mod the module whose records should be checked
         * 
         * @refcode
         *  OB1, OB2, OB3, OB4, OB5
         * @errtest
         * @errmsg
         *  EW.19, EW.20, ES.37
         * @author Mark Mathis
         * @creation May 22, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private void CheckRecords(Module mod)
        {
            if (mod.HeaderRecord.TotalTextRecords != mod.TotalTextRecords)
            {
                // error, wrong number of text records
                errPrinter.PrintError(ErrCat.Serious, 37);
            }

            if (mod.HeaderRecord.TotalLinkingRecords != mod.TotalLinkingRecords)
            {
                // error, wrong number of linking records
                errPrinter.PrintError(ErrCat.Warning, 19);
            }

            if (mod.HeaderRecord.TotalModifyRecords != mod.TotalModifyRecords)
            {
                // error, wrong number of modify records
                errPrinter.PrintError(ErrCat.Warning, 20);
            }
        }

        /**
         * Adds all the entries found in the linking records to the symbol table.
         * 
         * @param mod the module whose linking records should be added to the symbol table
         * 
         * @refcode
         *  OB2
         * @errtest
         * @errmsg
         *  ES.38, ES.39
         * @author Mark Mathis
         * @creation May 22, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        private void DoLinkingRecords(Module mod)
        {
            var lRec = mod.LinkingRecords;
            foreach (var rec in lRec)
            {
                try
                {
                    int lValue = mod.GetTextRecord(lRec[rec.Key].Location).Location;
                    symb.AddSymbol(lRec[rec.Key].EntryName, Assembler.Usage.ENTRY, lValue);
                }
                catch (NullReferenceException) 
                {
                    errPrinter.PrintError(ErrCat.Serious, 39);
                }
                catch (Assembler.SymbolException)
                {
                    // error, symbol already in symbol table
                    errPrinter.PrintError(ErrCat.Serious, 38);
                }
            }
        }
    }
}
