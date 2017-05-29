using LinqToDB.Mapping;

namespace NorthwindORM
{
    [Table(Name = "Order Details")]
    public class OrderDetail
    {
        [PrimaryKey]
        public int OrderId { get; set; }

        [Association(ThisKey = "OrderId", OtherKey = "Id", CanBeNull = false)]
        public Order Order { get; set; }

        [PrimaryKey]
        public int ProductId { get; set; }

        [Association(ThisKey = "ProductId", OtherKey = "Id", CanBeNull = false)]
        public Product Product { get; set; }

        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }
    }
}
