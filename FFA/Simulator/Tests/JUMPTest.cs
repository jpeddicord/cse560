using System;
using NUnit.Framework;

namespace Simulator
{
    /**
     * List of tests designed specifically to check correct functionality of procedures in the JUMP class.
     * See the test plan for more information.
     */
    [TestFixture]
    public class JUMPTest
    {
        /**
         * [J1] Tests that jump sets the location counter when it is <
         */
        [Test]
        public void LTTakeJump()
        {
            Runtime.GetInstance().LC = 0;
            STACK.Run("PUSH", "0010110000000000");
            STACK.Run("TEST", "0011110000000001");
            JUMP.Run("<", "0101000000001111");
            Assert.AreEqual(15, Runtime.GetInstance().LC);
        }

        /**
         * [J2] Tests that jump does not set the location counter when it is not <
         */
        [Test]
        public void LTDontTakeJump()
        {
            Runtime.GetInstance().LC = 0;
            STACK.Run("PUSH", "0010110000000010");
            STACK.Run("TEST", "0011110000000010");
            JUMP.Run("<", "0101000000001111");
            Assert.AreEqual(0, Runtime.GetInstance().LC);
        }

        /**
         * [J3] Tests that jump sets the location counter when it is >
         */
        [Test]
        public void GTTakeJump()
        {
            Runtime.GetInstance().LC = 0;
            STACK.Run("PUSH", "0010110000000001");
            STACK.Run("TEST", "0011110000000000");
            JUMP.Run(">", "0101000000001111");
            Assert.AreEqual(15, Runtime.GetInstance().LC);
        }

        /**
         * [J4] Tests that jump does not set the location counter when it is not >
         */
        [Test]
        public void GTDontTakeJump()
        {
            Runtime.GetInstance().LC = 0;
            STACK.Run("PUSH", "0010110000000000");
            STACK.Run("TEST", "0011110000000001");
            JUMP.Run(">", "0101000000001111");
            Assert.AreEqual(0, Runtime.GetInstance().LC);
        }

        /**
         * [J5] Tests that jump sets the location counter when it is =
         */
        [Test]
        public void EqTakeJump()
        {
            Runtime.GetInstance().LC = 0;
            STACK.Run("PUSH", "0010110000000001");
            STACK.Run("TEST", "0011110000000001");
            JUMP.Run("=", "0101000000001111");
            Assert.AreEqual(15, Runtime.GetInstance().LC);
        }

        /**
         * [J6] Tests that jump does not set the location counter when it is not =
         */
        [Test]
        public void EqDontTakeJump()
        {
            Runtime.GetInstance().LC = 0;
            STACK.Run("PUSH", "0010110000000001");
            STACK.Run("TEST", "0011110000000000");
            JUMP.Run("=", "0101000000001111");
            Assert.AreEqual(0, Runtime.GetInstance().LC);
        }
    }
}
