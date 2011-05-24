using System;
using NUnit.Framework;

namespace Simulator
{
    /**
     * List of tests designed specifically to check correct functionality of procedures in the SOPER class.
     * See the test plan for more information.
     */
    [TestFixture]
    public class SOPERTest
    {
        /**
         * [SO1] Tests that Add works with multiple positive numbers.
         */
        [Test]
        public void AddPos()
        {
            STACK.Run("PUSH", "0010110000000101");
            STACK.Run("PUSH", "0010110000001001");
            STACK.Run("PUSH", "0010110000010001");
            SOPER.Run("ADD", "1000000000000011");
            Assert.AreEqual(31, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO2] Tests that Add works with multiple negative numbers.
         */
        [Test]
        public void AddNeg()
        {
            STACK.Run("PUSH", "0010111111110000");
            STACK.Run("PUSH", "0010111111110001");
            STACK.Run("PUSH", "0010111111110011");
            SOPER.Run("ADD", "100000000000011");
            Assert.AreEqual(65492, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO3] Tests that Add works with multiple positive and negative numbers.
         */
        [Test]
        public void AddMixed()
        {
            STACK.Run("PUSH", "0010111111100101");
            STACK.Run("PUSH", "0010110001001001");
            STACK.Run("PUSH", "0010110001010001");
            SOPER.Run("ADD", "1000000000000011");
            Assert.AreEqual(127, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO4] Tests that Sub works with multiple positive numbers.
         */
        [Test]
        public void SubPos()
        {
            STACK.Run("PUSH", "0010110000000101");
            STACK.Run("PUSH", "0010110000001001");
            STACK.Run("PUSH", "0010110000010001");
            SOPER.Run("SUB", "1000100000000011");
            Assert.AreEqual(3, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO5] Tests that Sub works with multiple negative numbers.
         */
        [Test]
        public void SubNeg()
        {
            STACK.Run("PUSH", "0010111111110000");
            STACK.Run("PUSH", "0010111111110001");
            STACK.Run("PUSH", "0010111111110011");
            SOPER.Run("SUB", "100010000000011");
            Assert.AreEqual(18, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO6] Tests that Sub works with multiple positive and negative numbers.
         */
        [Test]
        public void SubMixed()
        {
            STACK.Run("PUSH", "0010111111100101");
            STACK.Run("PUSH", "0010110001001001");
            STACK.Run("PUSH", "0010110001010001");
            SOPER.Run("SUB", "1000100000000011");
            Assert.AreEqual(35, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO7] Tests that Mul works with multiple positive numbers.
         */
        [Test]
        public void MulPos()
        {
            STACK.Run("PUSH", "0010110000000101");
            STACK.Run("PUSH", "0010110000001001");
            STACK.Run("PUSH", "0010110000010001");
            SOPER.Run("MUL", "1001000000000011");
            Assert.AreEqual(765, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO8] Tests that Mul works with multiple negative numbers.
         */
        [Test]
        public void MulNeg()
        {
            STACK.Run("PUSH", "0010111111110000");
            STACK.Run("PUSH", "0010111111110001");
            STACK.Run("PUSH", "0010111111110011");
            SOPER.Run("MUL", "100100000000011");
            Assert.AreEqual(62416, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO9] Tests that Mul works with multiple positive and negative numbers.
         */
        [Test]
        public void MulMixed()
        {
            STACK.Run("PUSH", "0010111111110101");
            STACK.Run("PUSH", "0010110000001001");
            STACK.Run("PUSH", "0010110000010001");
            SOPER.Run("MUL", "1001000000000011");
            Assert.AreEqual(63853, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO10] Tests that Div works with multiple positive numbers.
         */
        [Test]
        public void DivPos()
        {
            STACK.Run("PUSH", "0010110000000101");
            STACK.Run("PUSH", "0010110000001001");
            STACK.Run("PUSH", "0010110000010001");
            SOPER.Run("Div", "1001100000000011");
            Assert.AreEqual(17, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO11] Tests that Div works with multiple negative numbers.
         */
        [Test]
        public void DivNeg()
        {
            STACK.Run("PUSH", "0010111111110000");
            STACK.Run("PUSH", "0010111111110001");
            STACK.Run("PUSH", "0010111111110011");
            SOPER.Run("Div", "100110000000011");
            Assert.AreEqual(65523, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO12] Tests that Div works with multiple positive and negative numbers.
         */
        [Test]
        public void DivMixed()
        {
            STACK.Run("PUSH", "0010111111100101");
            STACK.Run("PUSH", "0010110001001001");
            STACK.Run("PUSH", "0010110001010001");
            SOPER.Run("DIV", "1001100000000011");
            Assert.AreEqual(0, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO13] Tests that Or works with multiple positive numbers.
         */
        [Test]
        public void OrPos()
        {
            STACK.Run("PUSH", "0010110000000101");
            STACK.Run("PUSH", "0010110000001001");
            STACK.Run("PUSH", "0010110000010001");
            SOPER.Run("OR", "1010000000000011");
            Assert.AreEqual(29, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO14] Tests that Or works with multiple negative numbers.
         */
        [Test]
        public void OrNeg()
        {
            STACK.Run("PUSH", "0010111111110000");
            STACK.Run("PUSH", "0010111111110001");
            STACK.Run("PUSH", "0010111111110011");
            SOPER.Run("OR", "101000000000011");
            Assert.AreEqual(65523, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO15] Tests that Or works with multiple positive and negative numbers.
         */
        [Test]
        public void OrMixed()
        {
            STACK.Run("PUSH", "0010111111100101");
            STACK.Run("PUSH", "0010110001001001");
            STACK.Run("PUSH", "0010110001010001");
            SOPER.Run("OR", "1010000000000011");
            Assert.AreEqual(65533, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO16] Tests that And works with multiple positive numbers.
         */
        [Test]
        public void AndPos()
        {
            STACK.Run("PUSH", "0010110000000101");
            STACK.Run("PUSH", "0010110000001001");
            STACK.Run("PUSH", "0010110000010001");
            SOPER.Run("AND", "1010100000000011");
            Assert.AreEqual(1, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO17] Tests that And works with multiple negative numbers.
         */
        [Test]
        public void AndNeg()
        {
            STACK.Run("PUSH", "0010111111110000");
            STACK.Run("PUSH", "0010111111110001");
            STACK.Run("PUSH", "0010111111110011");
            SOPER.Run("AND", "101010000000011");
            Assert.AreEqual(65520, Memory.GetInstance().GetDataStack()[0]);
        }

        /**
         * [SO18] Tests that And works with multiple positive and negative numbers.
         */
        [Test]
        public void AndMixed()
        {
            STACK.Run("PUSH", "0010111111100101");
            STACK.Run("PUSH", "0010110001001001");
            STACK.Run("PUSH", "0010110001010001");
            SOPER.Run("AND", "1010100000000011");
            Assert.AreEqual(65, Memory.GetInstance().GetDataStack()[0]);
        }
    }
}
