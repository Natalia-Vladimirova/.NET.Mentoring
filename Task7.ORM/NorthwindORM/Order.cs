using System;
using System.Collections.Generic;
using LinqToDB;
using LinqToDB.Mapping;

namespace NorthwindORM
{
    [Table(Name = "Orders")]
    public class Order
    {
        [Column(Name = "OrderId"), PrimaryKey, Identity]
        public int Id { get; set; }
        
        [Column(Length = 5, DataType = DataType.NChar)]
        public string CustomerId { get; set; }

        [Association(ThisKey = "CustomerId", OtherKey = "Id", CanBeNull = true)]
        public Customer Customer { get; set; }

        [Nullable, Column(DataType = DataType.Int32)]
        public int? EmployeeId { get; set; }

        [Association(ThisKey = "EmployeeId", OtherKey = "Id", CanBeNull = true)]
        public Employee Employee { get; set; }

        [Nullable, Column(DataType = DataType.DateTime)]
        public DateTime? OrderDate { get; set; }

        [Nullable, Column(DataType = DataType.DateTime)]
        public DateTime? RequiredDate { get; set; }

        [Nullable, Column(DataType = DataType.DateTime)]
        public DateTime? ShippedDate { get; set; }

        [Nullable, Column(DataType = DataType.Int32)]
        public int? ShipVia { get; set; }

        [Association(ThisKey = "ShipVia", OtherKey = "Id", CanBeNull = true)]
        public Shipper Shipper { get; set; }

        [Nullable, Column(DataType = DataType.Money)]
        public decimal? Freight { get; set; }

        [Column(Length = 40, DataType = DataType.NVarChar)]
        public string ShipName { get; set; }

        [Column(Length = 60, DataType = DataType.NVarChar)]
        public string ShipAddress { get; set; }

        [Column(Length = 15, DataType = DataType.NVarChar)]
        public string ShipCity { get; set; }

        [Column(Length = 15, DataType = DataType.NVarChar)]
        public string ShipRegion { get; set; }

        [Column(Length = 10, DataType = DataType.NVarChar)]
        public string ShipPostalCode { get; set; }

        [Column(Length = 15, DataType = DataType.NVarChar)]
        public string ShipCountry { get; set; }

        [Association(ThisKey = "Id", OtherKey = "OrderId")]
        public IList<OrderDetail> OrderDetails { get; set; }
    }
}
