using System.Runtime.InteropServices;

namespace MyLazy;

public interface ILazy<T> { T? Get(); }

public class LazySingleThread<T> : ILazy<T>
{
    private bool IsCalculated = false;
    private T? Result;
    private Func<T> Supplier;

    public LazySingleThread(Func<T> supplier)
    {
        Supplier = supplier;
    }

    public T? Get()
    {
        if (!IsCalculated)
            Result = Supplier();
        IsCalculated = true;
        return Result;
    }
}

public class LazyMultiThread<T> //: ILazy<T>
{
    private Object lockObject = new();
    private bool IsCalculated = false;
    private T? Result;
    private Func<T> Supplier;

    public LazyMultiThread(Func<T> supplier)
    {
        Supplier = supplier;
    }

    public T? Get()
    {
        if (IsCalculated)
            return Result;

        lock (lockObject)
        {
            if (Volatile.Read(ref IsCalculated))
                return Result;
            Result = Supplier();
            Volatile.Write(ref IsCalculated, true);
            return Result;
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
