using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThread;

namespace MultiThread.Tests
{
    [TestClass]
    public class MatrixTests
    {
        static int[,] LeftMatrixCoefficients = { { 2, 1 }, { 0, 1 } };
        static Matrix LeftMatrix = new Matrix(LeftMatrixCoefficients);
        static int[,] RightMatrixCoefficients = { { 3, 1 }, { 2, 1 } };
        static Matrix RightMatrix = new Matrix(RightMatrixCoefficients);
        static int[,] ResultMatrixCoefficients = { { 8, 3 }, { 2, 1 } };
        static Matrix ResultMatrix = new Matrix(ResultMatrixCoefficients);


        [TestMethod]
        public void MultTest()
        {
            Assert.IsTrue(Matrix.Compare(Matrix.Multiply(LeftMatrix, RightMatrix), ResultMatrix));
        }

        [TestMethod]
        public void MultThreadTest()
        {
            Assert.IsTrue(Matrix.Compare(Matrix.MultiThreadMultiply(LeftMatrix, RightMatrix, 8), ResultMatrix));
        }

        [TestMethod]
        public void IsThreadCorrect()
        {
            var left1 = new Matrix(4, 1);
            var right1 = new Matrix(1, 4);
            var left2 = new Matrix(1000, 1);
            var right2 = new Matrix(1, 1500);
            var left3 = new Matrix(4, 2500);
            var right3 = new Matrix(2500, 15);
            var left4 = new Matrix(100, 100);
            var right4 = new Matrix(100, 100);
            var left5 = new Matrix(1, 1);
            var right5 = new Matrix(1, 1);

            Assert.IsTrue(Matrix.Compare(Matrix.Multiply(left1, right1), Matrix.MultiThreadMultiply(left1, right1, 8)));
            Assert.IsTrue(Matrix.Compare(Matrix.Multiply(left2, right2), Matrix.MultiThreadMultiply(left2, right2, 8)));
            Assert.IsTrue(Matrix.Compare(Matrix.Multiply(left3, right3), Matrix.MultiThreadMultiply(left3, right3, 8)));
            Assert.IsTrue(Matrix.Compare(Matrix.Multiply(left4, right4), Matrix.MultiThreadMultiply(left4, right4, 8)));
            Assert.IsTrue(Matrix.Compare(Matrix.Multiply(left5, right5), Matrix.MultiThreadMultiply(left5, right5, 8)));
        }

        [TestMethod]
        public void NullConstructorTest()
        {
            var nullHeightMatrix = new Matrix(0, 5);
            var nullWidthMatrix = new Matrix(5, 0);

            Assert.IsTrue(nullHeightMatrix.IsNull());
            Assert.IsTrue(nullWidthMatrix.IsNull());
        }

        [TestMethod]
        public void NullMultiplicationTest()
        {
            var nullHeightMatrix = new Matrix(0, 5);
            var notNullMatrix = new Matrix(5, 5);
            var nullWidthMatrix = new Matrix(5, 0);

            var notNullSingleThread = Matrix.Multiply(notNullMatrix, notNullMatrix);
            var notNullMultiThread = Matrix.MultiThreadMultiply(notNullMatrix, notNullMatrix, 4);

            var nullHeightSingleThread = Matrix.Multiply(nullHeightMatrix, notNullMatrix);
            var nullHeightMultiThread = Matrix.MultiThreadMultiply(nullHeightMatrix, notNullMatrix, 4);

            var nullWidthSingleThread = Matrix.Multiply(notNullMatrix, nullHeightMatrix);
            var nullWidthMultiThread = Matrix.MultiThreadMultiply(notNullMatrix, nullWidthMatrix, 4);

            var nullBothSingleThread = Matrix.Multiply(nullHeightMatrix, nullWidthMatrix);
            var nullBothMultiThread = Matrix.MultiThreadMultiply(nullHeightMatrix, nullWidthMatrix, 4);

            Assert.IsFalse(notNullSingleThread.IsNull());
            Assert.IsFalse(notNullMultiThread.IsNull());

            Assert.IsTrue(nullHeightSingleThread.IsNull());
            Assert.IsTrue(nullHeightMultiThread.IsNull());

            Assert.IsTrue(nullWidthSingleThread.IsNull());
            Assert.IsTrue(nullWidthMultiThread.IsNull());

            Assert.IsTrue(nullBothSingleThread.IsNull());
            Assert.IsTrue(nullBothMultiThread.IsNull());
        }
    }
}