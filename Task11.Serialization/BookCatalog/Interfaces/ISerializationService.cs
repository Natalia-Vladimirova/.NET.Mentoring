using BookCatalog.Models;

namespace BookCatalog.Interfaces
{
    public interface ISerializationService
    {
        void Serialize(Catalog catalog);

        Catalog Deserialize();
    }
}
