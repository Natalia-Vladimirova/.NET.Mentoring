--1.3.1
select distinct OrderID from [Order Details]
where Quantity between 3 and 10

--1.3.2
select CustomerID, Country from Customers
where substring(Country, 1, 1) between 'b' and 'g'
order by Country

--1.3.3
select CustomerID, Country from Customers
where substring(Country, 1, 1) >= 'b' and substring(Country, 1, 1) <= 'g'
order by Country
