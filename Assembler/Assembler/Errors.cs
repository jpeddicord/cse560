using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assembler
{
    /**
     * TODO: DOCUMENT
     */
    class Errors
    {
        /**
         * The type of error. Specifies three categories.
         * - Fatal - Cannot be recovered from, causes Assembler to stop.
         * - Serious - Can be recovered from, will probably lead to erratic program behavior.
         * - Warning - Can be recovered from, may lead to erratic program behavior.
         */
        public enum Category
        {
            Fatal, Serious, Warning
        };

        /**
         * An Error structure to hold an error severity and message.
         */
        public struct Error
        {
            public Category category;
            public string msg;

            /**
             * String representation of this error.
             */
            public override string ToString()
            {
                return "[" + this.category.ToString() + "] " + this.msg;
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

        /**
         * Creates an object that can be used to look up errors. These errors will be read in from a
         * resource file.
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg
         *  - Reports errors in reading individual lines of the text file to the log. Silently skips
         *          lines that are unreadable.
         * @author Mark
         * @creation April 14, 2011
         * @modlog
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

            foreach (string line in Properties.Resources.errors.Split('\n'))
            {
                // expecting first 5 characters of line to be error code
                // something like EF.01
                string[] errorCode = line.Substring(0, 5).Split('.');
                int numCode;
                if (errorCode.Length == 2)
                {
                    if (!int.TryParse(errorCode[1], out numCode))
                    {
                        Logger288.Log(String.Format("Error reading error code, skipping {0}.{1}",
                                                    errorCode[0], errorCode[1]), "Errors");
                        continue;
                    }
                }
                else
                {
                    Logger288.Log("Improper error code format, skipping \"" + line.Substring(0,5) + "\"...", "Errors");
                    continue;
                }

                // actual error message will start after a space
                Error err;
                err.msg = line.Substring(line.IndexOf(' ') + 1);

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
                            Logger288.Log(String.Format("Invalid errorCategory: {0}.  Skipping.",
                                                         errorCode[0]), "Errors");
                        } break;
                }
            }

            Logger288.Log("Finished reading error messages.", "Errors");
        }

        /**
         * Return the singleton instance of Errors.
         * 
         * @return the instance of this class
         * 
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 14, 2011
         * @modlog 
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public static Errors GetInstance()
        {
            Logger288.Log("Request for instance of Errors.", "Errors");

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
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
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
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
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
         * @refcode N/A
         * @errtest N/A
         * @errmsg N/A
         * @author Mark
         * @creation April 14, 2011
         * @modlog 
         * @codestandard Mark Mathis
         * @teststandard Andrew Buelow
         */
        public Error GetWarningError(int errCode)
        {
            return errorList[Category.Warning][errCode];
        }
    }
}
