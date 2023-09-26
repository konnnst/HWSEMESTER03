using System.Diagnostics;
using MyLazy;

namespace LazyTests;

public class Operations
{
    public static List<int> RandomListCreateSort()
    {
        var l = new List<int>();
        var size = 1000000;

        var random = new Random();
        for (int i = 0; i < size; ++i)
            l.Add(random.Next());

        return l;
    }
}

[TestClass]
public class LazySingleThreadTests
{
    private int GetExecutionTIme<T>(Func<T> function)
    {
        var time = new Stopwatch();

        time.Start();
        function();
        
        return (int)time.ElapsedMilliseconds;
    }

    [TestMethod]
    public void GetTest()
    {
        var lazy = new LazySingleThread<List<int>>(Operations.RandomListCreateSort);
        var iterationCount = 20;
        var firstExecutionTime = GetExecutionTIme<List<int>>(lazy.Get);

        for (int i = 0; i < iterationCount; ++i)
            Assert.IsTrue(firstExecutionTime > GetExecutionTIme<List<int>>(lazy.Get) * 20);
    }
}

[TestClass]
public class LazyMultiThreadTests
{
    [TestMethod]
    public void GetTest()
    {
        var lazy = new LazyMultiThread<List<int>>(Operations.RandomListCreateSort);
        var threadCount = Environment.ProcessorCount;
        var threads = new Thread[threadCount];

        for (int i = 0; i < threadCount; ++i)
            threads[i] = new Thread(() => lazy.Get());

        for (int i = 0; i < threadCount; ++i)
            threads[i].Start();

        for (int i = 0; i < threadCount; ++i)
            threads[i].Join();

        Assert.IsTrue(true == true);
    }
}