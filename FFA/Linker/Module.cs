using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linker
{
    class Module
    {
        Header headerRecord;

        Dictionary<int, Text> textRecords;

        Dictionary<int, Linking> linkingRecords;

        Dictionary<int, Modify> modifyRecords;

        public Module(Header header)
        {

        }
    }
}
