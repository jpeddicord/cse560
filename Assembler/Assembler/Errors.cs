using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assembler
{
    
    class Errors
    {
        /**
         * The type of error. Specifies three categories. <br />
         * Fatal - Cannot be recovered from, causes Assembler to stop. <br />
         * Serious - Can be recovered from, will probably lead to erratic program behavior. <br />
         * Warning - Can be recovered from, may lead to erratic program behavior.
         */
        public enum Category
        {
            Fatal, Serious, Warning
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
        private Dictionary<Errors.Category, Dictionary<int, string>> errorList;

        /**
         * Creates an object that can be used to look up errors.
         */
        private Errors()
        {
            // initialize the Dictionary that holds the errors
            errorList = new Dictionary<Category, Dictionary<int, string>>();

            // create the error categories in the Dictionary
            errorList[Category.Fatal] = new Dictionary<int,string>();
            errorList[Category.Serious] = new Dictionary<int,string>();
            errorList[Category.Warning] = new Dictionary<int,string>();

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

                // actual error message will start at the 6th character in the line
                string errorMsg = line.Substring(6);

                switch (errorCode[0].ToUpper())
                {
                    case "EF":
                        {
                            // this error is a fatal error
                            errorList[Category.Fatal][numCode] = errorMsg;
                        }break;
                    case "ES":
                        {
                            errorList[Category.Serious][numCode] = errorMsg;
                        }break;
                    case "EW":
                        {
                            errorList[Category.Warning][numCode] = errorMsg;
                        }break;
                    default:
                        {
                            Logger288.Log(String.Format("Invalid errorCategory: {0}.  Skipping.",
                                                         errorCode[0]), "Errors");
                        }break;
                }
            }

            Logger288.Log("Finished reading error messages.", "Errors");
        }

        /**
         * Return the singleton instance of Errors.
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

        public string GetFatalError(int errCode)
        {
            return errorList[Category.Fatal][errCode];
        }

        public string GetSeriousError(int errCode)
        {
            return errorList[Category.Serious][errCode];
        }

        public string GetWarningError(int errCode)
        {
            return errorList[Category.Warning][errCode];
        }
    }
}
