using System.Collections.Generic;

namespace CachingSolutionsSamples.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
    }
}
