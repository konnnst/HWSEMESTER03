using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

namespace MyPool;
public class MyTask<TResult> : IMyTask<TResult>, ICalculatable   
{
    private Func<TResult> Calculation;
    private bool isCompleted = false;
	private TResult? result;

    public bool IsCompleted => isCompleted;
    public TResult? Result
    {
        get
        {
            if (isCompleted)
                return result;
            while (!isCompleted)
                Thread.Yield(); 
            return result;
        }
    }
    public MyTask(Func<TResult> func)
    {
        Calculation = func;
    }

    public void CalculateTask()
    {
        result = Calculation();
        isCompleted = true;
    }
}
public class MyThreadPool
{
    private volatile bool cancellationToken = false;
    private Thread[] Threads;
    private ConcurrentQueue<Thread> FreeThreadQueue = new();
    private ConcurrentQueue<ICalculatable> TaskQueue = new();

    public MyThreadPool(int threadCount)
    {
        Threads = new Thread[threadCount];
        for (int i = 0; i < threadCount; ++i)
        {
            Threads[i] = new Thread(WaitForTask);
            Threads[i].Name = $"{i}";
            Threads[i].Start();
            FreeThreadQueue.Enqueue(Threads[i]);
        }
    }

    public void Shutdown()
    {
        cancellationToken = true;
    }

    public IMyTask<TResult> Submit<TResult>(Func<TResult> func)
    {
        if (cancellationToken)
            throw new Exception("Can't add task. Pool already finished");

        MyTask<TResult> task = new(func);
        TaskQueue.Enqueue(task);
        return task;
    }

    private void StartTask(ICalculatable task)
    {
        task.CalculateTask();
    }

    private void WaitForTask()
    {
        while (!cancellationToken)
        {
            if (TaskQueue.TryDequeue(out ICalculatable? task))
                StartTask(task);
        }
    }

}

internal class Program
{
    public static List<int> SortList()
    {
        int size = 1000000;
        return SortList(size);
    }
    public static List<int> SortList(int size)
    {
        List<int> list = new();

        Random random = new();
        for (int i = 0; i < size; ++i)
            list.Add(random.Next());
        
        list.Sort();
        return list;
    }

    static void Main()
    {
        var pool = new MyThreadPool(Environment.ProcessorCount);
        var task1 = pool.Submit<List<int>>(() => SortList(228));
        var task2 = pool.Submit<List<int>>(SortList);
        var task3 = pool.Submit<List<int>>(SortList);
        var task4 = pool.Submit<List<int>>(SortList);
        var task5 = pool.Submit<List<int>>(SortList);
        var task6 = pool.Submit<List<int>>(SortList);
        var task7 = pool.Submit<List<int>>(SortList);

        Console.WriteLine(task1.Result.Count);
        Console.WriteLine(task2.Result.Count);
        Console.WriteLine(task3.Result.Count);
        Console.WriteLine(task4.Result.Count);
        Console.WriteLine(task5.Result.Count);    
        Console.WriteLine(task6.Result.Count);
        Console.WriteLine(task7.Result.Count);

        Console.ReadKey();
    }
}
