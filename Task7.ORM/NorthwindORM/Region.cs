using LinqToDB.Mapping;

namespace NorthwindORM
{
    [Table(Name = "Region")]
    public class Region
    {
        [Column(Name = "RegionId"), PrimaryKey]
        public int Id { get; set; }

        [Column(Name = "RegionDescription", Length = 50), NotNull]
        public string Description { get; set; }
    }
}
