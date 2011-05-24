using System;
using NUnit.Framework;

namespace Simulator
{
    /**
     * List of tests designed specifically to check correct functionality of procedures in the STACK class.
     * See the test plan for more information.
     */
    [TestFixture]
    public class STACKTest
    {
        /**
         * [ST1] Tests that Push works with the largest positive integer.
         */
        [Test]
        public void PushMax()
        {
            Memory m = Memory.GetInstance();
            m.SetWordInt(1, 32767);
            STACK.Run("PUSH", "0010100000000001");
            Assert.AreEqual(32767, m.GetDataStack()[0]);
        }

        /**
         * [ST2] Tests that Push works with the smallest negative integer.
         */
        [Test]
        public void PushMin()
        {
            Memory m = Memory.GetInstance();
            m.SetWordInt(1, -32768);
            STACK.Run("PUSH", "0010100000000001");
            Assert.AreEqual(32768, m.GetDataStack()[0]);
        }

        /**
         * [ST3] Tests that Push works with the largest negative integer.
         */
        [Test]
        public void PushNegativeOne()
        {
            Memory m = Memory.GetInstance();
            m.SetWordInt(1, -1);
            STACK.Run("PUSH", "0010100000000001");
            Assert.AreEqual(65535, m.GetDataStack()[0]);
        }

        /**
         * [ST4] Tests that Pop works with the largest positive integer.
         */
        [Test]
        public void PopMax()
        {
            Memory m = Memory.GetInstance();
            m.SetWordInt(1, 32767);
            STACK.Run("PUSH", "0010100000000001");
            STACK.Run("POP", "0011000000000010");
            Assert.AreEqual(32767, m.GetWord(2));
        }

        /**
         * [ST5] Tests that Pop works with the smallest negative integer.
         */
        [Test]
        public void PopMin()
        {
            Memory m = Memory.GetInstance();
            m.SetWordInt(1, -32768);
            STACK.Run("PUSH", "0010100000000001");
            STACK.Run("POP", "0011000000000010");
            Assert.AreEqual(32768, m.GetWord(2));
        }

        /**
         * [ST6] Tests that Pop works with the largest negative integer.
         */
        [Test]
        public void PopNegativeOne()
        {
            Memory m = Memory.GetInstance();
            m.SetWordInt(1, -1);
            STACK.Run("PUSH", "0010100000000001");
            STACK.Run("POP", "0011000000000010");
            Assert.AreEqual(65535, m.GetWord(2));
        }

        /**
         * [ST7] Test that STACK Test works when the item on the stack is less than the item in memory.
         */
        [Test]
        public void TestLT()
        {
            STACK.Run("PUSH", "0010110000000000");
            STACK.Run("TEST", "0011110000000001");
            Assert.AreEqual(1, Memory.GetInstance().GetTestStack()[0]);
        }

        /**
         * [ST8] Test that STACK Test works when the item on the stack is greater than the item in memory.
         */
        [Test]
        public void TestGT()
        {
            STACK.Run("PUSH", "0010110000000001");
            STACK.Run("TEST", "0011110000000000");
            Assert.AreEqual(2, Memory.GetInstance().GetTestStack()[0]);
        }

        /**
         * [ST9] Test that STACK Test works when the item on the stack is equal to the item in memory.
         */
        [Test]
        public void TestEq()
        {
            STACK.Run("PUSH", "0010110000000001");
            STACK.Run("TEST", "0011110000000001");
            Assert.AreEqual(0, Memory.GetInstance().GetTestStack()[0]);
        }
    }
}
