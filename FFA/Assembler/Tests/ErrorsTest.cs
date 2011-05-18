using System;
using NUnit.Framework;

namespace Assembler
{

    /**
     * List of tests designed specifically to check correct functionality of procedures in the Errors class.
     * See the test plan for more information.
     */
    [TestFixture]
    public class ErrorsTest
    {
        /**
         * [E1] Checking that fatal errors are read in.
         */
        [Test]
        public void ErrorsContainsFatal()
        {
            var err = Errors.GetInstance();
            Assert.AreEqual("[Fatal][1] Invalid start directive.  Assembler has been stopped.", err.GetFatalError(1).ToString());
        }

        /**
         * [E2] Checking that serious errors are read in.
         */
        [Test]
        public void ErrorsContainsSerious()
        {
            var err = Errors.GetInstance();
            Assert.AreEqual("[Serious][3] Function is invalid for instruction; line substituted with a NOP.", err.GetSeriousError(3).ToString());
        }

        /**
         * [E3] Checking that warning errors are read in.
         */
        [Test]
        public void ErrorsContainsWarning()
        {
            var err = Errors.GetInstance();
            Assert.AreEqual("[Warning][1] Blank Line, line ignored and no message given.", err.GetWarningError(1).ToString());
        }

        /**
         * [E4] Checking that the last item in the error file is included. See concerns for this test.
         */
        [Test]
        public void ErrorsContainsLast()
        {
            var err = Errors.GetInstance();
            Assert.AreEqual("[Warning][6] Invalid input after the operand. Input after operand is ignored. Assembly continues.", err.GetWarningError(6).ToString());
        }
    }
}
