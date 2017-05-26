--1.2.1
select ContactName, Country from Customers
where Country in ('USA', 'Canada')
order by ContactName, Country

--1.2.2
select ContactName, Country from Customers
where Country not in ('USA', 'Canada')
order by ContactName

--1.2.3
select distinct Country from Customers
order by Country desc
