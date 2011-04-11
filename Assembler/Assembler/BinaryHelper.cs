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
    class BinaryHelper
    {
        
        public BinaryHelper()
        {
           // Write a message to the log to confirm the binary helper has been initialized
           Trace.WriteLine("Initializing Binary Helper.", "Binary Helper");
         }

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
         * @refcode N/A
         * @errtest 
         *          Tested using a mix of digits and converting from and to two's complement.
         * @errmsg N/A
         * @author Andrew
         * @creation April 10, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         *
         * @param number The integer to be converted to or from two's complement.
         * @param digits The number of digits the number is being represented by in base 2.
         * @return The number converted from or to two's complement.
         */
        public static int ConvertNumber(int number, int digits)
        {
            /**
             * Stores the value of the converted number.
             */
            int convertedNum = 0;

            // Used to enforce digit number restrictions which are a result of integer size.
            if (17 > digits && digits > 1)
            {
                /**
                 * Stores the point at which a number would become a negative number in two's complement.
                 */
                int limit = (int)(Math.Pow(2, digits - 1));

                /**
                 * Stores -1 in two's complement with results in a binary number of size digits filled with 1's.
                 */
                int filledDigits = ((int)(Math.Pow(2, digits))) - 1;

                // If given a negative number within the limit, we should convert to the two's complement equivalent.
                if ((0 > number) && (number >= (-1) * limit))
                {
                    // Take the absolute value of the number and exclusive or it with the filled digits which
                    // flips all of the bits, then add one.
                    convertedNum = (Math.Abs(number) ^ filledDigits) + 1;
                } // if within the limit for a negative two's complement, we want to convert to non-two's complement.
                else if ((limit <= number) && (number < limit * 2))
                {
                    // subtract one from the number then exclusive or it and multiply by -1 to make it a negative number.
                    convertedNum = ((number - 1) ^ filledDigits) * -1;
                }
            }
    
            return convertedNum;
        }
    }
}