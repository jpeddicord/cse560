using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembler.Errors.SetResource(Properties.Resources.errors);

            var parser = new Parser();
            bool success = parser.ParseFile(args[0]);

            if (success)
            {
                Runtime.GetInstance().Run();
            }
        }
    }
}
