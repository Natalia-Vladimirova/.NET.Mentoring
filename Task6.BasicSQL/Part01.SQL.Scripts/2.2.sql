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
select o.EmployeeID as SellerID, 
		(select FirstName + ' ' + LastName from Employees where EmployeeID = o.EmployeeID) as SellerName, 
		CustomerID as Customer, 
		count(*) as Amount 
from Orders o
where year(OrderDate) = '1998'
group by CustomerID, EmployeeID


--2.2.4
select * 
from (select City, 
		(select stuff(
			(select ', ' + ContactName
				from Customers
				where City = c.City
				for xml path('')), 1, 2, '')) as Customers,
		(select stuff(
			(select ', ' + e.FirstName + ' ' + e.LastName
				from Employees e
				where City = c.City
				for xml path('')), 1, 2, '')) as Sellers
from Customers c group by City) as r
where r.Sellers is not null and r.Customers is not null


--2.2.5
select City, (select stuff(
				(select ', ' + ContactName
				 from Customers
				 where City = c.City
				 for xml path('')), 1, 2, '')) as Customers
from Customers c
group by City
having count(*) > 1


--2.2.6
select e.EmployeeID as Seller, e.FirstName + ' ' + e. LastName as SellerName,
		m.EmployeeID as Manager, m.FirstName + ' ' + m.LastName as ManagerName
from Employees e
left join Employees m
on e.ReportsTo = m.EmployeeID
