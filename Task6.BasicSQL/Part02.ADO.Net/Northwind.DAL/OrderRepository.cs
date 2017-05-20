using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Northwind.DAL.Entities;
using Northwind.DAL.Interfaces;

namespace Northwind.DAL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public OrderRepository(DbProviderFactory dbProviderFactory, string connectionString, IMapper mapper)
        {
            _dbProviderFactory = dbProviderFactory;
            _connectionString = connectionString;
            _mapper = mapper;
        }
        
        public bool MoveToProcess(int orderId, DateTime date)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@OrderID"] = orderId,
                ["@ActualDate"] = date
            };

            return ChangeStatus("OrderDate", parameters);
        }

        public bool MoveToCompleted(int orderId, DateTime date)
        {
            var parameters = new Dictionary<string, object>
            {
                ["@OrderID"] = orderId,
                ["@ActualDate"] = date
            };

            return ChangeStatus("ShippedDate", parameters);
        }

        public IEnumerable<Order> GetOrders()
        {
            return Get(SqlCommandTexts.GetOrdersCommand, GetAll);
        }

        public Order GetOrderById(int id)
        {
            var command = SqlCommandTexts.GetOrderCommand + SqlCommandTexts.GetOrderDetailsWithProductsCommand;

            var parameters = new Dictionary<string, object>
            {
                ["@OrderID"] = id
            };

            return Get(command, GetOrderWithDetails, parameters);
        }

        public int CreateOrder(Order order)
        {
            var command = SqlCommandTexts.InsertOrderCommand;
            var additionalCommand = SqlCommandTexts.InsertOrderDetailsCommand;

            var parameters = new Dictionary<string, object>
            {
                ["@ShipCountry"] = order.ShipCountry,
                ["@ShipCity"] = order.ShipCity,
                ["@ShipAddress"] = order.ShipAddress
            };

            return Execute(command, additionalCommand, parameters, order.Details);
        }

        public int UpdateOrder(Order order)
        {
            if (order.OrderStatus == OrderStatus.InProcess || order.OrderStatus == OrderStatus.Completed)
            {
                return -1;
            }

            var command = SqlCommandTexts.UpdateOrderCommand;
            var additionalCommand = SqlCommandTexts.UpdateOrderDetailsCommand;

            var parameters = new Dictionary<string, object>
            {
                ["@OrderID"] = order.OrderId,
                ["@ShipCountry"] = order.ShipCountry,
                ["@ShipCity"] = order.ShipCity,
                ["@ShipAddress"] = order.ShipAddress
            };

            return Execute(command, additionalCommand, parameters, order.Details);
        }

        public bool DeleteOrder(int orderId)
        {
            using (IDbConnection dbConnection = _dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = _connectionString;
                dbConnection.Open();

                using (IDbCommand command = dbConnection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = SqlCommandTexts.GetOrderCommand;

                    Order order;
                    
                    CreateParameters(command, new Dictionary<string, object> { ["@OrderID"] = orderId });

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        order = GetOrderWithDetails(reader);
                    }

                    if (order == null || order.OrderStatus == OrderStatus.Completed) return false;

                    DeleteOrderDetails(command);

                    return  DeleteOrder(command);
                }
            }
        }

        private bool DeleteOrder(IDbCommand command)
        {
            command.CommandText = SqlCommandTexts.DeleteOrderCommand;
            return command.ExecuteNonQuery() > 0;
        }

        private void DeleteOrderDetails(IDbCommand command)
        {
            command.CommandText = SqlCommandTexts.DeleteOrderDetailsCommand;
            command.ExecuteNonQuery();
        }

        private IEnumerable<Order> GetAll(IDataReader reader)
        {
            var result = new List<Order>();

            while (reader.Read())
            {
                var order = _mapper.Map<Order>(reader);
                result.Add(order);
            }

            return result;
        }

        private Order GetOrderWithDetails(IDataReader reader)
        {
            if (!reader.Read()) return null;

            var order = _mapper.Map<Order>(reader);
            order.Details = new List<OrderDetails>();

            reader.NextResult();

            while (reader.Read())
            {
                var details = _mapper.Map<OrderDetails>(reader);
                details.Product = _mapper.Map<Product>(reader);
                order.Details.Add(details);
            }

            return order;
        }

        private T Get<T>(string commandText, Func<IDataReader, T> executeQuery, IDictionary<string, object> parameters = null)
        {
            using (IDbConnection dbConnection = _dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = _connectionString;
                dbConnection.Open();
                
                using (IDbCommand command = dbConnection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;

                    CreateParameters(command, parameters);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        return executeQuery(reader);
                    }
                }
            }
        }
        
        private int Execute(string commandText, string additionalCommandText, IDictionary<string, object> parameters, IList<OrderDetails> orderDetails)
        {
            using (IDbConnection dbConnection = _dbProviderFactory.CreateConnection())
            {
                int orderId;

                dbConnection.ConnectionString = _connectionString;
                dbConnection.Open();

                using (IDbCommand command = dbConnection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;

                    CreateParameters(command, parameters);
                    
                    orderId = (int) command.ExecuteScalar();
                }

                if (orderDetails != null && orderDetails.Any())
                {
                    ExecuteAdditional(dbConnection, additionalCommandText, orderId, orderDetails);
                }

                return orderId;
            }
        }

        private void ExecuteAdditional(IDbConnection connection, string commandText, int orderId, IList<OrderDetails> orderDetails)
        {
            using (IDbTransaction transaction = connection.BeginTransaction())
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.CommandText = commandText;

                    var parameters = new Dictionary<string, object>();
                    
                    foreach (var details in orderDetails)
                    {
                        parameters["@OrderId"] = orderId;
                        parameters["@ProductId"] = details.ProductId;
                        parameters["@UnitPrice"] = details.UnitPrice;
                        parameters["@Quantity"] = details.Quantity;
                        parameters["@Discount"] = details.Discount;

                        CreateParameters(command, parameters);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }

        private bool ChangeStatus(string dateName, IDictionary<string, object> parameters)
        {
            using (IDbConnection dbConnection = _dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = _connectionString;
                dbConnection.Open();

                using (IDbCommand command = dbConnection.CreateCommand())
                {
                    command.CommandText = string.Format(SqlCommandTexts.UpdateOrderStatusCommand, dateName);
                    command.CommandType = CommandType.Text;

                    CreateParameters(command, parameters);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        private void CreateParameters(IDbCommand command, IDictionary<string, object> parameters)
        {
            if (parameters == null || !parameters.Any()) return;

            foreach (var key in parameters.Keys)
            {
                var param = command.CreateParameter();
                param.ParameterName = key;
                param.Value = parameters[key];
                command.Parameters.Add(param);
            }
        }
        
        private static class SqlCommandTexts
        {
            public const string GetOrdersCommand = "select OrderID, OrderDate, ShippedDate, ShipCountry, ShipCity, " +
                                                   "ShipAddress from Orders ";

            public const string GetOrderCommand = GetOrdersCommand + "where OrderID = @OrderID; ";

            public const string GetOrderDetailsWithProductsCommand = "select " +
                "od.OrderID, od.ProductID, od.UnitPrice, od.Quantity, od.Discount, " +
                "p.ProductName, p.SupplierID, p.CategoryID, p.QuantityPerUnit, p.UnitPrice as ProductUnitPrice, " +
                "p.UnitsInStock, p.UnitsOnOrder, p.ReorderLevel, p.Discontinued "+
                "from [Order Details] od " +
                "join Products p " +
                "on od.ProductID = p.ProductID " +
                "where OrderId = @OrderID;";

            public const string InsertOrderCommand = "insert into Orders " +
                "(ShipCountry, ShipCity, ShipAddress) output inserted.OrderID "+
                "values (@ShipCountry, @ShipCity, @ShipAddress)";

            public const string InsertOrderDetailsCommand = "insert into [Order Details] " +
                "(OrderID, ProductID, UnitPrice, Quantity, Discount) " +
                "values(@OrderID, @ProductID, @UnitPrice, @Quantity, @Discount)";

            public const string UpdateOrderCommand = "update Orders " +
                "set ShipCountry = @ShipCountry, ShipCity = @ShipCity, ShipAddress = @ShipAddress " +
                "output inserted.OrderID where OrderID = @OrderID";

            public const string UpdateOrderDetailsCommand = "update [Order Details] " +
                "set UnitPrice = @UnitPrice, Quantity = @Quantity, Discount = @Discount " +
                "where OrderID = @OrderID and ProductID = @ProductID";

            public const string DeleteOrderCommand = "delete from Orders where OrderID = @OrderID";

            public const string DeleteOrderDetailsCommand = "delete from [Order Details] where OrderID = @OrderID";

            public const string UpdateOrderStatusCommand = "update Orders " +
                                                           "set {0} = @ActualDate " +
                                                           "where OrderID = @OrderID";
        }
    }
}
