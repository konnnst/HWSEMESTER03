using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace THREAD
{
    static class ListStatsExtension
    {
        /// <summary>
        /// Counts median of list
        /// </summary>
        /// <param name="numbers">List of double numbers</param>
        /// <returns>Median of list</returns>
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

        /// <summary>
        /// Counts standart deviation of list elements
        /// </summary>
        /// <param name="numbers">List of double numbers</param>
        /// <returns>Standart deviation of list elements</returns>
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
        /// <summary>
        /// Clears files in .exe directory by format string with one iterator 
        /// Example: "matrix_{iterator}.txt"
        /// </summary>
        /// <param name="fString"></param>
        public static void ClearByFstring(string fString)
        {
            int iterator = 0;

            while (!File.Exists($"{Constants.CurrentFolder}\\{String.Format(fString, iterator)}") && iterator < 100)
                ++iterator;

            while (File.Exists($"{Constants.CurrentFolder}\\{String.Format(fString, iterator)}"));
                File.Delete(String.Format(fString, iterator++));
        }
    }
    class MultBenchmark
    {
        /// <summary>
        /// Creates benchmark object
        /// </summary>
        public MultBenchmark() { }

        /// <summary>
        /// Writes comparision stats for single and multi threads
        /// matrix multiplication for matrices size up to 2^sizeLogmax
        /// </summary>
        /// <param name="threadCount">Threads count in multithreading mode</param>
        /// <param name="sizeLogMax">Log_2 of max size matrix if stats row</param>
        public void StatsTable(int threadCount, int sizeLogMax)
        {
            var result = new StreamWriter(Constants.ResultPath);
            result.WriteLine("Executed on Asus D509D (AMD Ryzen 3 3200U, 2 core, 4 threads)");
            result.WriteLine("|Mode|Size|Mean|Median|St.dev.|Boost|");
            result.WriteLine("|-|-|-|-|-|-|");
            for (int sizeLog = 0; sizeLog <= sizeLogMax; ++sizeLog)
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
                Console.WriteLine("Worked on {0}x{1} matrix", size, size);
            }

            result.Close();
        }
        private double TestSingleThread(Matrix a, Matrix b)
        {
            var timer = new Stopwatch();

            timer.Start();
            Matrix.Mult(a, b);
            timer.Stop();

            return Convert.ToDouble(timer.ElapsedMilliseconds);
        }
        private double TestMultiThread(Matrix a, Matrix b, int threadCount)
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
        /// <summary>
        /// Creates matrix with random elements by given height and width
        /// </summary>
        /// <param name="h">Matrix height</param>
        /// <param name="w">Matrix width</param>
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
        /// <summary>
        /// Reads matrix from given file
        /// </summary>
        /// <param name="fileName">Name of file to read from /bin project folder</param>
        /// <exception cref="FormatException"></exception>
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
        /// <summary>
        /// Creates matrix by given 2d array
        /// </summary>
        /// <param name="newMatrix">2d array of mattrix coefficients</param>
        public Matrix(int[,] newMatrix)
        {
            this.matrix = newMatrix;
            this.Height = matrix.GetLength(0);
            this.Width = matrix.GetLength(1);
        }

        #endregion

        #region output
        /// <summary>
        /// Returns matrix width
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            return this.Width;
        }

        /// <summary>
        /// Returns matrix height
        /// </summary>
        /// <returns></returns>
        public int GetHeight()
        {
            return this.Height;
        }

        /// <summary>
        /// Saves matrix to file in directory, where .exe located
        /// with name "matrix_{index}.txt" with first free index
        /// </summary>
        public void Save()
        {
            int i = -1;
            while (File.Exists(String.Format(Constants.MatrixPath, ++i))) { }
            this.Save(i);
        }

        /// <summary>
        /// Saves matrix to file in directory, where .exe located
        /// with name "matrix_{index}.txt", if not matrix with this
        /// index not exists
        /// </summary>
        /// <param name="fileIndex">index in matrix file name</param>
        public void Save(int fileIndex)
        {
            int maxIndex = 0;
            while (File.Exists(String.Format(Constants.MatrixPath, ++maxIndex))) { }
            if (maxIndex > fileIndex)
            {
                Console.WriteLine("File with this index already exists");
                return;
            }

            StreamWriter writer = new StreamWriter(String.Format(Constants.MatrixPath, fileIndex));
            for (int i = 0; i < this.Height; ++i)
            {
                for (int k = 0; k < this.Width; ++k)
                    writer.Write(matrix[i, k] + " ");
                writer.Write("\n");
            }
            writer.Write("{0} {1}", this.Height, this.Width);
            writer.Close();
        }

        /// <summary>
        /// Prints matrix in console
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < this.Height; ++i)
            {
                for (int k = 0; k < this.Width; ++k)
                    Console.Write("{0} ", this.matrix[i, k].ToString().PadLeft(4));
                Console.WriteLine();
            }
        }

        #endregion

        #region operations
        /// <summary>
        /// Multiplies two matrices taken from files and writes to file in
        /// folder with .exe
        /// </summary>
        /// <param name="f1">First matrix file name</param>
        /// <param name="f2">Second matix file name</param>
        /// <param name="threadCount">Count of threads for multiplication</param>
        static public void FileMultThread(string f1, string f2, int threadCount)
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

        /// <summary>
        /// Multiplies two matrices taken from files and
        /// writes result in folder with .exe file
        /// </summary>
        /// <param name="f1">First matrix</param>
        /// <param name="f2">Second matrix</param>
        static public void FileMult(string f1, string f2)
        {
            if (!File.Exists(f1) || !File.Exists(f2))
            {
                Console.WriteLine("Files not exits");
                return;
            }
            var a = new Matrix(f1);
            var b = new Matrix(f2);

            var c = Mult(a, b);

            c.Save();
        }

        /// <summary>
        /// Checks if two matrices equal
        /// </summary>
        /// <param name="a">First matrix</param>
        /// <param name="b">Second matrix</param>
        /// <returns>True if equal, else false</returns>
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

        /// <summary>
        /// Multiplies two matrices in single thread mode
        /// </summary>
        /// <param name="a">First matrix</param>
        /// <param name="b">Second matrix</param>
        /// <returns>Multiplication result</returns>
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

        /// <summary>
        /// Multiplies two matrices in multithreading mode
        /// </summary>
        /// <param name="a">First matrix</param>
        /// <param name="b">Second matrix</param>
        /// <param name="threadCount">Count of using threads</param>
        /// <returns>Multiplication result</returns>
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
            var bench = new MultBenchmark();
            bench.StatsTable(12, 13);
        }
    }
}
