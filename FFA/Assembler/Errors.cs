using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assembler
{
    /**
     * An exception representing an Error.
     */
    public class ErrorException : ApplicationException
    {
        public Errors.Error err;

        public ErrorException(Errors.Category category, int code)
        {
            this.err.category = category;
            this.err.code = code;
            Errors inst = Errors.GetInstance();
            if (category == Errors.Category.Warning)
            {
                this.err.msg = inst.GetWarningError(code).msg;
            }
            else if (category == Errors.Category.Serious)
            {
                this.err.msg = inst.GetSeriousError(code).msg;
            }
            else if (category == Errors.Category.Fatal)
            {
                this.err.msg = inst.GetFatalError(code).msg;
            }
        }

        public override string ToString()
        {
            return this.err.ToString();
        }
    }

    /**
     * A singleton class that can be used to fetch the list of built-in errors
     * and their levels and messages.
     */
    public class Errors
    {
        /**
         * The severity of the error.
         */
        public enum Category
        {
            /**
             * Cannot be recovered from, causes Assembler to stop.
             */
            Fatal,

            /**
             * Can be recovered from, will probably lead to erratic program behavior.
             * Afflicted lines may be replaced with NOPs.
             */
            Serious,

             /**
              * Can be recovered from, may lead to erratic program behavior.
              */
            Warning
        };

        /**
         * An Error structure to hold an error severity and message.
         */
        public struct Error
        {
            public int code;
            public Category category;
            public string msg;

            /**
             * String representation of this error.
             */
            public override string ToString()
            {
                return "[" + this.category.ToString() + "][" + this.code + "] " + this.msg;
            }
        };

        /**
         * Singleton instance of this class.
         */
        private static Errors instance;

        /**
         * Stores the errors for easy access during program execution.
         * The errors will be accessed with a category (fatal, serious, warning)
         * and then an error code, e.g. 1, 2, or 3.
         */
        private Dictionary<Errors.Category, Dictionary<int, Error>> errorList;

        private static string errorRes = "";

        /**
         * Creates an object that can be used to look up errors. These errors
         * will be read in from a resource file.
         * 
         * @refcode ES, EF, EW
         * @errtest
         *  Loading and types of errors
         * @errmsg
         *  Reports errors in reading individual lines of the text file to the log. Silently skips lines that are unreadable.
         * @author Mark Mathis
         * @creation April 14, 2011
         * @modlog
         *  - May 20, 2011 - Jacob - Now ignores blank lines when parsing errors file.
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        private Errors()
        {
            // initialize the Dictionary that holds the errors
            errorList = new Dictionary<Category, Dictionary<int, Error>>();

            // create the error categories in the Dictionary
            errorList[Category.Fatal] = new Dictionary<int, Error>();
            errorList[Category.Serious] = new Dictionary<int, Error>();
            errorList[Category.Warning] = new Dictionary<int, Error>();

            foreach (string line in Errors.errorRes.Split('\n'))
            {
                // ignore empty lines
                if (line.Trim().Length == 0)
                {
                    continue;
                }

                // expecting first 5 characters of line to be error code
                // something like EF.01
                string[] errorCode = line.Substring(0, 5).Split('.');
                int numCode;
                if (errorCode.Length == 2)
                {
                    if (!int.TryParse(errorCode[1], out numCode))
                    {
                        Logger.Log(String.Format("Error reading error code, skipping {0}.{1}",
                                                    errorCode[0], errorCode[1]), "Errors");
                        continue;
                    }
                }
                else
                {
                    Logger.Log("Improper error code format, skipping \"" + line.Substring(0,5) + "\"...", "Errors");
                    continue;
                }

                // actual error message will start after a space
                Error err;
                err.code = numCode;
                err.msg = line.Substring(line.IndexOf(' ') + 1).Trim();

                switch (errorCode[0].ToUpper())
                {
                    case "EF":
                        {
                            // this error is a fatal error
                            err.category = Errors.Category.Fatal;
                            errorList[Category.Fatal][numCode] = err;
                        } break;
                    case "ES":
                        {
                            // this error is a serious error
                            err.category = Errors.Category.Serious;
                            errorList[Category.Serious][numCode] = err;
                        } break;
                    case "EW":
                        {
                            // this error is a warning error
                            err.category = Errors.Category.Warning;
                            errorList[Category.Warning][numCode] = err;
                        } break;
                    default:
                        {
                            // no category found
                            Logger.Log(String.Format("Invalid errorCategory: {0}.  Skipping.",
                                                     errorCode[0]), "Errors");
                        } break;
                }
            }

            Logger.Log("Finished reading error messages.", "Errors");
        }

        /**
         * Set the error string resource to load from.
         *
         * @param errs Resource to use
         * 
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Jacob Peddicord
         * @creation May 20, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public static void SetResource(string errs)
        {
            Errors.errorRes = errs;
        }

        /**
         * Return the singleton instance of Errors.
         * 
         * @return the instance of this class
         * 
         * @refcode N/A
         * @errtest
         *  N/A
         * @errmsg
         *  N/A
         * @author Mark Mathis
         * @creation April 14, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public static Errors GetInstance()
        {
            if (Errors.instance == null)
            {
                Errors.instance = new Errors();
            }

            return Errors.instance;
        }

        /**
         * Returns the fatal error that corresponds to the specified error code.
         *
         * @param errCode error id
         * @return the error message specified by EF.errCode
         * 
         * @refcode EF
         * @errtest
         *  The type of error returned
         * @errmsg
         *  None
         * @author Mark Mathis
         * @creation April 14, 2011
         * @modlog
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public Error GetFatalError(int errCode)
        {
            return errorList[Category.Fatal][errCode];
        }

        /**
         * Returns the serious error that corresponds to the specified error code.
         *
         * @param errCode error id
         * @return the error message specified by EF.errCode
         * 
         * @refcode ES
         * @errtest
         *  The type of error returned
         * @errmsg
         *  None
         * @author Mark Mathis
         * @creation April 14, 2011
         * @modlog 
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public Error GetSeriousError(int errCode)
        {
            return errorList[Category.Serious][errCode];
        }

        /**
         * Returns the warning error that corresponds to the specified error code.
         *
         * @param errCode error id
         * @return the error message specified by EF.errCode
         * 
         * @refcode EW
         * @errtest
         *  The type of error returned
         * @errmsg
         *  None
         * @author Mark Mathis
         * @creation April 14, 2011
         * @modlog 
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public Error GetWarningError(int errCode)
        {
            return errorList[Category.Warning][errCode];
        }

        /**
         * Print the specified error to the console.
         *
         * @param cat category of error
         * @param errCode error id
         * 
         * @refcode EW, ES, EF
         * @errtest
         * @errmsg
         *  None
         * @author Mark Mathis
         * @creation May 22, 2011
         * @modlog 
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public void PrintError(Category cat, int errCode)
        {
            
            if (cat == Category.Fatal)
            {
                Console.WriteLine(GetFatalError(errCode));
            }
            else if (cat == Category.Serious)
            {
                Console.WriteLine(GetSeriousError(errCode));
            }
            else if (cat == Category.Warning)
            {
                Console.WriteLine(GetWarningError(errCode));
            }
        }
    }
}
