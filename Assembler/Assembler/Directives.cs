using System;
using System.Collections;
using System.Linq;
using System.Text;
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
         * A private constructor that fills the list with the directives.
         */
        private Directives()
        {
            Trace.WriteLine(String.Format("{0} -> {1}", DateTime.Now, "Creating instance of Directives."), "Directives");
            directiveList = new ArrayList(Properties.Resources.directives.Split('\n'));
        }

        /**
         * This is called to retrieve or create the single instance of the Directives class.
         */
        public static Directives GetInstance()
        {
            Trace.WriteLine(String.Format("{0} -> {1}", DateTime.Now, 
                "Request for instance of Directives."), "Directives");
            if (Directives.instance == null)
            {
                instance = new Directives();
            }

            return instance;
        }

        /**
         * Returns true if dir is contained in the list of directives. False, otherwise.
         */
        public bool Contains(string dir)
        {
            return directiveList.Contains(dir.ToUpper());
        }
    }
}
