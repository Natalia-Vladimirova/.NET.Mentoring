using System;
using System.Collections.Generic;
using LinqToDB;
using LinqToDB.Mapping;

namespace NorthwindORM
{
    [Table(Name = "Employees")]
    public class Employee
    {
        [Column(Name = "EmployeeId"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column(Length = 20), NotNull]
        public string LastName { get; set; }

        [Column(Length = 10), NotNull]
        public string FirstName { get; set; }

        [Column(Length = 30, DataType = DataType.NVarChar)]
        public string Title { get; set; }

        [Column(Length = 25, DataType = DataType.NVarChar)]
        public string TitleOfCourtesy { get; set; }

        [Nullable, Column(DataType = DataType.DateTime)]
        public DateTime? BirthDate { get; set; }

        [Nullable, Column(DataType = DataType.DateTime)]
        public DateTime? HireDate { get; set; }

        [Column(Length = 60, DataType = DataType.NVarChar)]
        public string Address { get; set; }

        [Column(Length = 15, DataType = DataType.NVarChar)]
        public string City { get; set; }

        [Column(Length = 15, DataType = DataType.NVarChar)]
        public string Region { get; set; }

        [Column(Length = 10, DataType = DataType.NVarChar)]
        public string PostalCode { get; set; }

        [Column(Length = 15, DataType = DataType.NVarChar)]
        public string Country { get; set; }

        [Column(Length = 24, DataType = DataType.NVarChar)]
        public string HomePhone { get; set; }

        [Column(Length = 4, DataType = DataType.NVarChar)]
        public string Extension { get; set; }

        [Column(DataType = DataType.Image)]
        public byte[] Photo { get; set; }

        [Column(DataType = DataType.NText)]
        public string Notes { get; set; }
        
        [Nullable, Column(DataType = DataType.Int32)]
        public int? ReportsTo { get; set; }

        [Association(ThisKey = "ReportsTo", OtherKey = "Id", CanBeNull = true)]
        public Employee Manager { get; set; }

        [Column(Length = 255, DataType = DataType.NVarChar)]
        public string PhotoPath { get; set; }

        [Association(ThisKey = "Id", OtherKey = "EmployeeId")]
        public IList<EmployeeTerritory> EmployeeTerritories { get; set; }
    }
}
