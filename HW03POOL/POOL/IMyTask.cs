namespace MyPool;
public interface IMyTask<TResult>
{
    public bool IsCompleted { get; }
    public TResult? Result { get; }
}