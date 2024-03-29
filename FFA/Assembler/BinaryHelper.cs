﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Assembler
{
    /**
     * Allows users to convert to and from two's complement representation.
     */
    public static class BinaryHelper
    {

        /**
         * This procedure can be used to convert to and from two's complement. When given a
         * number less than 0 but greater than or equal to -(2^(digits - 1)) the number is 
         * considered not to be in two's complement and will be converted to it's
         * equivalent two's complement value.  When given a number greater
         * than or equal to 2^(digits -1) but smaller than 2^(digits) it is considered a negative
         * value that is in two's complement and will convert it to the negative equivalent not
         * in two's complement.  Any other values given will return 0.  Requires that the number
         * of digits is less than 17 and greater than 1, any other number of digits will
         * cause the procedure to return 0.
         *
         * @param number The integer to be converted to or from two's complement.
         * @param digits The number of digits the number is being represented by in base 2. This is an
         *               optional parameter that will be 10 if not given a value.
         * @return The number converted from or to two's complement.
         *
         * @refcode N/A
         * @errtest 
         *  Tested using a mix of digits and converting from and to two's complement.
         * @errmsg
         *  N/A
         * @author Andrew Buelow
         * @creation April 10, 2011
         * @modlog
         *  - April 14, 2011 - Andrew - Altered so that it will also accept values between 0 and 2^(digits - 1) which will simply give back the same number.
         *  - April 20, 2011 - Jacob  - Changed to a 32-bit restriction instead of 16.
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static int ConvertNumber(int number, int digits = 10)
        {
            // Stores the value of the converted number.
            int convertedNum = 0;

            // Used to enforce digit number restrictions which are a result of integer size.
            if (32 >= digits && digits > 1)
            {
                // Stores the point at which a number would become a negative number in two's complement.
                int limit = (int)(Math.Pow(2, digits - 1));

                // Stores -1 in two's complement with results in a binary number of size digits filled with 1's.
                int filledDigits = ((int)(Math.Pow(2, digits))) - 1;

                // a number that doesn't have a 1 in the most significant bit will be the same in two's complement
                if ((0 <= number) && (number < limit))
                {
                    convertedNum = number;
                }
                // If given a negative number within the limit, we should convert to the two's complement equivalent.
                else if ((0 > number) && (number >= (-1) * limit))
                {
                    // Take the absolute value of the number and exclusive or it with the filled digits which
                    // flips all of the bits, then add one.
                    convertedNum = (Math.Abs(number) ^ filledDigits) + 1;
                }
                // if within the limit for a negative two's complement, we want to convert to non-two's complement.
                else if ((limit <= number) && (number < limit * 2))
                {
                    // subtract one from the number then exclusive or it and multiply by -1 to make it a negative number.
                    convertedNum = ((number - 1) ^ filledDigits) * -1;
                }
            }

            Logger.Log("Converted " + number + " to " + convertedNum, "BinaryHelper");
            return convertedNum;
        }

        /**
         * Used to determine if a number is within the range to be correctly used by ConvertNumber.
         * The number must be greater than the smallest accepted negative value that can be converted
         * to two's complement with digits number of bits (-(2^(digits - 1))).  It must also be smaller
         * than or equal to the two's complement representation of -1 (2^(digits) - 1).  The number of
         * digits must also meet the requirements of ConvertNumber in that it is larger than 1 but
         * smaller than 17. This will return false if it falls outside of this range.
         *
         * @param number The integer to be converted to or from two's complement.
         * @param digits The number of digits the number is being represented by in base 2. This is an
         *               optional parameter that will be 10 if not given a value.
         * @return True if the number and digits are within their respective specified ranges.
         *
         * @refcode N/A
         * @errtest 
         *          Tested edge cases and various values with a couple different digits.
         * @errmsg
         *  N/A
         * @author Andrew Buelow
         * @creation April 14, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static bool IsInRange(int number, int digits = 10)
        {
            // Stores the point at which a number would become a negative number in two's complement.
            int limit = (int)(Math.Pow(2, digits - 1));

            // return true if within -(2^(digits - 1)) and 2^(digits)
            return ((((-1) * limit) <= number) && (number < (limit * 2)));
        }

        /**
         * Convert a hex number (represented as a string) into a string of
         * binary digits.
         *
         * @param hex hex number string representation to convert
         * @return string of '0' and '1' characters representing the binary digits
         *
         * @refcode N/A
         * @errtest
         *  Conversions of various binary values.
         * @errmsg
         *  None
         * @author Jacob Peddicord
         * @creation April 15, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static string BinaryString(string hex)
        {
            return Convert.ToString(Convert.ToInt32(hex, 16), 2);
        }

        /**
         * Convert the given hex number of the specified bit length to a
         * 32-bit integer. Useful for checking bounds or just finding the
         * integer value of something.
         *
         * @param hex value to convert
         * @param bits bit length of the hex string
         *
         * @refcode N/A
         * @errtest
         *  Tested positive and negative number conversions in different bit sizes.
         * @errmsg
         *  None
         * @author Jacob Peddicord
         * @creation April 18, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static int HexToInt(string hex, int bits)
        {
            int val = Convert.ToInt32(hex, 16);
            val = BinaryHelper.ConvertNumber(val, bits);
            return val;
        }
    }
}
