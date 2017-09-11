Drop Table Menu
create table MENU
(
   MenuID int Identity(1,1) PRIMARY KEY,
   FoodName varchar(25) NOT NULL unique,
   Price Decimal(6,2) NOT NULL,
   Category varchar(30) NOT null
)

drop table DISCOUNT
create table DISCOUNT
(
   DiscountID int Identity(1,1) PRIMARY KEY,
   DiscountName varchar(25) NOT NULL unique,
   DiscountType varchar(20) NOT NULL,
   DiscountAmount decimal(5,2) NOT NULL,
)

drop table TAX
create table TAX
(
	TaxID int Identity(1,1) PRIMARY KEY,
	TaxName VARCHAR(25) NOT NULL unique,
	TaxAmount INT NOT NULL
)

drop table CUSTOMERORDER
create table CUSTOMERORDER
(
	OrderID INT Identity(1,1) PRIMARY KEY,
	OrderDate varchar(25),
	ServerName VARCHAR(50),
	Subtotal DECIMAL(6,2),
	DiscountName varchar(25),
	DiscountAmount DECIMAL(5,2),
	Pretax DECIMAL(6,2),
	Tax DECIMAL(6,2),
	Total DECIMAL(6,2)
)

drop table ORDERITEM
create table ORDERITEM
(
	DetailID int Identity(1,1) Primary Key,
	OrderID int,
	FoodName varchar(25),
	FoodPrice decimal
)