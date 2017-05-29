using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using NorthwindDAL.Interfaces;
using NorthwindORM;

namespace NorthwindDAL
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataConnection _dataConnection;

        public ProductRepository(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public IList<Product> GetAll()
        {
            return _dataConnection.GetTable<Product>()
                .LoadWith(x => x.Category)
                .LoadWith(x => x.Supplier)
                .ToList();
        }

        public void ChangeProductsCategory(IEnumerable<Product> products, int? categoryId)
        {
            foreach (var product in products)
            {
                _dataConnection.GetTable<Product>()
                    .Where(x => x.Id == product.Id)
                    .Set(x => x.CategoryId, categoryId)
                    .Update();
            }
        }

        public void AddProductList(IList<Product> products)
        {
            foreach (var product in products)
            {
                UpdateCategory(product);
                UpdateSupplier(product);
            }

            _dataConnection.BulkCopy(new BulkCopyOptions { BulkCopyType = BulkCopyType.MultipleRows }, products);
        }

        private void UpdateCategory(Product product)
        {
            if (product.Category == null) return;

            var existingCategory = _dataConnection.GetTable<Category>()
                .FirstOrDefault(x => x.Name == product.Category.Name);

            if (existingCategory == null)
            {
                int id = Convert.ToInt32(_dataConnection.InsertWithIdentity(product.Category));
                product.Category.Id = id;
                product.CategoryId = id;
            }
            else
            {
                product.Category = existingCategory;
                product.CategoryId = existingCategory.Id;
            }
        }

        private void UpdateSupplier(Product product)
        {
            if (product.Supplier == null) return;

            var existingSupplier = _dataConnection.GetTable<Supplier>()
                .FirstOrDefault(x => x.CompanyName == product.Supplier.CompanyName);

            if (existingSupplier == null)
            {
                int id = Convert.ToInt32(_dataConnection.InsertWithIdentity(product.Supplier));
                product.Supplier.Id = id;
                product.SupplierId = id;
            }
            else
            {
                product.Supplier = existingSupplier;
                product.SupplierId = existingSupplier.Id;
            }
        }
    }
}
