using System.Diagnostics;
using System.Xml.Schema;
using MyLazy;

namespace LazyTests;


/// <summary>
/// Counts calculation attempts from all working threads
/// </summary>
public class Counter
{
    private int _counterValue = 0;
    public int CounterValue => _counterValue;
    public int Calculation()
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
        var counter = new Counter();
        var iterations = 20;
        var lazyCalculator = new LazySingleThread<int>(counter.Calculation);

        for (var i = 0; i < iterations; ++i)
            lazyCalculator.Get();

        Assert.IsTrue(lazyCalculator.Get() == 1);        
        Assert.IsTrue(counter.CounterValue == 1);
    }
}

[TestClass]
public class LazyMultiThreadTests
{
    [TestMethod]
    public void GetTest()
    {
        var threadCount = 20;
        var threadIterations = 20;
        var threads = new Thread[threadCount];
        var counter = new Counter();
        var lazyCalculator = new LazyMultiThread<int>(counter.Calculation);
        
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

        Assert.IsTrue(lazyCalculator.Get() == 1);
        Assert.IsTrue(counter.CounterValue == 1);
    }
}


