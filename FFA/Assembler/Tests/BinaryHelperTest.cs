using System;
using NUnit.Framework;

namespace Assembler
{
    /**
     * List of tests designed specifically to check correct functionality of procedures in the BinaryHelper class.
     * See the test plan for more information.
     */
    [TestFixture]
    public class BinaryHelperTest
    {
        /**
         * [B1] Testing conversion of a negative number to its two's complement representation with 10 digits.
         */
        [Test]
        public void BHToTwosComp10Digits()
        {
            Assert.AreEqual(960, BinaryHelper.ConvertNumber(-64, 10));
        }

        /**
         * [B2] Testing conversion of a negative number to its two's complement representation with 16 digits.
         */
        [Test]
        public void BHToTwosComp16Digits()
        {
            Assert.AreEqual(65472, BinaryHelper.ConvertNumber(-64, 16));
        }

        /**
         * [B3] Testing conversion of a two's complement number to its negative value with 10 digits.
         */
        [Test]
        public void BHFromTwosComp10Digits()
        {
            Assert.AreEqual(-64, BinaryHelper.ConvertNumber(960, 10));
        }

        /**
         * [B4] Testing conversion of a two's complement number to its negative value with 8 digits.
         */
        [Test]
        public void BHFromTwosComp8Digits()
        {
            Assert.AreEqual(-64, BinaryHelper.ConvertNumber(192, 8));
        }

        /**
         * [B5] Testing conversion of the smallest number to its negative two's complement representation with max digits (16).
         */
        [Test]
        public void BHToTwosComp16DigitsMin()
        {
            Assert.AreEqual(32768, BinaryHelper.ConvertNumber(-32768, 16));
        }

        /**
         * [B6] Testing conversion of a smallest two's complement to its negative corresponding value.
         */
        [Test]
        public void BHFromTwosComp16DigitsMin()
        {
            Assert.AreEqual(-32768, BinaryHelper.ConvertNumber(32768, 16));
        }

        /**
         * [B7] Testing conversion of the largest number to its negative two's complement representation with max digits (16).
         */
        [Test]
        public void BHToTwosComp16DigitsMax()
        {
            Assert.AreEqual(65535, BinaryHelper.ConvertNumber(-1, 16));
        }

        /**
         * [B8] Testing conversion of the largest two's complement to its negative corresponding value.
         */
        [Test]
        public void BHFromTwosComp16DigitsMax()
        {
            Assert.AreEqual(-1, BinaryHelper.ConvertNumber(65535, 16));
        }

        /**
         * [B9] Testing conversion of the largest two's complement (2 bits) to its negative corresponding value.
         */
        [Test]
        public void BHToTwosComp2DigitsMax()
        {
            Assert.AreEqual(-1, BinaryHelper.ConvertNumber(3, 2));
        }

        /**
         * [B10] Testing conversion of the smallest two's complement (2 bits) to its negative corresponding value.
         */
        [Test]
        public void BHToTwosComp2DigitsMin()
        {
            Assert.AreEqual(-2, BinaryHelper.ConvertNumber(2, 2));
        }

        /**
         * [B11] Testing conversion of the largest negative number (2 bits) to its two's complement representation.
         */
        [Test]
        public void BHFromTwosComp2DigitsMin()
        {
            Assert.AreEqual(3, BinaryHelper.ConvertNumber(-1, 2));
        }

        /**
         * [B12] Testing that providing a number above the range returns 0.
         */
        [Test]
        public void BHAboveRange()
        {
            Assert.AreEqual(0, BinaryHelper.ConvertNumber(10000, 10));
        }

        /**
         * [B13] Testing that providing a number below the range returns 0.
         */
        [Test]
        public void BHBelowRange()
        {
            Assert.AreEqual(0, BinaryHelper.ConvertNumber(-10000, 10));
        }

        /**
         * [B14] Testing when the given number is 0.
         */
        [Test]
        public void BHIs0()
        {
            Assert.AreEqual(0, BinaryHelper.ConvertNumber(0, 10));
        }

        /**
         * [B15] Testing when the given number is a positive number that will not change regardless of which notation it was it.
         */
        [Test]
        public void BHNoValueChange()
        {
            Assert.AreEqual(100, BinaryHelper.ConvertNumber(100, 10));
        }

        /**
         * [B16] Checking an arbitrary value with IsInRange that should return true.
         */
        [Test]
        public void BHInRange()
        {
            Assert.IsTrue(BinaryHelper.IsInRange(427, 10));
        }

        /**
         * [B17] Checking the largest value that is still in range.
         */
        [Test]
        public void BHInRangeMax()
        {
            Assert.IsTrue(BinaryHelper.IsInRange(1023, 10));
        }

        /**
         * [B18] Checking the smallest value that is still in range.
         */
        [Test]
        public void BHInRangeMin()
        {
            Assert.IsTrue(BinaryHelper.IsInRange(-512, 10));
        }

        /**
         * [B19] Checking a value that is above the range.
         */
        [Test]
        public void BHOutOfRangeAbove()
        {
            Assert.IsFalse(BinaryHelper.IsInRange(1024, 10));
        }

        /**
         * [B20] Checking a value that is below the range.
         */
        [Test]
        public void BHOutOfRangeBelow()
        {
            Assert.IsFalse(BinaryHelper.IsInRange(-513, 10));
        }

        /**
         * [B21] Test converting a hex number into binary.
         */
        [Test]
        public void BHConvertBinaryString()
        {
            Assert.AreEqual("10101010", BinaryHelper.BinaryString("AA"));
        }

        /**
         * [B22] Test converting a negative 5-bit hex into an integer
         */
        [Test]
        public void BHHexToInt5Bit()
        {
            Assert.AreEqual(-6, BinaryHelper.HexToInt("1A", 5));
        }

        /**
         * [B23] Test converting a negative 5-bit hex into an integer
         */
        [Test]
        public void BHHexToInt10Bit()
        {
            Assert.AreEqual(-511, BinaryHelper.HexToInt("201", 10));
        }

        /**
         * [B24] Test converting a negative 5-bit hex into an integer
         */
        [Test]
        public void BHHexToIntPositive()
        {
            Assert.AreEqual(511, BinaryHelper.HexToInt("1ff", 10));
        }
    }
}
