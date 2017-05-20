using System;
using System.Collections.Generic;

namespace Northwind.DAL.Entities
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public string ShipCountry { get;set;}

        public string ShipCity { get;set;}

        public string ShipAddress { get; set; }

        public OrderStatus OrderStatus => GetOrderStatus();

        public IList<OrderDetails> Details { get; set; }

        private OrderStatus GetOrderStatus()
        {
            if (OrderDate == null)
            {
                return OrderStatus.New;
            }

            return ShippedDate == null 
                ? OrderStatus.InProcess 
                : OrderStatus.Completed;
        }
    }
}