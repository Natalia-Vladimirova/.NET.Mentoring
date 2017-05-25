-- 1.0 -> 1.1
use Northwind;

if object_id(N'EmployeeCreditCards', N'U') is null
create table EmployeeCreditCards
(
	CreditCardID int identity (1, 1) not null,
	CardNumber bigint not null,
	ExpiredDate datetime not null,
	CardHolder varchar not null,
	EmployeeID int not null,
	constraint PK_EmployeeCreditCards primary key (CreditCardID),
	constraint FK_EmployeeCreditCards_Employees foreign key (EmployeeID) references Employees (EmployeeID)
);
