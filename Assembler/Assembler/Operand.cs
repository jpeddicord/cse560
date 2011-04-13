using System;

namespace Assembler
{
    static class Operand
    {
        /**
         * Document!
         */
        public enum Type {
            elabel,
            mlabel,
            rlabel,
            olabel,
            n,
            nn,
            nnn,
            nmax,
            nnnnnn,
            none
        }

        /**
         * Check if a directive operand fits within the given type.
         * TODO: DOC
         */
        public static bool IsValidDirectiveOperand(string lit, Type type)
        {
            // bail early if passed in an error literal
            if (lit == "_ERROR")
            {
                return false;
            }

            return true;
        }

        /**
         * Check if an instruction operand fits within the given type.
         */
        public static bool IsValidInstructionOperand(string lit, Type type)
        {
            // TODO
            return true;
        }
    }
}

