// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{
		private readonly DataSource _dataSource = new DataSource();

		[Category("Restriction Operators")]
		[Title("Where - Example 1")]
		[Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]
		public void LinqExample1()
		{
			int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

			var lowNums =
				from num in numbers
				where num < 5
				select num;

			Console.WriteLine("Numbers < 5:");
			foreach (var x in lowNums)
			{
				Console.WriteLine(x);
			}
		}

		[Category("Restriction Operators")]
		[Title("Where - Example 2")]
		[Description("This sample returns all presented in market products.")]
		public void LinqExample2()
		{
			var products =
				from p in _dataSource.Products
				where p.UnitsInStock > 0
				select p;

			foreach (var p in products)
			{
				ObjectDumper.Write(p);
			}
		}

        
        [Category("Restriction Operators")]
	    [Title("Where - Task 1")]
	    [Description("This sample gives a list of customers where total turnover (the sum of all orders) is more than a certain value of X.")]
	    public void Linq1()
        {
            var coef = 10000;

            for (int i = 1; i < 4; i++)
            {
                var ordersTotal = i * coef;

                var customers = _dataSource.Customers.Where(x => x.Orders.Sum(y => y.Total) > ordersTotal);

                Console.WriteLine($"Orders Total > {ordersTotal}");
                foreach (var customer in customers)
                {
                    Console.WriteLine($"Customer ID: {customer.CustomerID}\tOrders Total: {customer.Orders.Sum(x => x.Total)}");
                }
                Console.WriteLine();
            }

        }
        
        [Category("Join Operators")]
        [Title("Join - Task 2.1")]
        [Description("This sample creates a list of suppliers for each customer located in the same country and the same city (using grouping).")]
        public void Linq2()
        {
            var suppliers = _dataSource.Suppliers;
            var customers = _dataSource.Customers;
            var customerSuppliers = customers.Join(suppliers, x => x.Country, x => x.Country,
                (c, s) => new { Customer = c, Supplier = s })
                .Where(x => x.Customer.City == x.Supplier.City)
                .GroupBy(x => x.Customer, x => x.Supplier);

            foreach (var info in customerSuppliers)
            {
                var customer = info.Key;
                Console.WriteLine($"Customer ID: {customer.CustomerID}\tCountry: {customer.Country}\tCity: {customer.City}");
                foreach (var supplier in info)
                {
                    Console.WriteLine($"\tSupplier Name: {supplier.SupplierName}\tCountry: {supplier.Country}\tCity: {supplier.City}");
                }
                Console.WriteLine();
            }
        }

        [Category("Join Operators")]
        [Title("Join - Task 2.2")]
        [Description("This sample creates a list of suppliers for each customer located in the same country and the same city (without using grouping).")]
        public void Linq3()
        {
            var suppliers = _dataSource.Suppliers;
            var customers = _dataSource.Customers;

            var customerSuppliers = customers.GroupJoin(suppliers, x => x.Country, x => x.Country, 
                (c, s) => new { Customer = c, Suppliers = s.Where(x => x.City == c.City) })
                .Where(x => x.Suppliers.Any());
            
            foreach (var info in customerSuppliers)
            {
                var customer = info.Customer;
                Console.WriteLine($"CustomerID: {customer.CustomerID}\tCountry: {customer.Country}\tCity: {customer.City}");
                foreach (var supplier in info.Suppliers)
                {
                    Console.WriteLine($"\tSupplier Name: {supplier.SupplierName}\tCountry: {supplier.Country}\tCity: {supplier.City}");
                }
                Console.WriteLine();
            }
        }
        
        [Category("Quantifiers")]
        [Title("Any - Task 3")]
        [Description("This sample finds all customers who have orders exceeding the value of X by sum.")]
        public void Linq4()
        {
            decimal orderTotal = 10000;
            var customers = _dataSource.Customers.Where(x => x.Orders.Any(y => y.Total > orderTotal));

            foreach (var customer in customers)
            {
                Console.WriteLine($"Customer ID: {customer.CustomerID}");
                foreach (var order in customer.Orders)
                {
                    Console.Write(order.Total > orderTotal ? "!!\t" : "\t");
                    ObjectDumper.Write(order);
                }
                Console.WriteLine();
            }
        }
        
        [Category("Ordering Operators")]
        [Title("OrderBy - Task 4")]
        [Description("This sample gives a list of customers indicating from which month of which year they became customers " +
                     "(get this month and this year from the very first order).")]
        public void Linq5()
        {
            var customers = _dataSource.Customers;

            foreach (var customer in customers)
            {
                var firstOrderDate = customer.Orders.OrderBy(y => y.OrderDate).FirstOrDefault()?.OrderDate;

                Console.Write($"Customer ID: {customer.CustomerID}\t");
                Console.WriteLine(firstOrderDate == null 
                    ? "No orders yet" 
                    : $"Order year: {firstOrderDate.Value.Year}\tOrder month: {firstOrderDate.Value:MMMM}");
            }
        }
        
        [Category("Ordering Operators")]
        [Title("OrderBy & ThenBy - Task 5")]
        [Description("This sample gives a list of customers indicating from which month of which year they became customers " +
                     "(get this month and this year from the very first order). " +
                     "The list is sorted by year, month, orders total sum (from max to min) and customer id")]
        public void Linq6()
        {
            var customers = _dataSource.Customers
                .Select(x => new
                {
                    Customer = x,
                    OrdersTotal = x.Orders.Sum(y => y.Total),
                    FirstOrderDate = x.Orders.OrderBy(y => y.OrderDate).FirstOrDefault()?.OrderDate
                })
                .OrderBy(x => x.FirstOrderDate?.Year)
                .ThenBy(x => x.FirstOrderDate?.Month)
                .ThenByDescending(x => x.OrdersTotal)
                .ThenBy(x => x.Customer.CustomerID);

            foreach (var customerInfo in customers)
            {
                Console.Write(customerInfo.FirstOrderDate == null
                    ? "No orders yet\t"
                    : $"Order year: {customerInfo.FirstOrderDate.Value.Year}\tOrder month: {customerInfo.FirstOrderDate.Value:MMMM}\t");
                Console.Write($"Orders Total: {customerInfo.OrdersTotal}\t");
                Console.WriteLine($"Customer ID: {customerInfo.Customer.CustomerID}");
            }
        }
        
        [Category("Restriction Operators")]
        [Title("Where - Task 6")]
        [Description("This sample finds all customers who have a non-digital postal code or " +
                     "a region is empty or an operator code is not specified in the phone " +
                     "(for simplicity, phone has no round brackets at the beginning).")]
        public void Linq7()
        {
            var postcodeRegex = new Regex(@"^\d+$");
            var phoneOperatorCodeRegex = new Regex(@"\(.+\).+");

            var customers = _dataSource.Customers
                .Where(x => !postcodeRegex.IsMatch(x.PostalCode ?? "") ||
                            string.IsNullOrWhiteSpace(x.Region) ||
                            !phoneOperatorCodeRegex.IsMatch(x.Phone));

            foreach (var customer in customers)
            {
                Console.WriteLine($"Customer ID: {customer.CustomerID}\t" +
                                  $"Region: {customer.Region}\t\t" +
                                  $"Postcode: {customer.PostalCode}\t\t" +
                                  $"Phone: {customer.Phone}");
            }
        }
        
        [Category("Grouping Operators")]
        [Title("GroupBy - Task 7")]
        [Description("This sample groups all products by categories, inside - by stock, inside the last group - orders by price.")]
        public void Linq8()
        {
            var categoryGroups = _dataSource.Products
                .GroupBy(x => x.Category, 
                         (s, enumerable) => new
                         {
                             Category = s,
                             StockGroups = enumerable.GroupBy(y => y.UnitsInStock,
                                                               (i, units) => new
                                                               {
                                                                   UnitsInStock = i,
                                                                   Products = units.OrderBy(z => z.UnitPrice)
                                                               })
                         });
            
            foreach (var categoryGroup in categoryGroups)
            {
                Console.WriteLine($"Category: {categoryGroup.Category}");
                foreach (var stockGroup in categoryGroup.StockGroups)
                {
                    Console.WriteLine($"\tIn Stock: {stockGroup.UnitsInStock}");
                    foreach (var product in stockGroup.Products)
                    {
                        Console.Write("\t\t");
                        ObjectDumper.Write(product);
                    }
                }
                Console.WriteLine();
            }
        }

        [Category("Grouping Operators")]
        [Title("GroupBy - Task 8")]
        [Description("This sample groups products by groups \"cheap\", \"average price\", \"expensive\".")]
        public void Linq9()
        {
            Func<decimal, string> getGroup = x =>
            {
                if (x < 50m) return "cheap";
                if (x < 100m) return "average";
                return "expensive";
            };

            var groupedProducts = _dataSource.Products
                .Select(x => new
                {
                    Group = getGroup(x.UnitPrice),
                    Product = x
                })
                .GroupBy(x => x.Group, (x, y) => new { Group = x, Products = y.Select(z => z.Product) });

            foreach (var groupedProduct in groupedProducts)
            {
                Console.WriteLine($"Group: {groupedProduct.Group}");
                foreach (var product in groupedProduct.Products)
                {
                    ObjectDumper.Write(product);
                }
                Console.WriteLine();
            }
        }
        
	    [Category("Aggregate Operators")]
	    [Title("Sum & Count - Task 9")]
	    [Description("This sample calculates the average profitability of each city " +
	                 "(the average order sum for all customers from a given city) and " +
	                 "the average intensity (the average amount of orders per customer from each city).")]
	    public void Linq10()
	    {
	        var citiesInfo = _dataSource.Customers
	            .GroupBy(x => x.City,
	                (city, customers) =>
	                {
	                    var totalOrdersAmount = customers.Sum(y => y.Orders.Length);

	                    return new
	                    {
	                        City = city,
	                        AverageProfitability = customers.Sum(y => y.Orders.Sum(z => z.Total)) / totalOrdersAmount,
	                        AverageIntensity = (decimal) totalOrdersAmount / customers.Count()
	                    };
	                });

	        foreach (var cityInfo in citiesInfo)
	        {
	            Console.WriteLine($"City: {cityInfo.City}\t\t" +
	                              $"Average Profitability: {cityInfo.AverageProfitability:#.##}\t\t" +
	                              $"Average Intensity: {cityInfo.AverageIntensity:#.##}");
	        }
	    }
        
	    [Category("Different Operators")]
	    [Title("SelectMany & GroupBy & OrderBy - Task 10")]
	    [Description("This sample creates average annual activity statistics of customers by months (without years), " +
	                 "statistics by years, " +
	                 "by years and by months (that is, when one month in different years is important).")]
	    public void Linq11()
	    {
	        #region Statistics By Months

	        var statisticsByMonths = _dataSource.Customers
	            .SelectMany(x => x.Orders, (customer, order) => new
	            {
                    OrderMonth = order.OrderDate.Month,
	                Order = order
	            })
	            .GroupBy(x => x.OrderMonth, (orderMonth, enumerable) => new
	            {
	                OrderMonth = orderMonth,
	                OrdersAmount = enumerable.Count()
	            })
	            .OrderBy(x => x.OrderMonth);

	        Console.WriteLine("Statistics by months without years");
	        foreach (var info in statisticsByMonths)
	        {
	            Console.WriteLine($"\tMonth: {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(info.OrderMonth)}\t\t" +
	                              $"Orders Amount: {info.OrdersAmount}");
	        }
	        Console.WriteLine();

            #endregion

            #region Statistics By Years

            var statisticsByYears = _dataSource.Customers
                .SelectMany(x => x.Orders, (customer, order) => new
                {
                    OrderYear = order.OrderDate.Year,
                    Order = order
                })
                .GroupBy(x => x.OrderYear, (orderYear, enumerable) => new
                {
                    OrderYear = orderYear,
                    OrdersAmount = enumerable.Count()
                })
                .OrderBy(x => x.OrderYear);

            Console.WriteLine("Statistics by years");
            foreach (var info in statisticsByYears)
            {
                Console.WriteLine($"\tYear: {info.OrderYear}\t\tOrders Amount: {info.OrdersAmount}");
            }
            Console.WriteLine();

            #endregion

            #region Statistics By Years And Months

            var statisticsByYearsAndMonths = _dataSource.Customers
                .SelectMany(x => x.Orders, (customer, order) => new
                {
                    OrderYear = order.OrderDate.Year,
                    OrderMonth = order.OrderDate.Month,
                    Order = order
                })
                .GroupBy(x => x.OrderYear, (orderYear, enumerable) => new
                {
                    OrderYear = orderYear,
                    StatisticsByMonths = enumerable.GroupBy(y => y.OrderMonth, (orderMonth, orders) => new
                    {
                        OrderMonth = orderMonth,
                        OrdersAmount = orders.Count()
                    })
                    .OrderBy(x => x.OrderMonth)
                })
                .OrderBy(x => x.OrderYear);

            Console.WriteLine("Statistics by years and months");
            foreach (var info in statisticsByYearsAndMonths)
            {
                Console.WriteLine($"\tYear: {info.OrderYear}");
                foreach (var statisticsByMonth in info.StatisticsByMonths)
                {
                    Console.WriteLine($"\t\tMonth: {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(statisticsByMonth.OrderMonth)}\t\t" +
                                  $"Orders Amount: {statisticsByMonth.OrdersAmount}");
                }
            }
            Console.WriteLine();

            #endregion
        }
    }
}
