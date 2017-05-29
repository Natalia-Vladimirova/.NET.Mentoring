using LinqToDB;
using LinqToDB.Mapping;

namespace NorthwindORM
{
    [Table(Name = "Shippers")]
    public class Shipper
    {
        [Column(Name = "ShipperId"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column(Length = 40), NotNull]
        public string CompanyName { get; set; }

        [Column(Length = 24, DataType = DataType.NVarChar)]
        public string Phone { get; set; }
    }
}
