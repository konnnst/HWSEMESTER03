using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace P09THREAD
{
    static class ListStatsExtension
    {
        public static double Median(this List<double> numbers)
        {
            if (numbers.Count == 0)
                return 0;

            numbers = numbers.OrderBy(n => n).ToList();

            var halfIndex = numbers.Count() / 2;

            if (numbers.Count() % 2 == 0)
                return (numbers[halfIndex] + numbers[halfIndex - 1]) / 2.0;

            return numbers[halfIndex];
        }

        public static double StandardDeviation(this List<double> numbers)
        {
            if (numbers.Count == 0)
                return 0;

            var avg = numbers.Average();
            var sum = numbers.Sum(d => Math.Pow(d - avg, 2));
            return Math.Sqrt(sum / (numbers.Count() - 1));
        }
    }
    class Constants
    {
        public static string CurrentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
        public static string MatrixPath = CurrentFolder + "matrix_{0}.txt";
        public static string ResultPath = CurrentFolder + "README.md";
    }
    class FileCleaner
    {
        public static void ClearByFstring(string fString)
        {
            int iterator = 0;

            while (File.Exists($"{Constants.CurrentFolder}\\{String.Format(fString, iterator)}"));
                File.Delete(String.Format(fString, iterator++));
        }
    }
    class MultBenchmark
    {
        public MultBenchmark() { }
        public void StatsTable(int threadCount, int sizeLogMax)
        {
            var result = new StreamWriter(Constants.ResultPath);
            result.WriteLine("Executed on Asus D509D (AMD Ryzen 3 3200U, 2 core, 4 threads)");
            result.WriteLine("|Mode|Size|Mean|Median|St.dev.|Boost|");
            result.WriteLine("|-|-|-|-|-|-|");
            for (int sizeLog = 0; sizeLog < sizeLogMax; ++sizeLog)
            {
                int size = Convert.ToInt32(Math.Pow(2, sizeLog));
                var single = new List<double>();
                var multi = new List<double>();
                for (int i = 0; i < 8; ++i)
                {
                    var a = new Matrix(size, size);
                    var b = new Matrix(size, size);
                    single.Add(TestSingleThread(a, b));
                    multi.Add(TestMultiThread(a, b, threadCount));
                }
                result.WriteLine("|Single|{0}|{1} ms|{2} ms|{3} ms||",
                    size, single.Average(), single.Median(), single.StandardDeviation());
                result.WriteLine("|Multi|{0}|{1} ms|{2} ms|{3} ms|{4} times|",
                    size, multi.Average(), multi.Median(), multi.StandardDeviation(), single.Average() / multi.Average());
                result.Flush();
            }

            result.Close();
        }
        public double TestSingleThread(Matrix a, Matrix b)
        {
            var timer = new Stopwatch();

            timer.Start();
            Matrix.Mult(a, b);
            timer.Stop();

            return Convert.ToDouble(timer.ElapsedMilliseconds);
        }
        public double TestMultiThread(Matrix a, Matrix b, int threadCount)
        {
            var timer = new Stopwatch();

            timer.Start();
            Matrix.MultThread(a, b, threadCount);
            timer.Stop();

            return Convert.ToDouble(timer.ElapsedMilliseconds);
        }
    }
    public class Matrix
    {
        #region parameters
        int[,] matrix;
        int Width { get; set; }
        int Height { get; set; }
        #endregion

        #region constructor
        public Matrix(int h, int w)
        {
            this.matrix = new int[h, w];
            this.Width = w; this.Height = h;

            Random random = new Random();
            for (int i = 0; i < h; ++i)
            {
                for (int k = 0; k < w; ++k)
                    matrix[i, k] = random.Next(-5, 5);
            }
        }
        public Matrix(string fileName)
        {
            if (!File.Exists(Constants.CurrentFolder + fileName))
            {
                Console.WriteLine("File not exists");
                return;
            }
            StreamReader reader = new StreamReader(Constants.CurrentFolder + fileName);
            int w;
            string line;
            List<int[]> matrix = new List<int[]>();

            if ((line = reader.ReadLine()) == null)
            {
                this.Height = 0; this.Width = 0;
                this.matrix = new int[0, 0];
                return;
            }

            w = line.Split().Length;

            while (line != null)
            {
                var rowNums = line.Split();
                if (rowNums.Length != w)
                    throw new FormatException("Incorrect matrix size");
                var row = new int[w];
                for (int i = 0; i < w; ++i)
                {
                    if (!Int32.TryParse(rowNums[i], out row[i]))
                        throw new FormatException("Incorrect element type");
                }
                matrix.Add(row);
                line = reader.ReadLine();
            }
            this.Height = matrix.Count; this.Width = w;
            this.matrix = new int[this.Height, this.Width];

            for (int i = 0; i < this.Height; ++i)
            {
                for (int k = 0; k < this.Width; ++k)
                    this.matrix[i, k] = matrix[i][k];
            }
        }
        public Matrix(int[,] newMatrix)
        {
            this.matrix = newMatrix;
            this.Height = matrix.GetLength(0);
            this.Width = matrix.GetLength(1);
        }

        #endregion

        #region output
        public int GetWidth()
        {
            return this.Width;
        }
        public int GetHeight()
        {
            return this.Height;
        }

        public void Save()
        {
            int i = -1;
            while (File.Exists(String.Format(Constants.MatrixPath, ++i))) { }
            this.Save(i);
        }
        public void Save(int FileIndex)
        {
            StreamWriter writer = new StreamWriter(String.Format(Constants.MatrixPath, FileIndex));
            for (int i = 0; i < this.Height; ++i)
            {
                for (int k = 0; k < this.Width; ++k)
                    writer.Write(matrix[i, k] + " ");
                writer.Write("\n");
            }
            writer.Write("{0} {1}", this.Height, this.Width);
            writer.Close();
        }
        public void Print()
        {
            for (int i = 0; i < this.Height; ++i)
            {
                for (int k = 0; k < this.Width; ++k)
                    Console.Write("{0} ", this.matrix[i, k].ToString().PadLeft(6));
                Console.WriteLine();
            }
        }

        #endregion

        #region operations
        static public void MultFiles(string f1, string f2, int threadCount)
        {
            if (!File.Exists(f1) || !File.Exists(f2))
            {
                Console.WriteLine("Files not exits");
                return;
            }
            var a = new Matrix(f1);
            var b = new Matrix(f2);

            var c = MultThread(a, b, threadCount);

            c.Save();
        }
        static public bool Compare(Matrix a, Matrix b)
        {
            if (a.Width != b.Width || a.Height != b.Height)
                return false;

            for (int i = 0; i < a.Height; ++i)
            {
                for (int j = 0; j < a.Width; ++j)
                {
                    if (a.matrix[i, j] != b.matrix[i, j])
                        return false;
                }
            }

            return true;
        }
        static public Matrix Mult(Matrix a, Matrix b)
        {
            if (a.Width != b.Height)
                return null;
            Matrix c = new Matrix(a.Height, b.Width);

            for (int i = 0; i < a.Height; ++i)
            {
                for (int j = 0; j < b.Width; ++j)
                {
                    c.matrix[i, j] = 0;
                    for (int k = 0; k < b.Height; ++k)
                        c.matrix[i, j] += a.matrix[i, k] * b.matrix[k, j];
                }
            }

            return c;
        }
        static public Matrix MultThread(Matrix a, Matrix b, int threadCount)
        {
            if (threadCount < 0 || threadCount > a.Height)
            {
                Console.WriteLine("Thread count clipped according to matrix size");
                threadCount = a.Height;
            }
            if (a.Width != b.Height)
            {
                Console.WriteLine("Incorrect matrix sizes");
                return null;
            }

            Matrix c = new Matrix(a.Height, b.Width);
            Thread[] threads = new Thread[threadCount];
            int threadPiece = a.Height / threadCount + Convert.ToInt32(a.Height % threadCount != 0);

            for (int i = 0; i < threadCount; ++i)
            {
                int localI = i;
                threads[i] = new Thread(() =>
                {
                    for (int n = threadPiece * localI; n < threadPiece * (localI + 1) && n < a.Height; ++n)
                    {
                        for (int j = 0; j < b.Width; ++j)
                        {
                            c.matrix[n, j] = 0;
                            for (int k = 0; k < a.Width; ++k)
                                c.matrix[n, j] += a.matrix[n, k] * b.matrix[k, j];
                        }
                    }
                });

            }

            for (int i = 0; i < threads.Length; ++i)
                threads[i].Start();

            for (int i = 0; i < threads.Length; ++i)
                threads[i].Join();

            return c;
        }
        #endregion
    }
    internal class Program
    {
        static void Main()
        {
            FileCleaner.ClearByFstring("matrix_{0}.txt");
            var a = new Matrix("1.txt");
            var b = new Matrix("2.txt");
            var aa = new Matrix(4, 3);
            var bb = new Matrix(3, 4);

            Matrix.Mult(aa, bb).Print();
            Matrix.MultThread(aa, bb, 8).Print();

            Console.ReadKey();
        }
    }
}
