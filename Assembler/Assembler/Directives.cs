using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Assembler
{
    class Directives
    {
        private static Directives instance;
        private static ArrayList directiveList;

        private Directives()
        {
            directiveList = new ArrayList(Properties.Resources.directives.Split('\n'));
        }

        public static Directives GetInstance()
        {
            if (Directives.instance == null)
            {
                instance = new Directives();
            }

            return instance;
        }

        public ArrayList DirList
        {
            get { return directiveList; }
        }

        public bool Contains(string dir)
        {
            return directiveList.Contains(dir.ToUpper());
        }
    }
}
