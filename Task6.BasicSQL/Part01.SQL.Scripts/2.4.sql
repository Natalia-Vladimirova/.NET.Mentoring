--2.4.1
select CompanyName from Suppliers s
where SupplierID in 
(
	select SupplierID
	from Products
	where UnitsInStock = 0
)


--2.4.2
select e.EmployeeID, e.FirstName, e.LastName, o.OrdersCount from Employees e
join (
	select EmployeeID, count(*) as OrdersCount
	from Orders
	group by EmployeeID
	having count(*) > 150
) as o
on e.EmployeeID = o.EmployeeID


--2.4.3
select * from Customers c
where not exists 
(
	select *
	from Orders o
	where o.CustomerID = c.CustomerID
)
