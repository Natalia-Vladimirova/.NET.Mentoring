## Task 6 - Basic SQL

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
