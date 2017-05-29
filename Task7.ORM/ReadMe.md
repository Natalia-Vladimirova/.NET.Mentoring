## Task 7 - ORM

### General Info

To perform the task use Northwind database and one of the Micro-ORM (Dapper, linq2db - more preferable).

### Task 1

Create object model and describe mapping for Northwind database tables.  
Pay attention to the next:
+ setup primary keys;
+ description of relations between entities and implementation of relations 1-M, M-M.

### Task 2

Show the following queries:
+ a list of products with a category and supplier;
+ a list of employees with a region which they are responsible for;
+ statistics by regions: a number of employees per region;
+ a list "employee - carriers who employee worked with" (according to orders).

### Task 3

Show possibility of changes:
+ Add new employee and specify a list of territories which he is responsible for.
+ Move products from one category to another category.
+ Add a list of products with their suppliers and categories (bulk entry). In this case, if a supplier or category with this name exists use them. Otherwise, create new ones.
+ Replace a product with a similar one in orders which are not processed yet (consider orders where ShippedDate is null).
