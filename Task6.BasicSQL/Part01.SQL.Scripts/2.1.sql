--2.1.1
select sum(UnitPrice * Quantity * (1 - Discount)) as Totals
from [Order Details]

--2.1.2
select count(*) - count(ShippedDate) from Orders

--2.1.3
select count(distinct CustomerID) from Orders
