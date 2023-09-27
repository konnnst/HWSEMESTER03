using System.Diagnostics;
using System.Xml.Schema;
using MyLazy;

namespace LazyTests;

internal class Counter<T>
{
    private volatile int callsCount;
    public int CallsCount
    {
        get => this.callsCount;
        private set => this.callsCount = value;
    }
    private Func<T> func;

    public Counter(Func<T> func)
    {
        this.CallsCount = 0;
        this.func = func;
    }

    public T Call()
    {
        this.CallsCount++;
        return func();
    }
}

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
        var stopwatch = new Stopwatch();
        var lazyControl = new LazySingleThread<List<int>>(Operations.RandomListCreateSort);

        stopwatch.Start();
        lazyControl.Get();
        stopwatch.Stop();
        var oneTime = stopwatch.ElapsedMilliseconds;
        stopwatch.Reset();

        stopwatch.Start();
        for (int i = 0; i < threadCount; ++i)
            threads[i] = new Thread(() => lazy.Get());

        for (int i = 0; i < threadCount; ++i)
            threads[i].Start();

        for (int i = 0; i < threadCount; ++i)
            threads[i].Join();
        stopwatch.Stop();
        var totalTime = stopwatch.ElapsedMilliseconds;

        Assert.IsTrue(totalTime * 3 < 2 * oneTime);
    }

}


