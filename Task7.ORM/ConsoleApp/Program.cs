using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB.Common;
using LinqToDB.Data;
using NorthwindDAL;
using NorthwindDAL.Interfaces;
using NorthwindDAL.Models;
using NorthwindORM;

namespace ConsoleApp
{
    internal class Program
    {
        private static IProductRepository _productRepository;
        private static IEmployeeRepository _employeeRepository;
        private static IOrderRepository _orderRepository;
        private static IStatisticsRepository _statisticsRepository;

        private static void Main(string[] args)
        {
            Setup();

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("1 - a list of products with a category and supplier");
                Console.WriteLine("2 - a list of employees with a region which they are responsible for");
                Console.WriteLine("3 - statistics by regions: a number of employees per region");
                Console.WriteLine("4 - a list \"employee - carriers who employee worked with\" (according to orders)");
                Console.WriteLine("5 - add new employee and specify a list of territories which he is responsible for");
                Console.WriteLine("6 - move products from one category to another category");
                Console.WriteLine("7 - add a list of products with their suppliers and categories");
                Console.WriteLine("8 - replace a product with a similar one in orders which are not processed yet");
                Console.WriteLine("other - exit");
                Console.Write("Enter key: ");
                var key = Console.ReadLine();
                Console.WriteLine();

                int intKey;
                int.TryParse(key, out intKey);

                switch (intKey)
                {
                    case 1:
                        var allProducts = _productRepository.GetAll().Take(10);
                        PrintProducts(allProducts);
                        break;
                    case 2:
                        var employees = _employeeRepository.GetAll().Take(10);
                        PrintEmployees(employees);
                        break;
                    case 3:
                        var regionStatistics = _statisticsRepository.GetStatisticsByRegion();
                        PrintRegionStatistics(regionStatistics);
                        break;
                    case 4:
                        var shippersStatistics = _statisticsRepository.GetEmployeeShippersStatistics();
                        PrintShipperStatistics(shippersStatistics);
                        break;
                    case 5:
                        var id = _employeeRepository.Add(GetNewEmployeeWithExistingTerritories());
                        PrintEmployees(new [] {_employeeRepository.Get(id)});
                        break;
                    case 6:
                        var products = _productRepository.GetAll().Where(x => x.SupplierId == 4);
                        Console.WriteLine("BEFORE");
                        PrintProducts(products);
                        _productRepository.ChangeProductsCategory(products, 3);
                        Console.WriteLine("AFTER");
                        products = _productRepository.GetAll().Where(x => x.SupplierId == 4);
                        PrintProducts(products);
                        break;
                    case 7:
                        var newProducts = GetNewProducts();
                        _productRepository.AddProductList(newProducts);
                        var allProductsWithInserted = _productRepository.GetAll();
                        var inserted = allProductsWithInserted.Skip(allProductsWithInserted.Count - newProducts.Count);
                        PrintProducts(inserted);
                        break;
                    case 8:
                        var existingOrders = new List<Order>
                        {
                            _orderRepository.Get(11070),
                            _orderRepository.Get(11072),
                            _orderRepository.Get(11077)
                        };
                        Console.WriteLine("BEFORE");
                        PrintOrders(existingOrders);
                        _orderRepository.ChangeProduct(existingOrders, 2, 1);
                        var updatedOrders = new List<Order>
                        {
                            _orderRepository.Get(11070),
                            _orderRepository.Get(11072),
                            _orderRepository.Get(11077)
                        };
                        Console.WriteLine("AFTER");
                        PrintOrders(updatedOrders);
                        break;
                    default:
                        exit = true;
                        break;
                }

                Console.WriteLine();
            }
        }

        private static void Setup()
        {
            Configuration.Linq.AllowMultipleQuery = true;
            var connection = new DataConnection("NorthwindDb");

            _productRepository = new ProductRepository(connection);
            _employeeRepository = new EmployeeRepository(connection);
            _orderRepository = new OrderRepository(connection);
            _statisticsRepository = new StatisticsRepository(connection);
        }

        private static void PrintProducts(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id}, Name: {product.ProductName}");
                Console.WriteLine(product.Category == null ? "\tNo category" : $"\tCategory: {product.Category.Id} - {product.Category.Name}");
                Console.WriteLine(product.Supplier == null ? "\tNo supplier" : $"\tSupplier: {product.Supplier.Id} - {product.Supplier.CompanyName}");
            }
        }

        private static void PrintEmployees(IEnumerable<Employee> employees)
        {
            foreach (var employee in employees)
            {
                Console.WriteLine($"Id: {employee.Id}, Name: {employee.FirstName} {employee.LastName}");

                foreach (var territory in employee.EmployeeTerritories)
                {
                    Console.WriteLine($"\tRegion: {territory.Territory.Region.Description} \tTerritory Id: {territory.Territory.Id}, Description: {territory.Territory.Description}");
                }
            }
        }

        private static void PrintRegionStatistics(IList<RegionStatistics> regionStatistics)
        {
            foreach (var statistics in regionStatistics)
            {
                Console.WriteLine($"Region Id: {statistics.Region.Id}, Name: {statistics.Region.Description}, Employees Count: {statistics.EmployeesCount}");
            }
        }

        private static void PrintShipperStatistics(IList<EmployeeShippers> employeeShippers)
        {
            foreach (var statistics in employeeShippers)
            {
                Console.WriteLine($"Employee Id: {statistics.Employee.Id}, Name: {statistics.Employee.FirstName} {statistics.Employee.LastName}");

                foreach (var shipper in statistics.Shippers)
                {
                    Console.WriteLine($"\tShipper Id: {shipper.Id}, Company Name: {shipper.CompanyName}");
                }
            }
        }

        private static void PrintOrders(IEnumerable<Order> orders)
        {
            foreach (var order in orders)
            {
                Console.WriteLine($"Order Id: {order.Id}, OrderDate: {order.OrderDate}, ShippedDate: {order.ShippedDate}");

                foreach (var orderDetail in order.OrderDetails)
                {
                    Console.WriteLine($"\tProduct Id: {orderDetail.Product.Id}, Product Name: {orderDetail.Product.ProductName}");
                }
            }
        }

        private static Employee GetNewEmployeeWithExistingTerritories()
        {
            return new Employee
            {
                FirstName = "Test",
                LastName = "Last Test",
                EmployeeTerritories = new List<EmployeeTerritory>
                {
                    new EmployeeTerritory { TerritoryId = "01581" },
                    new EmployeeTerritory { TerritoryId = "01730" }
                }
            };
        }

        private static IList<Product> GetNewProducts()
        {
            return new List<Product>
            {
                new Product { ProductName = "Test product 1", Category = new Category { Name = "Test category 1" }, Supplier = new Supplier { CompanyName = "Test supplier 1" }},
                new Product { ProductName = "Test product 2", Category = new Category { Name = "Dairy Products" }, Supplier = new Supplier { CompanyName = "Exotic Liquids" }},
                new Product { ProductName = "Test product 3", Category = new Category { Name = "Test category 1" }, Supplier = new Supplier { CompanyName = "Tokyo Traders" }},
                new Product { ProductName = "Test product 4", Supplier = new Supplier { CompanyName = "Tokyo Traders" }},
                new Product { ProductName = "Test product 5", Category = new Category { Name = "Test category 1" }},
                new Product { ProductName = "Test product 6" },
                new Product { ProductName = "Test product 7", Category = new Category { Name = "Seafood" }, Supplier = new Supplier { CompanyName = "Test supplier 1" }}
            };
        }
    }
}
