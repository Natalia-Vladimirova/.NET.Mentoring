using System.Collections.Generic;
using NorthwindORM;

namespace NorthwindDAL.Interfaces
{
    public interface IProductRepository
    {
        IList<Product> GetAll();

        void ChangeProductsCategory(IEnumerable<Product> products, int? categoryId);

        void AddProductList(IList<Product> products);
    }
}
