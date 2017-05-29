using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using NorthwindDAL.Interfaces;
using NorthwindORM;

namespace NorthwindDAL
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataConnection _dataConnection;

        public EmployeeRepository(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public IList<Employee> GetAll()
        {
            return _dataConnection.GetTable<Employee>()
                .LoadWith(x => x.Manager)
                .LoadWith(x => x.EmployeeTerritories)
                .LoadWith(x => x.EmployeeTerritories[0].Territory)
                .LoadWith(x => x.EmployeeTerritories[0].Territory.Region)
                .ToList();
        }

        public Employee Get(int id)
        {
            return _dataConnection.GetTable<Employee>()
                .LoadWith(x => x.Manager)
                .LoadWith(x => x.EmployeeTerritories)
                .LoadWith(x => x.EmployeeTerritories[0].Territory)
                .LoadWith(x => x.EmployeeTerritories[0].Territory.Region)
                .FirstOrDefault(x => x.Id == id);
        }

        public int Add(Employee employee)
        {
            var identity = _dataConnection.InsertWithIdentity(employee);

            int id;
            int.TryParse(identity.ToString(), out id);

            var territories = employee.EmployeeTerritories;

            foreach (var territory in territories)
            {
                territory.EmployeeId = id;
            }
            
            _dataConnection.BulkCopy(new BulkCopyOptions { BulkCopyType = BulkCopyType.MultipleRows }, territories);
            
            return id;
        }
    }
}
