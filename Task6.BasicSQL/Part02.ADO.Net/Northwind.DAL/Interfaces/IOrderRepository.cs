using System;
using System.Collections.Generic;
using Northwind.DAL.Entities;

namespace Northwind.DAL.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();
        
        Order GetOrderById(int id);

        int CreateOrder(Order order);

        int UpdateOrder(Order order);

        bool DeleteOrder(int orderId);

        bool MoveToProcess(int orderId, DateTime date);

        bool MoveToCompleted(int orderId, DateTime date);
    }
}
