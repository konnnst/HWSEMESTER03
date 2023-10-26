using System.Diagnostics;
using System.Xml.Schema;
using MyLazy;

namespace LazyTests;

/// <summary>
/// Fraction calculation. Throws exception
/// if deviding zero
/// </summary>
public class Fraction
{
    int _numerator;
    int _denominator;
    
    public Fraction(int numerator, int denominator)
    {
        _numerator = numerator;
        _denominator = denominator;
    }

    public double Calculate()
    {
        if (_denominator != 0)
        {
            return (double)_numerator / _denominator;
        }
        else
        {
            throw new Exception("Zero division achtung");
        }
    }
}
   
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
    public void ExceptionGetTest()
    {
        var successCalc = new Fraction(5, 2);
        var exceptionCalc = new Fraction(5, 0);
        var successLazy = new LazySingleThread<double>(successCalc.Calculate);
        var exceptionLazy = new LazySingleThread<double>(exceptionCalc.Calculate);

        Assert.IsTrue(successLazy.Get() == 2.5);
        Assert.IsTrue(exceptionLazy.Get() == default(double));
    }

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
    public void ExceptionGetTest()
    {
        var successCalc = new Fraction(5, 2);
        var exceptionCalc = new Fraction(5, 0);
        var successLazy = new LazyMultiThread<double>(successCalc.Calculate);
        var exceptionLazy = new LazyMultiThread<double>(exceptionCalc.Calculate);

        Assert.IsTrue(successLazy.Get() == 2.5);
        Assert.IsTrue(exceptionLazy.Get() == default(double));
    }
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


