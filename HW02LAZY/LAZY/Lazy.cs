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

public class Counter
{
    private static int _counterValue = 0;
    public static int CounterValue => _counterValue;
    public static int Calculation()
    {
        return ++_counterValue;
    }
}
internal class MyLazy
{
    static void Main()
    {
    }   
}
