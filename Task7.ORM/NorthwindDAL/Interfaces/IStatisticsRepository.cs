using System.Collections.Generic;
using NorthwindDAL.Models;

namespace NorthwindDAL.Interfaces
{
    public interface IStatisticsRepository
    {
        IList<RegionStatistics> GetStatisticsByRegion();

        IList<EmployeeShippers> GetEmployeeShippersStatistics();
    }
}
