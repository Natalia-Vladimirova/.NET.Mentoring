namespace Subtask2.ObjectPool.Interfaces
{
    public interface IPoolObjectCreator<T>
    {
        T Create();
    }
}
