using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using THREAD;

namespace THREAD.Tests
{
    [TestClass]
    public class MatrixTests
    {
        static int[,] aMatr = { { 2, 1 }, { 0, 1 } };
        static Matrix a = new Matrix(aMatr);
        static int[,] bMatr = { { 3, 1 }, { 2, 1 } };
        static Matrix b = new Matrix(bMatr);
        static int[,] cMatr = { { 8, 3 }, { 2, 1 } };
        static Matrix c = new Matrix(cMatr);


        [TestMethod]
        public void MultTest()
        {
            Assert.IsTrue(Matrix.Compare(Matrix.Mult(a, b), c));
        }

        [TestMethod]
        public void MultThreadTest()
        {
            Assert.IsTrue(Matrix.Compare(Matrix.MultThread(a, b, 8), c));
        }

        [TestMethod]
        public void IsThreadCorrect()
        {
            var l1 = new Matrix(4, 1);
            var r1 = new Matrix(1, 4);
            var l2 = new Matrix(1000, 1);
            var r2 = new Matrix(1, 1500);
            var l3 = new Matrix(4, 2500);
            var r3 = new Matrix(2500, 15);
            var l4 = new Matrix(100, 100);
            var r4 = new Matrix(100, 100);
            var l5 = new Matrix(1, 1);
            var r5 = new Matrix(1, 1);

            Assert.IsTrue(Matrix.Compare(Matrix.Mult(l1, r1), Matrix.MultThread(l1, r1, 8)));
            Assert.IsTrue(Matrix.Compare(Matrix.Mult(l2, r2), Matrix.MultThread(l2, r2, 8)));
            Assert.IsTrue(Matrix.Compare(Matrix.Mult(l3, r3), Matrix.MultThread(l3, r3, 8)));
            Assert.IsTrue(Matrix.Compare(Matrix.Mult(l4, r4), Matrix.MultThread(l4, r4, 8)));
            Assert.IsTrue(Matrix.Compare(Matrix.Mult(l5, r5), Matrix.MultThread(l5, r5, 8)));
        }
    }
}
