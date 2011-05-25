using System;
using NUnit.Framework;

namespace Simulator
{
    /**
     * List of tests designed specifically to check correct functionality of procedures in the CNTL class.
     * See the test plan for more information.
     */
    [TestFixture]
    public class CNTLTest
    {
        /**
         * [C1] Tests that Goto works with the minimum LC.
         */
        [Test]
        public void GotoMin()
        {
            CNTL.Run("GOTO", "0010000000000000");
            Assert.AreEqual(0, Runtime.GetInstance().LC);
        }

        /**
         * [C2] Tests that Goto works with the maximum LC.
         */
        [Test]
        public void GotoMax()
        {
            CNTL.Run("GOTO", "0010001111111111");
            Assert.AreEqual(1023, Runtime.GetInstance().LC);
        }

        /**
         * [C3] Tests that Goto works with an arbitrary LC.
         */
        [Test]
        public void Goto()
        {
            CNTL.Run("GOTO", "0010000001100010");
            Assert.AreEqual(98, Runtime.GetInstance().LC);
        }

        /**
         * [C4] Tests that CLRD removes all items from the data stack. Assuming STACK push works from stack testing.
         */
        [Test]
        public void CLRD()
        {
            STACK.Run("PUSH", "0010110000000001");
            CNTL.Run("CLRD", "0010000001100010");
            Assert.AreEqual(0, Memory.GetInstance().DataSize());
        }

        /**
         * [C5] Tests that CLRT removes all items from the test stack. Assuming STACK Test works from stack testing.
         */
        [Test]
        public void CLRT()
        {
            STACK.Run("PUSH", "0010110000000001");
            STACK.Run("TEST", "0011110000000000");
            CNTL.Run("CLRT", "0010000001100010");
            Assert.AreEqual(0, Memory.GetInstance().TestSize());
        }
    }
}
