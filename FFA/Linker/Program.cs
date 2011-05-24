using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Error = Assembler.ErrorException;

namespace Linker
{
    /**
     * Runs the Linker.
     */
    class Program
    {
        /**
         * Output usage information
         *
         * @refcode
         * @errtest
         * @errmsg
         * @author Mark Mathis
         * @creation May 23, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        public static void Usage()
        {
            Console.WriteLine("FFA Linker");
            Console.WriteLine("Usage:  Linker obj1.obj [obj2.obj...]");
            Console.WriteLine("Linker will link all obj files into obj1.ffa");
        }

        /**
         * Makes the Linker do things in the right order.
         * 
         * @refcode
         * @errtest
         * @errmsg
         *  ES.57 and any errors caused by every other part of the Linker
         * @author Mark Mathis
         * @creation May 18, 2011
         * @modlog
         * @teststandard Andrew Buelow
         * @codestandard Mark Mathis
         */
        static void Main(string[] args)
        {
            // allows the use of the Assembler.Errors class for error handling
            Assembler.Errors.SetResource(Properties.Resources.Errors);

            // get the command line arguments
            string[] infiles = new string[1];
            if (args.Length > 0)
            {
                infiles = args;
            }
            else
            {
                // there must be at least one input file
                Usage();
                System.Environment.Exit(1);
            }

            // output the file to a file using the first argument minus the extension
            // with the .ffa extension added to the end
            string outfile = System.IO.Path.GetFileNameWithoutExtension(infiles[0]) + ".ffa";

            // get all the things needed to parse the input files
            Parser pars = new Parser();

            // how many files have been parsed
            int file = 0;

            // the address the parser should use when calculating the module load addresses
            int address = 0;

            // the symbol table to save symbols in
            SymbolTable symb = new SymbolTable();
            // the list to save the modules in
            List<Module> modules = new List<Module>();

            // Let's parse us some files
            try
            {
                foreach (var f in infiles)
                {
                    // the module that will hold the object file currently being parsed
                    Module mod = new Module();
                    try
                    {
                        pars.ParseFile(f, out mod, symb, file, ref address);
                    }
                    catch (Error ex)
                    {
                        // there was a problem while parsing :(
                        if (ex.err.category == Assembler.Errors.Category.Fatal)
                        {
                            // fatal error should stop the linker
                            // keep the exception moving
                            throw ex;
                        }
                        else
                        {
                            // not fatal, output the error and keep on going
                            Console.WriteLine(ex.err);
                            continue;
                        }
                    }
                    // add the module to the module list
                    modules.Add(mod);

                    // a file has been parsed
                    file++;
                }

                // make sure that modules were actually produced from the parsing
                if (modules.Count > 0)
                {
                    // create the load file with our newfound knowledge
                    LoadFile load = new LoadFile(modules, symb);
                    load.Render(outfile);
                }
                else
                {
                    // no valid files parsed
                    Assembler.Errors.GetInstance().PrintError(Assembler.Errors.Category.Serious, 57);
                }

                // output the symbol table after successfully making the load file
                Console.WriteLine(symb);
            }
            catch (Error)
            {
                // gracefully die on fatal exceptions
                Console.WriteLine("Stopping linker due to fatal error.");

                // output however much of the symbol table was made before the
                // fatal error was encountered. it probably won't ever be much
                Console.WriteLine(symb);
                System.Environment.Exit(1);
            }
            catch (Exception)
            {
                // die with a bad excuse for anything else
                Console.WriteLine("Something bad happened and the Linker could not recover.");
                System.Environment.Exit(2);
            }
        }
    }
}
