using System.Diagnostics;

namespace MultiThread;

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
            var size = Convert.ToInt32(Math.Pow(2, sizeLog));
            var single = new List<double>();
            var multi = new List<double>();
            for (var i = 0; i < 8; ++i)
            {
                var leftMatrix = new Matrix(size, size);
                var rightMatrix = new Matrix(size, size);
                single.Add(TestSingleThread(leftMatrix, rightMatrix));
                multi.Add(TestMultiThread(leftMatrix, rightMatrix, threadCount));
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
    private double TestSingleThread(Matrix leftMatrix, Matrix rightMatrix)
    {
        var timer = new Stopwatch();

        timer.Start();
        Matrix.Multiply(leftMatrix, rightMatrix);
        timer.Stop();

        return Convert.ToDouble(timer.ElapsedMilliseconds);
    }
    private double TestMultiThread(Matrix leftMatrix, Matrix rightMatrix, int threadCount)
    {
        var timer = new Stopwatch();

        timer.Start();
        Matrix.MultiThreadMultiply(leftMatrix, rightMatrix, threadCount);
        timer.Stop();

        return Convert.ToDouble(timer.ElapsedMilliseconds);
    }
}