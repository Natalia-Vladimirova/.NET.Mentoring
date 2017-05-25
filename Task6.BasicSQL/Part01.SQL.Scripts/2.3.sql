--2.3.1


--2.3.2
select c.ContactName, count(o.CustomerID) as OrdersCount
from Customers c
left join Orders o
on c.CustomerID = o.CustomerID
group by c.ContactName
order by count(o.CustomerID) asc
