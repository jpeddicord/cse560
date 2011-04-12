using System;
using System.Collections;
using System.Diagnostics;

namespace Assembler
{
    /**
     * A singleton class that enables us to pass the list of directives around the program.
     */
    class Directives
    {
        /**
         * The single instance of this class.
         */
        private static Directives instance;

        /**
         * The list of directives.
         */
        private static ArrayList directiveList;

        /**
         * The number of directives.
         */
        public int DirectiveCount
        {
            get { return directiveList.Count; }
        }

        /**
         * A private constructor that fills the list with the directives.
         */
        private Directives()
        {
            Logger288.Log("Creating instance of Directives.", "Directives");
            directiveList = new ArrayList(Properties.Resources.directives.Split('\n'));
        }

        /**
         * This is called to retrieve or create the single instance of the Directives class.
         * 
         * @refcode N/A
         * @errtest 
         *          All directives are properly pulled out of the source text file.
         * @errmsg N/A
         * @author Mark
         * @creation April 8, 2011
         * @modlog
         * @codestandard Mark
         * @codestandard Andrew
         */
        public static Directives GetInstance()
        {
            Logger288.Log("Request for instance of Directives.", "Directives");
            if (Directives.instance == null)
            {
                instance = new Directives();
            }

            return instance;
        }

        /**
         * Returns true if dir is contained in the list of directives. False, otherwise.
         * 
         * @refcode N/A
         * @errtest 
         *          Directives can be found regardless of the case they are in.  Only returns true for directives that are in the source file.
         * @errmsg N/A
         * @author Mark
         * @creation April 8, 2011
         * @modlog
         * @codestandard Mark
         * @codestandard Andrew
         * @param dir The directive to check for.
         * @return true if dir is a directive <br />
         *         false if dir is not a directive
         */
        public bool Contains(string dir)
        {
            Logger288.Log(String.Format("Check if {0} is valid directive.", dir), "Directives");
            return directiveList.Contains(dir.ToUpper());
        }
    }
}
