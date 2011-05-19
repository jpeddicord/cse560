using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Linker
{
    class Parser
    {
        public void ParseFile(string file)
        {
            string[] records = File.ReadAllLines(file);

            foreach (string rec in records)
            {
                if (rec[0] == 'H')
                {
                    ParseHeader(rec);
                }
                else if (rec[0] == 'L')
                {
                    ParseText(rec);
                }
                else if (rec[0] == 'T')
                {
                    ParseLink(rec);
                }
                else if (rec[0] == 'M')
                {
                    ParseModify(rec);
                }
                else if (rec[0] == 'E')
                {
                    ParseEnd(rec);
                }
                else
                {
                    // Invalid record type
                }
            }
        }

        public void ParseHeader(string rec)
        {
            string[] field = rec.Split(':');

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
            if (modLenVal < 0 || modLenVal > 1023)
            {
                // error, must be between 0 and 1023
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
            string verNum = field[8];

            // check that it's the proper length, 4 digits
            if (verNum.Length != 4)
            {
                // error?, not the proper length
            }


            // get the total number of records in the object file
            string totalRec = field[9];

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

            // check that it is in the correct range
            if (totalRecVal < 0 || totalRecVal > 1023)
            {
                // error, must be between 0 and 1023
            }



        }

        public void ParseLink(string rec)
        {
            string[] field = rec.Split(':');

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



        }

        public void ParseText(string rec)
        {
            string[] field = rec.Split(':');
        }

        public void ParseModify(string rec)
        {
            string[] field = rec.Split(':');
        }

        public void ParseEnd(string rec)
        {
            string[] field = rec.Split(':');
        }
    }
}
