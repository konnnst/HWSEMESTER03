using System.Collections.Generic;

public class Task<TResult>
{
    private Func<TResult> Func;
    public bool IsCompleted = false;
    public TResult? Result;

    public Task(Func<TResult> func)
    {
        Func = func;
    }
}
public class MyThreadPool
{
    private Thread[] Threads;
    private Queue<int> FreeThreadQueue;
    private Queue<Task<object>>? TaskQueue;
    public MyThreadPool(int threadCount)
    {
        FreeThreadQueue = new Queue<int>();
        Threads = new Thread[threadCount];
        for (int i = 0; i < threadCount; ++i)
        {
            Threads[i] = new Thread(() => WaitForTask());
            FreeThreadQueue.Enqueue(i);
        }
    }

    private void WaitForTask()
    {
        while (true)
            Thread.Yield();
    }

}

internal class Program
{
    void Main()
    {

    }
}