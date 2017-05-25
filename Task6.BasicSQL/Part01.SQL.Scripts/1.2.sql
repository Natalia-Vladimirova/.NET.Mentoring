--1.2.1
select CustomerID, Country from Customers
where Country in ('USA', 'Canada')
order by CustomerID, Country

--1.2.2
select CustomerID, Country from Customers
where Country not in ('USA', 'Canada')
order by CustomerID

--1.2.3
select distinct Country from Customers
order by Country desc
