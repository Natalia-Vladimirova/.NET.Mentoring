using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using Northwind.DAL.Entities;
using Northwind.DAL.Interfaces;
using NUnit.Framework;

namespace Northwind.DAL.Tests
{
    [TestFixture]
    public class OrderRepositoryTests
    {
        private IOrderRepository _orderRepository;

        [TestFixtureSetUp]
        public void Setup()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["NorthwindDb"];
            var connectionString = connectionStringSettings.ConnectionString;
            var providerFactory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);

            _orderRepository = new OrderRepository(providerFactory, connectionString, new NorthwindMapper());
        }

        //to use this method pass to it id of existing order
        [TestCase(10250)]
        public void GetOrdersTest(int orderId)
        {
            var order = _orderRepository.GetOrderById(orderId);
            var orders = _orderRepository.GetOrders();

            Assert.IsNotNull(order);
            Assert.IsNotNull(orders);
            Assert.IsNotEmpty(orders);

            var existingOrder = orders.FirstOrDefault(x => x.OrderId == order.OrderId);

            Assert.IsNotNull(existingOrder);
        }

        [Test]
        public void InsertOrderWithoutDetailsTest()
        {
            var newOrder = new Order
            {
                ShipCountry = "Test",
                ShipCity = "City",
                ShipAddress = "Address"
            };

            int id = _orderRepository.CreateOrder(newOrder);

            var insertedOrder = _orderRepository.GetOrderById(id);

            Assert.AreEqual(newOrder.ShipCountry, insertedOrder.ShipCountry);
            Assert.AreEqual(newOrder.ShipCity, insertedOrder.ShipCity);
            Assert.AreEqual(newOrder.ShipAddress, insertedOrder.ShipAddress);

            Assert.AreEqual(0, insertedOrder.Details.Count);
        }

        [Test]
        public void InsertOrderWithDetailsTest()
        {
            var firstDetails = new OrderDetails
            {
                ProductId = 41, Quantity = 13, Discount = 0.13f, UnitPrice = 10.3m
            };

            var secondDetails = new OrderDetails
            {
                ProductId = 51, Quantity = 33, Discount = 0.23f, UnitPrice = 12.3m
            };

            var newOrder = new Order
            {
                ShipCountry = "Another country",
                ShipCity = "City",
                ShipAddress = "Address",
                Details = new List<OrderDetails> { firstDetails, secondDetails }
            };

            int id = _orderRepository.CreateOrder(newOrder);

            var insertedOrder = _orderRepository.GetOrderById(id);

            Assert.AreEqual(newOrder.ShipCountry, insertedOrder.ShipCountry);
            Assert.AreEqual(newOrder.ShipCity, insertedOrder.ShipCity);
            Assert.AreEqual(newOrder.ShipAddress, insertedOrder.ShipAddress);

            Assert.AreEqual(2, insertedOrder.Details.Count);

            var actualDetails = insertedOrder.Details.First();

            Assert.AreEqual(id, actualDetails.OrderId);
            CheckDetails(firstDetails, actualDetails);

            actualDetails = insertedOrder.Details.Last();

            Assert.AreEqual(id, actualDetails.OrderId);
            CheckDetails(secondDetails, actualDetails);
        }

        //to use this method pass to it id of existing order with details (where status is New)
        [TestCase(11101)]
        public void UpdateOrderWithDetailsTest(int orderId)
        {
            string shipCountry = "Upated country";
            string shipCity = "Upated city";
            string shipAddress = "Upated address";

            short quantity = 23;
            float discount = 0.1f;
            decimal unitPrice = 100.3m;
            
            var existingOrder = _orderRepository.GetOrderById(orderId);

            existingOrder.ShipCountry = shipCountry;
            existingOrder.ShipCity = shipCity;
            existingOrder.ShipAddress = shipAddress;

            var details = existingOrder.Details.First();

            details.Quantity = quantity;
            details.Discount = discount;
            details.UnitPrice = unitPrice;

            _orderRepository.UpdateOrder(existingOrder);

            var updatedOrder = _orderRepository.GetOrderById(orderId);

            Assert.AreEqual(shipCountry, updatedOrder.ShipCountry);
            Assert.AreEqual(shipCity, updatedOrder.ShipCity);
            Assert.AreEqual(shipAddress, updatedOrder.ShipAddress);
            
            var firstDetails = updatedOrder.Details.First();
            
            Assert.AreEqual(quantity, firstDetails.Quantity);
            Assert.AreEqual(discount, firstDetails.Discount);
            Assert.AreEqual(unitPrice, firstDetails.UnitPrice);
        }

        //to use this method pass to it id of existing order (where status is not Completed)
        [TestCase(11101)]
        public void DeleteOrderTests(int orderId)
        {
            _orderRepository.DeleteOrder(orderId);

            var order = _orderRepository.GetOrderById(orderId);

            Assert.IsNull(order);
        }

        //to use this method pass to it id of existing order (where status is new)
        [TestCase(11095)]
        public void ChangeOrderStatus(int orderId)
        {
            var dateProcessed = new DateTime(2017, 05, 18);
            var dateCompleted = new DateTime(2017, 05, 19);

            _orderRepository.MoveToProcess(orderId, dateProcessed);
            _orderRepository.MoveToCompleted(orderId, dateCompleted);

            var order = _orderRepository.GetOrderById(orderId);

            Assert.AreEqual(dateProcessed, order.OrderDate);
            Assert.AreEqual(dateCompleted, order.ShippedDate);
        }

        private void CheckDetails(OrderDetails expected, OrderDetails actual)
        {
            Assert.AreEqual(expected.ProductId, actual.ProductId);
            Assert.AreEqual(expected.Quantity, actual.Quantity);
            Assert.AreEqual(expected.Discount, actual.Discount);
            Assert.AreEqual(expected.UnitPrice, actual.UnitPrice);
            Assert.IsNotNull(actual.Product);
        }
    }
}
