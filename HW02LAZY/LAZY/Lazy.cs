using System.Runtime.InteropServices;

namespace MyLazy;

public interface ILazy<T> {
    T? Get();
 }

/// <summary>
/// Thread non-safe realisation of built-in Lazy class
/// </summary>
/// <typeparam name="T">Type of supplier function returning value</typeparam>
public class LazySingleThread<T> : ILazy<T>
{
    private bool _isCalculated = false;
    private T? _result;
    private Func<T> _supplier;

    /// <summary>
    /// Creates object of this class
    /// </summary>
    /// <param name="supplier">Function, providing calculation</param>
    public LazySingleThread(Func<T> supplier)
    {
        _supplier = supplier;
    }

    /// <summary>
    /// Calls for lazy initialised object. First time runs calculation
    /// via supplier function. Following times returns precalculated value
    /// </summary>
    /// <returns>Result of running supplier function</returns>
    public T? Get()
    {
        if (!_isCalculated) {
            _result = _supplier();
        }
        _isCalculated = true;
        return _result;
    }
}

/// <summary>
/// Thread safe realisation of built-in Lazy class
/// </summary>
/// <typeparam name="T">Type of supplier function returning value</typeparam>
public class LazyMultiThread<T> : ILazy<T>
{
    private Object _lockObject = new();
    private bool _isCalculated = false;
    private T? _result;
    private Func<T> _supplier;

    /// <summary>
    /// Creates object of this class
    /// </summary>
    /// <param name="supplier">Function, providing calculation</param>
    public LazyMultiThread(Func<T> supplier)
    {
        _supplier = supplier;
    }

    /// <summary>
    /// Calls for lazy initialised object. First time runs calculation
    /// via supplier function. Following times returns precalculated value
    /// </summary>
    /// <returns>Result of running supplier function</returns>
    public T? Get()
    {
        if (Volatile.Read(ref _isCalculated)) {
            return _result;
        }

        lock (_lockObject)
        {
            if (Volatile.Read(ref _isCalculated))
                return _result;
            _result = _supplier();
            Volatile.Write(ref _isCalculated, true);
            return _result;
        }
    }
}

internal class MyLazy
{
    static List<int> GenerateList()
    {
        var l = new List<int>();
        var randomizer = new Random();

        for (int i = 0; i < 10000000; ++i)
            l.Add(randomizer.Next(i));
        l.Sort();

        return l;
    }

    static void Main()
    {
        var n = 5;
        var threads = new Thread[n];
        var lazy = new LazyMultiThread<List<int>>(GenerateList);

        for (int i = 0; i < n; ++i)
        {
            threads[i] = new Thread(() => {
                var l = lazy.Get();
                Console.WriteLine("Now in {0} thread", Thread.CurrentThread.Name);
                });
        }

        for (int i = 0; i < n; ++i)
            threads[i].Start();

        for (int i = 0; i < n; ++i)
            threads[i].Join();

        Console.ReadKey();
    }


}
