--2.2.1
select year(ShippedDate) as Year, count(*) as Total 
from Orders
group by year(ShippedDate)

--check
select count(*) from Orders


--2.2.2
select (select LastName + ' ' + FirstName from Employees where EmployeeID = o.EmployeeID) as Seller, count(*) as Amount 
from Orders o
group by o.EmployeeID
order by count(*) desc


--2.2.3
select EmployeeID, CustomerID, count(*) as Amount from Orders
where year(OrderDate) = '1998'
group by CustomerID, EmployeeID


--2.2.4


--2.2.5


--2.2.6
select EmployeeID as Seller, (select EmployeeID from Employees where e.ReportsTo = EmployeeID) as Manager
from Employees e
