using System.Collections.Generic;
using Northwind.DAL.Entities;

namespace Northwind.DAL.Interfaces
{
    public interface IStatisticsRepository
    {
        IEnumerable<CustOrderHist> GetCustOrderHist(string customerId);

        IEnumerable<CustOrdersDetail> GetCustOrdersDetail(int orderId);
    }
}
