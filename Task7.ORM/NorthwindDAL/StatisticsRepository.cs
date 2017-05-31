using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using NorthwindDAL.Comparers;
using NorthwindDAL.Interfaces;
using NorthwindDAL.Models;
using NorthwindORM;

namespace NorthwindDAL
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly DataConnection _dataConnection;

        public StatisticsRepository(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public IList<RegionStatistics> GetStatisticsByRegion()
        {
            return _dataConnection.GetTable<Territory>()
                .LoadWith(x => x.Region)
                .LoadWith(x => x.EmployeeTerritories)
                .ToList()
                .GroupBy(x => x.RegionId, (x, y) => new RegionStatistics
                {
                    Region = y.FirstOrDefault(z => z.RegionId == x)?.Region, //Error	2	Invalid expression term '.'
                    EmployeesCount = y.SelectMany(z => z.EmployeeTerritories).Count()
                })
                .OrderBy(x => x.Region.Id)
                .ToList();
        }

        public IList<EmployeeShippers> GetEmployeeShippersStatistics()
        {
            return _dataConnection.GetTable<Order>()
                 .LoadWith(x => x.Employee)
                 .LoadWith(x => x.Shipper)
                 .ToList()
                 .GroupBy(x => x.EmployeeId, (x, y) => new EmployeeShippers
                 {
                     Employee = y.FirstOrDefault(z => z.EmployeeId == x)?.Employee, //Error	4	Invalid expression term '.'
                     Shippers = y.Select(z => z.Shipper).Distinct(new ShipperComparer()).ToList()
                 })
                 .OrderBy(x => x.Employee.Id)
                 .ToList();
        }
    }
}
