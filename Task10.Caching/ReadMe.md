## Task 10 - Caching

### General

Perform the task using data caching. Implement two options in each subtask:
+ In-process cache (e.g. using System.Runtime.Caching or System.Web.Caching).
+ Out-of-process / distributed cache (e.g. using [Redis on Windows](https://github.com/MSOpenTech/Redis) and a client based on [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis)).

### Subtask 1. Fibonacci numbers

Implement a procedure for calculating Fibonacci numbers. Make it store previously calculated values in the cache (and check for the presence of previously calculated values in the calculation). 

### Subtask 2. Caching data from the database

Modify the example (CachingSolutionsSamples) to cache data from the database:
+ add caching of 2-3 entities;
+ add invalidation cache policies: by time and using SQL monitors (depending on which options the particular library supports).

### Note

To avoid problems with data serialization (when using out-of-process cache) disable LazyLoading and Proxy generation when selecting via Entity Framework.
  
Run the following commands to enable broker for database:
```sql
USE master;
GO
ALTER DATABASE Northwind SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE;
GO
USE Northwind;
GO
```