using System;
using System.IO;

namespace Simulator
{
    public class Parser
    {
        public Parser()
        {

        }

        public void ParseFile(string filename)
        {
            var file = new StreamReader(filename);

            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                    string[] parts = line.Split(':');

                    // header record found
                    if (parts[0] == "H")
                    {
                        ParseHeader(parts);
                    }
                    // text record found
                    else if (parts[0] == "T")
                    {
                        ParseText(parts);
                    }
                    // end record found
                    else if (parts[0] == "E")
                    {
                        ParseEnd(parts);
                    }
                    else
                    {
                        // TODO error
                        throw new NotImplementedException();
                    }
                }
            }

            file.Close();
        }

        public void ParseHeader(string[] parts)
        {
            // TODO check that the header is of the correct length

        }

        public void ParseText(string[] parts)
        {
            // TODO check size
            // TODO check that program name matches
        }

        public void ParseEnd(string[] parts)
        {
            // TODO check size and parts
        }
    }
}

