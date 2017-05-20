using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Northwind.DAL.Entities;
using Northwind.DAL.Interfaces;

namespace Northwind.DAL
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public StatisticsRepository(DbProviderFactory dbProviderFactory, string connectionString, IMapper mapper)
        {
            _dbProviderFactory = dbProviderFactory;
            _connectionString = connectionString;
            _mapper = mapper;
        }

        public IEnumerable<CustOrderHist> GetCustOrderHist(string customerId)
        {
            return Get<CustOrderHist>("CustOrderHist", "@CustomerID", customerId);
        }

        public IEnumerable<CustOrdersDetail> GetCustOrdersDetail(int orderId)
        {
            return Get<CustOrdersDetail>("CustOrdersDetail", "@OrderID", orderId);
        }

        private IEnumerable<T> Get<T>(string commandText, string paramName, object paramValue) where T : new()
        {
            using (IDbConnection dbConnection = _dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = _connectionString;
                dbConnection.Open();

                using (IDbCommand command = dbConnection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.CommandType = CommandType.StoredProcedure;

                    CreateParameter(command, paramName, paramValue);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        var result = new List<T>();

                        while (reader.Read())
                        {
                            var obj = _mapper.Map<T>(reader);
                            result.Add(obj);
                        }

                        return result;
                    }
                }
            }
        }

        private void CreateParameter(IDbCommand command, string paramName, object paramValue)
        {
            var param = command.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            command.Parameters.Add(param);
        }
    }
}
