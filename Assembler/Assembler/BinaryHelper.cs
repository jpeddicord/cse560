using System;
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
         * This procedure can be used to convert to and from two's complement. The procedure
         * can be given a number greater than 0 and a number of digits.  If the given number
         * is greater than 2^(digits - 1) which is the largest positive number that can be
         * represented in two's complement with digits number of digits, then the number is
         * assumed to be a negative number in two's complement and will be converted to its
         * corresponding positive number.  If the number is within that range, the number is
         * considered to be a negative number that needs to be converted to its two's
         * complement equivalent.
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
         */
        public static int ConvertNumber(int number, int digits)
        {
            int convertedNum = 0;
            int limit = (int)(Math.Pow(2, digits - 1));
            int filledDigits = (limit * 2) - 1;

            Console.WriteLine("Filled: " + filledDigits);

            if (number > limit)
            {
                convertedNum = (number ^ filledDigits) + 1;
            }
            else
            {
                convertedNum = (number - 1) ^ filledDigits;
            }

            return convertedNum;
        }
    }
}
