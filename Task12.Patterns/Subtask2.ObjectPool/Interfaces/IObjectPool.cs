namespace Subtask2.ObjectPool.Interfaces
{
    public interface IObjectPool<T>
    {
        int Count { get; }

        T GetObject();

        void ReturnObject(ref T obj);
    }
}
