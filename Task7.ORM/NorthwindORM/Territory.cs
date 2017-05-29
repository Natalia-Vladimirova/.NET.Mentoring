using System.Collections.Generic;
using LinqToDB;
using LinqToDB.Mapping;

namespace NorthwindORM
{
    [Table(Name = "Territories")]
    public class Territory
    {
        [Column(Name = "TerritoryId", Length = 20), PrimaryKey, NotNull]
        public string Id { get; set; }

        [Column(Name = "TerritoryDescription", Length = 50), PrimaryKey, NotNull]
        public string Description { get; set; }

        [Column(DataType = DataType.Int32)]
        public int RegionId { get; set; }

        [Association(ThisKey = "RegionId", OtherKey = "Id", CanBeNull = false)]
        public Region Region { get; set; }
        
        [Association(ThisKey = "Id", OtherKey = "TerritoryId")]
        public IList<EmployeeTerritory> EmployeeTerritories { get; set; }
    }
}
