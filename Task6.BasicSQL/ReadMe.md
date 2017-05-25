## Task 6 - Basic SQL

### Part 1 - SQL

#### General

To perform this task use Northwind database (instnwnd.sql).

#### Task 1. SQL queries for search and filtration

Results of the task can be arranged as sql scripts or can be added to SSDT project which should be created from instnwnd.sql file (do not forget to exclude such scripts from compilation of the project).

##### Task 1.1. Simple data filtration

1. Select orders from Orders table which were delivered after 6 May 1998 inclusive (ShippedDate column) and which were delivered with ShipVia >= 2. The query should return only OrderID, ShippedDate and ShipVia columns.
2. Write a query which returns only non delivered orders from Orders table. The result of the query should contain 'Not Shipped' string instead of NULL values in ShippedDate column (use the system function CASE). The query should return only OrderID and ShippedDate columns.
3. Select orders from Orders table which were delivered after 6 May 1998 excluding this date (ShippedDate column) or orders which are not delivered yet. The query should return only OrderID (rename to Order Number) and ShippedDate (rename to Shipped Date) columns. Also, ShippedDate column should contain 'Not Shipped' string instead of NULL values and date in the default format in the other cases.

##### Task 1.2. Using IN, DISTINCT, ORDER BY, NOT operators

1. Select all customers from Customers table who live in USA and Canada. Use only IN operator in this query. The query should return user name and country name columns. Order the query result by customer name and country.
2. Select all customers from Customers table who do not live in USA and Canada. Use IN operator in the query. The query should return user name and country name columns. Order the query result by customer name.
3. Select all countries from Customers table where customers live in. A country should be mentioned only one time. The query result should be ordered descending. Do not use GROUP BY clause. Return only one column in the query result.
    
##### Task 1.3. Using BETWEEN, DISTINCT operators

1. Select all distinct orders (OrderID) from Order Details table which have products with quantity between 3 and 10 inclusive (Quantity column in Order Details table). Use BETWEEN operator. The query should return only OrderID column.
2. Select all customers from Customers table who have country name started with letters from the range b and g. Use BETWEEN operator. Check that result contains Germany. The query should return only CustomerID and Country columns and should be ordered by Country.
3. Select all customers from Customers table who have country name started with letters from the range b and g. Do not use BETWEEN operator.

##### Task 1.4. Using LIKE operator

1. Find all products (ProductName column) from Products table which have 'chocolade' substring. It is known that one letter 'c' in the middle can be changed in 'chocolade' substring. Find all products satisfied this condition.

#### Task 2. SQL queries for joining and aggregation

##### Task 2.1. Using aggregate functions (SUM, COUNT)

1. Find the total amount of all orders from the Order Details table taking into account the number of purchased products and their discounts. The query should return one record with one column named 'Totals'.
2. Find the number of orders in Orders table which are not delivered yet (ShippedDate column does not have a value). Use only COUNT operator. Do not use WHERE and GROUP clauses.
3. Find the number of distinct customers (CustomerID) in Orders table who placed orders. Use COUNT function. Do not use WHERE and GROUP clauses.

##### Task 2.2. Join tables, using aggregate functions and GROUP BY and HAVING clauses

1. Find the number of orders in Orders table using grouping by year. The query should return two columns with Year and Total names. Write a check query which calculates the number of all orders.
2. Find the number of orders in Orders table using grouping by seller. An order for specified seller is any record in Orders table which has value in EmployeeID column. The query should return a column with seller name named 'Seller' (use LastName and FirstName concatenation; this string should be obtained by a separate query in the main one. Also, the main query should use grouping by EmployeeID) and a column with a number of orders named 'Amount'. The query result should be ordered by the number of orders descending.
3. Find the number of orders in Orders table using grouping by seller and customer. Find this only for orders made in 1998 year.
4. Find the number of customers and sellers who live in the same city. If only one or several sellers or ony one or several customers live in a city such information should not appear in the query result. Do not use JOIN operator.
5. Find all customers who live in the same city.
6. Find managers for each employee in Employees table.

##### Task 2.3. Using JOIN operator

1. Find sellers who serve 'Western' region (Region table).
2. Find all customers names from Customers table and amount of their orders from Orders table. Take into account that some customers do not have orders but they should be displayed in the query result. Order the query result by orders amount ascending.

##### Task 2.4. Using subqueries

1. Find all suppliers (CompanyName column in Suppliers table) who do not have at least one product in the warehouse (UnitsInStock column in Products table equals 0). Use nested SELECT for this query using IN operator.
2. Find all sellers who have more than 150 orders. Use nested SELECT.
3. Find all customers (Customers table) who do not have any orders (subquery in Orders table). Use EXISTS operator.

#### Task 3. Database and updates deployment

The task is to prepare for release several versions of the Northwind Extended project, to be exact - the project database. The original project is based on the original Northwind database.  
It is planned to release the following 3 versions within the Northwind Extended:
+ Version 1.0. Based on the original Northwin database.
+ Version 1.1. Add a table containing data of employees credit cards: card number, expired date, card holder, employee id.
+ Version 1.3. Add the following minor changes to the version 1.1:
  + Renaming Region to Regions.
  + Adding foundation date to Customers table.

All work is performed on SQL Server of any available edition.

##### Task 3.1. Using Alter Scripts

Create 2 sql-scripts which update the database by versions:
+ 1.0 -> 1.1
+ 1.1 -> 1.3

When executing the task make sure that the scripts can be applied many times without errors (for example, for the case of wrong re-update).

##### Task 3.2. Using SSDT

Import the original Northwind database to database project for Visual Studio (from the database or instnwnd.sql script). Import only metadata without data.  
Present 3 versions of the project based on SSDT with the changes as described above.  
Make sure that it is possible to update versions either sequentially or with a missing version using SSDT. Also, make sure that multiple update works.

##### Task 3.3. Inserting data during deployment

Complete the SSDT project with the Northwind database of version 1.3 with post deploy script to insert data to the following tables:
+ Categories
+ Suppliers
+ Products

It is allowed to use data from the source instnwnd.sql script (take the first 5-10 records).  
In this case the project should allow applying data several times (given that new data has not appeared in these and other tables).

  
### Part 2 - ADO.Net

#### General

Task goal is to create DAL (Data Access Layer) to the database Northwind.  
Requirements are the next:

+ DAL provides an object interface. That is, the data it receives and returns are usual POCO (plain old CLR object) objects.
+ DAL should be easily replaced with fakes or mocks for unit testing of the business layer.
+ DAL should support connection to any database (connection strings should not be hardcoded).
+ DAL should be designed in provider independent style. At the same time, it should be possible to replace some methods with specific ones for a particular DBMS.

At the same time, it is necessary to follow the next requirements when working with the Northwind database: 
+ An order (Orders db table) can be in the next states: 
  + New (not sent), if its field OrderDate == NULL 
  + In process (not completed), if its field ShippedDate == NULL 
  + Completed, if ShippedDate != NULL
+ In the test data provided with the Northwind distributive the field Picture in the table Categories contains pictures stored in the BMP format, but the first 78 bytes garbage which should be removed. 

#### About task

Imagine that this application is an operator workstation, but no UI is required.  
To perform the work the next is required: 
+ Create DAL.
+ Create a set of tests that demonstrate its work. This tests should work with database but rollback of the database to its original state is not required.

#### Task. Order management
 
Implement opportunity to manage orders via DAL: 
1. Show a list of orders ([Orders] table). In addition to the main fields the next fields should be returned:
    * An order status as an Enum field
2. Show detailed informaion about a specific order including data from the tables [Orders], [Order Details] and [Products] (it is required to retrieve product id and name) 
3. Create new orders
4. Modify existing orders. In this case: 
    * Do not change the next fields directly:
      * OrderDate and ShippedDate 
      * Order Status
    * In oders with the status "New" all fields and order list excepting the fields from the previous point can be changed
    * In oders with the statuses "In process" and "Completed" no fields must be changed
5. Delete orders with the statuses "New" and "In process"
6. Change order status. To implement this point it is needed to create special methods:
    * Move to process: set OrderDate
    * Move to completed: set ShippedDate 
7. Get order statistics using existing stored procedures:
    * CustOrderHist
    * CustOrdersDetail
