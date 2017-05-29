using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using NorthwindDAL.Interfaces;
using NorthwindORM;

namespace NorthwindDAL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataConnection _dataConnection;

        public OrderRepository(DataConnection dataConnection)
        {
            _dataConnection = dataConnection;
        }

        public Order Get(int id)
        {
            return _dataConnection.GetTable<Order>()
                .LoadWith(x => x.Customer)
                .LoadWith(x => x.Employee)
                .LoadWith(x => x.Shipper)
                .LoadWith(x => x.OrderDetails)
                .LoadWith(x => x.OrderDetails[0].Product)
                .FirstOrDefault(x => x.Id == id);
        }

        public void ChangeProduct(IEnumerable<Order> orders, int oldProductId, int newProductId)
        {
            foreach (var order in orders)
            {
                if (order.ShippedDate != null) continue;

                var existingOrderDetail = order.OrderDetails.FirstOrDefault(x => x.ProductId == newProductId);

                if (existingOrderDetail != null) continue;

                var orderDetail = order.OrderDetails.FirstOrDefault(x => x.ProductId == oldProductId); 

                if (orderDetail == null) continue;

                _dataConnection.GetTable<OrderDetail>()
                    .Where(x => x.OrderId == order.Id && x.ProductId == oldProductId)
                    .Set(x => x.ProductId, newProductId)
                    .Update();
            }
        }
    }
}
