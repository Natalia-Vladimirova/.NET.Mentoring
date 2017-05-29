using LinqToDB;
using LinqToDB.Mapping;

namespace NorthwindORM
{
    [Table(Name = "Products")]
    public class Product
    {
        [Column(Name = "ProductId"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column(Length = 40), NotNull]
        public string ProductName { get; set; }

        [Nullable, Column(DataType = DataType.Int32)]
        public int? SupplierId { get; set; }
        
        [Association(ThisKey = "SupplierId", OtherKey = "Id", CanBeNull = true)]
        public Supplier Supplier { get; set; }

        [Nullable, Column(DataType = DataType.Int32)]
        public int? CategoryId { get; set; }

        [Association(ThisKey = "CategoryId", OtherKey = "Id", CanBeNull = true)]
        public Category Category { get; set; }

        [Column(Length = 20)]
        public string QuantityPerUnit { get; set; }

        [Nullable, Column(DataType = DataType.Decimal)]
        public decimal? UnitPrice { get; set; }

        [Nullable, Column(DataType = DataType.Int16)]
        public short? UnitsInStock { get; set; }

        [Nullable, Column(DataType = DataType.Int16)]
        public short? UnitsOnOrder { get; set; }

        [Nullable, Column(DataType = DataType.Int16)]
        public short? ReorderLevel { get; set; }

        [Column(DataType = DataType.Binary)]
        public bool Discontinued { get; set; }
    }
}
