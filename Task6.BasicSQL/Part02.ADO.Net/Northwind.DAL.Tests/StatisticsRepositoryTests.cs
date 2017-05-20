using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using Northwind.DAL.Interfaces;
using NUnit.Framework;

namespace Northwind.DAL.Tests
{
    [TestFixture]
    public class StatisticsRepositoryTests
    {
        private IStatisticsRepository _statisticsRepository;

        [TestFixtureSetUp]
        public void Setup()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["NorthwindDb"];
            var connectionString = connectionStringSettings.ConnectionString;
            var providerFactory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
            
            _statisticsRepository = new StatisticsRepository(providerFactory, connectionString, new NorthwindMapper());
        }

        //to use this method pass to it id of existing order
        [TestCase(10250)]
        public void GetCustOrdersDetail(int orderId)
        {
            var statistics = _statisticsRepository.GetCustOrdersDetail(orderId);
            statistics.ToList().ForEach(x => Debug.WriteLine($"\t{x}"));
        }

        //to use this method pass to it id of existing customer
        [TestCase("AROUT")]
        public void GetCustOrderHist(string customerId)
        {
            var statistics = _statisticsRepository.GetCustOrderHist(customerId);
            statistics.ToList().ForEach(x => Debug.WriteLine($"\t{x}"));
        }
    }
}
