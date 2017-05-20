using System.Data;

namespace Northwind.DAL.Interfaces
{
    public interface IMapper
    {
        T Map<T>(IDataReader reader) where T : new();
    }
}
