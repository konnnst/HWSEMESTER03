using System.Diagnostics;
using System.Xml.Schema;
using MyLazy;

namespace LazyTests;

public class Counter
{
    private static int _counterValue = 0;
    public static int CounterValue => _counterValue;
    public static int Calculation()
    {
        return ++_counterValue;
    }
}

[TestClass]
public class LazySingleThreadTests
{
    [TestMethod]
    public void GetTest()
    {
        var iterations = 20;
        var lazyCalculator = new LazySingleThread<int>(Counter.Calculation);

        for (var i = 0; i < iterations; ++i)
            lazyCalculator.Get();
        
        Assert.IsTrue(Counter.CounterValue == 1);
    }
}

[TestClass]
public class LazyMultiThreadTests
{
    [TestMethod]
    public void GetTest()
    {
        var threadCount = 20;
        var threadIterations = 10;
        var threads = new Thread[threadCount];
        var lazyCalculator = new LazyMultiThread<int>(Counter.Calculation);
        
        for (var i = 0; i < threadCount; ++i)
        {
            threads[i] = new Thread(() => {
                for (var i = 0;  i < threadIterations; ++i) {
                    lazyCalculator.Get();
                }});
        }

        for (var i = 0; i < threadCount; ++i) {
            threads[i].Start();
        }

        for (var i = 0; i < threadCount; ++i) {
            threads[i].Join();
        }
        Assert.IsTrue(Counter.CounterValue == 1);
    }
}


