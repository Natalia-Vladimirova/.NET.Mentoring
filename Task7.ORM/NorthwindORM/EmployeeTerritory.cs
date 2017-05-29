using LinqToDB.Mapping;

namespace NorthwindORM
{
    [Table(Name = "EmployeeTerritories")]
    public class EmployeeTerritory
    {
        [PrimaryKey]
        public int EmployeeId { get; set; }

        [Association(ThisKey = "EmployeeId", OtherKey = "Id", CanBeNull = false)]
        public Employee Employee { get; set; }

        [PrimaryKey, NotNull, Column(Length = 20)]
        public string TerritoryId { get; set; }

        [Association(ThisKey = "TerritoryId", OtherKey = "Id", CanBeNull = false)]
        public Territory Territory { get; set; }
    }
}
