--2.3.1
select distinct e.EmployeeID, e.FirstName + ' ' + e.LastName as FullName
from Employees e
join EmployeeTerritories et
on e.EmployeeID = et.EmployeeID
join Territories t
on et.TerritoryID = t.TerritoryID
join Region r
on r.RegionID = t.RegionID
where r.RegionDescription = 'Western'


--2.3.2
select c.ContactName, count(o.CustomerID) as OrdersCount
from Customers c
left join Orders o
on c.CustomerID = o.CustomerID
group by c.ContactName
order by count(o.CustomerID) asc
