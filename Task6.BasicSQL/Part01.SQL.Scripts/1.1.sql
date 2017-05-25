--1.1.1
select OrderID, ShippedDate, ShipVia from Orders 
where cast(ShippedDate as date) >= '1998-05-06' and ShipVia >= 2 

--1.1.2
select OrderID,
		case   
			when ShippedDate is null THEN 'Not Shipped'   
			else 'Shipped'
		end as ShippedDate
from Orders
where ShippedDate is null

--1.1.3
select OrderID as 'Order Number',
		case   
			when ShippedDate is null THEN 'Not Shipped'   
			else  cast(ShippedDate as varchar)
		end as 'Shipped Date'
from Orders
where ShippedDate is null or cast(ShippedDate as date) > '1998-05-06'
