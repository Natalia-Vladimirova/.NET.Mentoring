using LinqToDB;
using LinqToDB.Mapping;

namespace NorthwindORM
{
    [Table(Name = "Categories")]
    public class Category
    {
        [Column(Name = "CategoryId"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column(Name = "CategoryName", Length = 15), NotNull]
        public string Name { get; set; }

        [Column(DataType = DataType.NVarChar)]
        public string Description { get; set; }

        [Column(DataType = DataType.Image)]
        public byte[] Picture { get; set; }
    }
}
