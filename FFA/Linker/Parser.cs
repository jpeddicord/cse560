using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using ErrCat = Assembler.Errors.Category;

namespace Linker
{
    class Parser
    {
        private int fileNum;
        private int address;

        public void ParseFile(string filename, int fileNum, ref int startAddress)
        {
            this.fileNum = fileNum;
            this.address = startAddress;
            var file = new StreamReader(filename);

            int lineNum = 0;
            string rec;
            while ((rec = file.ReadLine()) != null)
            {
                try
                {
                    // ignore empty lines
                    if (rec.Trim().Length == 0)
                    {
                        continue;
                    }
    
                    // process different types of records
                    if (rec[0] == 'H')
                    {
                        ParseHeader(rec);
                    }
                    else if (rec[0] == 'L')
                    {
                        ParseLink(rec);
                    }
                    else if (rec[0] == 'T')
                    {
                        ParseText(rec);
                    }
                    else if (rec[0] == 'M')
                    {
                        ParseModify(rec);
                    }
                    else if (rec[0] == 'E')
                    {
                        ParseEnd(rec);
                    }
                    // invalid record or garbage data
                    else
                    {
                        throw new Assembler.ErrorException(ErrCat.Serious, 1); // FIXME: code is wrong
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
                        break;
                    }
                }

                lineNum++;
            }
            startAddress = address;
        }

        public void ParseHeader(string rec)
        {
            string[] field = rec.Split(':');

            // check that the header record contains the correct number of fields
            if (field.Length != 15)
            {
                // error, wrong number of fields in header record
            }

            // check that program name is valid
            string prgmName = field[1].ToUpper();

            /* Regular expression used to determine if all characters in the token are
             * letters or numbers. */
            Regex alphaNumeric = new Regex(@"[^0-9a-zA-Z]");

            if (2 <= prgmName.Length && prgmName.Length <= 32)
            {
                if (!(!alphaNumeric.IsMatch(prgmName) && char.IsLetter(prgmName[0])))
                {
                    // program name is not a valid label

                }
            }
            else
            {
                // program name is not the right length

            }


            // get the Assembler assigned program load address
            // see if it is properly formatted
            string assLoad = field[2].ToUpper();

            // check length, should be 4 digit hex number
            if (assLoad.Length != 4)
            {
                // error, wrong length
            }

            //check that it is valid hex
            int assLoadVal = 0;
            try
            {
                assLoadVal = Convert.ToInt32(assLoad, 16);
            }
            catch (FormatException ex)
            {
                // error, not valid hex
            }

            // check that it is in the correct range
            if (assLoadVal < 0 || assLoadVal > 1023)
            {
                // error, must be between 0 and 1023
            }


            // get the module length
            string modLen = field[3].ToUpper();

            // check that it is a 4 digit hex
            if (modLen.Length != 4)
            {
                // error, wrong length
            }

            // check that it is valid hex
            int modLenVal = 0;
            try
            {
                modLenVal = Convert.ToInt32(modLen, 16);
            }
            catch (FormatException ex)
            {
                // error, not valid hex
            }

            // check that it is in the correct range
            if (modLenVal < 0 || modLenVal > 1024)
            {
                // error, must be between 0 and 1024
            }


            // get the execution start address
            string execAdd = field[4].ToUpper();

            // check length, should be 4 digit hex number
            if (execAdd.Length != 4)
            {
                // error, wrong length
            }

            //check that it is valid hex
            int execAddVal = 0;
            try
            {
                execAddVal = Convert.ToInt32(execAdd, 16);
            }
            catch (FormatException ex)
            {
                // error, not valid hex
            }

            // check that it is in the correct range
            if (execAddVal < 0 || execAddVal > 1023)
            {
                // error, must be between 0 and 1023
            }


            // get date and time of assembly
            string dateAndTime = String.Format("{0}:{1}:{2}", field[5], field[6], field[7]);

            // check that it is the proper length
            if (dateAndTime.Length != 16)
            {
                // error?, not the proper length
            }


            // get version number of assembler that assembled this header record
            string verNum = field[8].ToUpper();

            // check that it's the proper length, 4 digits
            if (verNum.Length != 4)
            {
                // error?, not the proper length
            }


            // get the total number of records in the object file
            string totalRec = field[9].ToUpper();

            // check length, should be 4 digit hex number
            if (totalRec.Length != 4)
            {
                // error, wrong length
            }

            //check that it is valid hex
            int totalRecVal = 0;
            try
            {
                totalRecVal = Convert.ToInt32(totalRec, 16);
            }
            catch (FormatException ex)
            {
                // error, not valid hex
            }


            // get the number of linking records in the object file
            string linkRec = field[10].ToUpper();

            // check length, should be 4 digit hex number
            if (linkRec.Length != 4)
            {
                // error, wrong length
            }

            // check that it is valid hex
            int linkRecVal = 0;
            try
            {
                linkRecVal = Convert.ToInt32(linkRec, 16);
            }
            catch (FormatException ex)
            {
                // error, not valid hex
            }


            // get the number of text records in the object file
            string textRec = field[11].ToUpper();

            // check length, should be 4 digit hex number
            if (textRec.Length != 4)
            {
                // error, wrong length
            }

            // check that it is valid hex
            int textRecVal = 0;
            try
            {
                textRecVal = Convert.ToInt32(textRec, 16);
            }
            catch (FormatException ex)
            {
                // error, not valid hex
            }

            // check that the number is in the proper range
            if (textRecVal < 0 || textRecVal > modLenVal)
            {
                // error, can only have 0 to module length text records
            }


            // get the number of modify records in the object file
            string modRec = field[12].ToUpper();

            // check length, should be 4 digit hex number
            if (modRec.Length != 4)
            {
                // error, wrong length
            }

            // check that it is valid hex
            int modRecVal = 0;
            try
            {
                modRecVal = Convert.ToInt32(modRec, 16);
            }
            catch (FormatException ex)
            {
                // error, not valid hex
            }

            
            // check that the assembler name is correct
            if (field[13].ToUpper() != "FFA-ASM")
            {
                // error? wrong assembler?
            }


            // check that the program name at the end of
            // the record is the same as at the beginning
            if (field[14].ToUpper() != prgmName)
            {
                // error, program names do not match
            }

            // add program to the symbol table
            // is this the first file we've looked at?
            if (fileNum < 1)
            {
                address = assLoadVal + modLenVal;
            }
            else
            {

            }
        }

        public void ParseLink(string rec)
        {
            string[] field = rec.Split(':');

            // check if record has the correct number of fields
            if (field.Length != 4)
            {
                // error, wrong number of fields
            }

            // check that Entry name is valid
            string entry = field[1].ToUpper();

            /* Regular expression used to determine if all characters in the token are
             * letters or numbers. */
            Regex alphaNumeric = new Regex(@"[^0-9a-zA-Z]");

            if (2 <= entry.Length && entry.Length <= 32)
            {
                if (!(!alphaNumeric.IsMatch(entry) && char.IsLetter(entry[0])))
                {
                    // entry name is not a valid label

                }
            }
            else
            {
                // entry name is not the right length

            }


            // get the location within the program
            string pgmLoc = field[2].ToUpper();

            // check length, should be 4 digit hex number
            if (pgmLoc.Length != 4)
            {
                // error, wrong length
            }

            //check that it is valid hex
            int pgmLocVal = 0;
            try
            {
                pgmLocVal = Convert.ToInt32(pgmLoc, 16);
            }
            catch (FormatException ex)
            {
                // error, not valid hex
            }

            // check that it is in the correct range
            if (pgmLocVal < 0 || pgmLocVal > 1023)
            {
                // error, must be between 0 and 1023
            }


            // make sure the program name is properly formatted
            // and in the symbol table
            string pgmName = field[3];

            if (2 <= pgmName.Length && pgmName.Length <= 32)
            {
                if (!(!alphaNumeric.IsMatch(pgmName) && char.IsLetter(pgmName[0])))
                {
                    // program name is not a valid label

                }
            }
            else
            {
                // program name is not the right length
                
            }

            // check that program name is in symbol table

            // add entry to symbol table
        }

        public void ParseText(string rec)
        {
            string[] field = rec.Split(':');

            // check that the record has the correct number of fields
            if (field.Length != 6)
            {
                // error, wrong number of fields
            }

            // check program assigned location
            string progLoc = field[1].ToUpper();

            // check length, should be 4 digit hex number
            if (progLoc.Length != 4)
            {
                // error, wrong length
            }

            //check that it is valid hex
            int progLocVal = 0;
            try
            {
                progLocVal = Convert.ToInt32(progLoc, 16);
            }
            catch (FormatException ex)
            {
                // error, not valid hex
            }

            // check that it is in the correct range
            if (progLocVal < 0 || progLocVal > 1023)
            {
                // error, must be between 0 and 1023
            }

            // check the hex code of the instruction
            string hexCode = field[2].ToUpper();

            // check length, should be 4 digit hex number
            if (hexCode.Length != 4)
            {
                // error, wrong length
            }

            //check that it is valid hex
            int hexCodeVal = 0;
            try
            {
                hexCodeVal = Convert.ToInt32(hexCode, 16);
            }
            catch (FormatException ex)
            {
                // error, not valid hex
            }


            // check address status flag
            string addStatus = field[3].ToUpper();

            // check length of flag, should be 1
            if (addStatus.Length != 1)
            {
                // error wrong length
            }

            // check that the flag is a valid value
            if (!(addStatus[0] == 'A' || addStatus[0] == 'R' || addStatus[0] == 'M'))
            {
                // error, invalid value for status flag
            }


            // check number of M adjustments needed
            string mAdj = field[4].ToUpper();

            // check that it is the correct length, should be 1
            if (mAdj.Length != 1)
            {
                // error wrong length
            }

            // check that it is valid hex
            int mAdjVal = 0;
            try
            {
                mAdjVal = Convert.ToInt32(mAdj, 16);
            }
            catch (FormatException ex)
            {
                // error, not valid hex
            }
            // if it's valid then it should be in the correct range
            // since one hex digit can only be 0-15


            // check the program name
            string prgmName = field[5].ToUpper();

            // check that the program name is in the symbol table
        }

        public void ParseModify(string rec)
        {
            string[] field = rec.Split(':');

            // check the location
            string loc = field[1];

            // check the length of the location
            if (loc.Length != 4)
            {
                // error, incorrect length
            }

            // check that it is a valid hex string
            int locVal = 0;
            try
            {
                locVal = Convert.ToInt32(loc, 16);
            }
            catch (FormatException ex)
            {
                // error, not valid hex
            }

            // check that the location value is in the proper range
            if (locVal < 0 || locVal > 1023)
            {
                // error, location invalid
            }
            
            // check the hex code of the word
            string hex = field[2];

            // check the length of the hex code
            if (hex.Length != 4)
            {
                // error, incorrect length
            }

            // check that it is a valid hex string
            int hexVal = 0;
            try
            {
                hexVal = Convert.ToInt32(hex, 16);
            }
            catch (FormatException ex)
            {
                // error, not valid hex
            }

            /* Regular expression used to determine if all characters in the token are
             * letters or numbers. */
            Regex alphaNumeric = new Regex(@"[^0-9a-zA-Z]");

            // go through the modifications and make sure they are formatted correctly
            string sign = field[3];
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

                    }
                }
                else
                {
                    // label is not the right length

                }

                // get next sign
                sign = field[i++];
            }
            // reached end of modification record, or there was an error
            // in the format of modifications

            // check that label is valid label
            if (2 <= sign.Length && sign.Length <= 32)
            {
                if (!(!alphaNumeric.IsMatch(sign) && char.IsLetter(sign[0])))
                {
                    // label is not a valid label

                }
            }
            else
            {
                // label is not the right length

            }

            // check if the label is in the symbol table
            // if it is, it's probably not an error
            // if it isn't, it could be an error: say something about it
        }

        public void ParseEnd(string rec)
        {
            string[] field = rec.Split(':');

            // check that the record has the correct number of fields
            if (field.Length != 2)
            {
                // record has the wrong number of fields
            }

            // check that program name is valid
            string prgmName = field[1].ToUpper();

            /* Regular expression used to determine if all characters in the token are
             * letters or numbers. */
            Regex alphaNumeric = new Regex(@"[^0-9a-zA-Z]");

            if (2 <= prgmName.Length && prgmName.Length <= 32)
            {
                if (!(!alphaNumeric.IsMatch(prgmName) && char.IsLetter(prgmName[0])))
                {
                    // program name is not a valid label

                }
            }
            else
            {
                // program name is not the right length

            }


            // check that this program name is in the symbol table.
        }
    }
}
