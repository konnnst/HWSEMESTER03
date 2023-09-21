using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.IsTrue(Matrix.Compare(Matrix.Mult(LeftMatrix, RightMatrix), ResultMatrix));
        }

        [TestMethod]
        public void MultThreadTest()
        {
            Assert.IsTrue(Matrix.Compare(Matrix.MultThread(LeftMatrix, RightMatrix, 8), ResultMatrix));
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

            Assert.IsTrue(Matrix.Compare(Matrix.Mult(left1, right1), Matrix.MultThread(left1, right1, 8)));
            Assert.IsTrue(Matrix.Compare(Matrix.Mult(left2, right2), Matrix.MultThread(left2, right2, 8)));
            Assert.IsTrue(Matrix.Compare(Matrix.Mult(left3, right3), Matrix.MultThread(left3, right3, 8)));
            Assert.IsTrue(Matrix.Compare(Matrix.Mult(left4, right4), Matrix.MultThread(left4, right4, 8)));
            Assert.IsTrue(Matrix.Compare(Matrix.Mult(left5, right5), Matrix.MultThread(left5, right5, 8)));
        }
    }
}
