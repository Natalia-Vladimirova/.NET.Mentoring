using Subtask2.ObjectPool.Interfaces;

namespace Subtask2.ObjectPool
{
    public class HeavyObjectCreator : IPoolObjectCreator<HeavyObject>
    {
        public HeavyObject Create()
        {
            return new HeavyObject();
        }
    }
}
