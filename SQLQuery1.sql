create database FinancialCrmDb
use FinancialCrmDb
create table  Categories
(
CategoryId int primary key identity(1,1),
CategoryName nvarchar(50) not null 
);

insert into Categories(CategoryName) 
values ('Faturalar'),('Yeme-Ýçme'),('Ulaþým'),('Okul'),('Kira'),('Spor')

select *from Categories

CREATE  table Banks 
(
BankId int primary key identity(1,1),
BankAccountNumber nvarchar(50) not null ,
BankTitle nvarchar(50) not null ,
BankBalance nvarchar(50)not null
);
ALTER TABLE Banks
ALTER COLUMN BankBalance decimal(18,2) NOT NULL;


INSERT INTO Banks (BankAccountNumber, BankTitle, BankBalance)
VALUES 
('TR1234567890123456789012', 'Ziraat Bankasý', '0'),
('TR9876543210987654321098', 'Garanti BBVA', '0'),
('TR4567890123456789012345', 'Ýþ Bankasý', '0');


CREATE TABLE BankProcesses (
    BankProcessId INT PRIMARY KEY IDENTITY(1,1),
    Description_ NVARCHAR(50) NOT NULL,
    ProcessDate DATE NOT NULL,
    ProcessType NVARCHAR(50) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL DEFAULT 0,
    BankId INT NOT NULL,
    CONSTRAINT FK_BankProcesses_Bank FOREIGN KEY (BankId) REFERENCES Banks(BankId)
);

INSERT INTO BankProcesses (Description_, ProcessDate, ProcessType, Amount, BankId)
VALUES 
(N'Babamdan gelen ödeme', '2025-01-06', N'Gelen Havale', 2500.00, 1),

(N'Kyk Burs Ödemesi', '2025-01-07', N'Gelen Havale', 3000.00, 1),

(N'Ablamdan gelen para', '2025-01-08', N'Gelen Havale', 500.00, 3);

INSERT INTO BankProcesses (Description_, ProcessDate, ProcessType, Amount, BankId)
VALUES 
(N'Belediye burs ödemesi', '2025-01-08', N'Gelen Havale', 2000.00, 2),

(N'Part time iþ maaþý', '2025-01-08', N'Gelen Havale', 7500.00, 3),

(N'Kyk Burs Ödemesi', '2025-01-09', N'Gelen Havale', 3000.00, 1);

select *from BankProcesses

--ilk veriyi almak için?
select top(1) * from BankProcesses order by BankProcessId desc


CREATE Table Bills
(
BillId int primary KEY identity(1,1),
BillTitle nvarchar(50) not null,
BillAmount  decimal(18,2) not null ,
BillPeriod nvarchar(50) not null
);
insert into Bills (BillTitle,BillAmount,BillPeriod)values 
('Elektrik Faturasý',350.00,'Ocak 2025'),
('Doðalgaz Faturasý',750.00,'Ocak 2025'),
('Su faturasý',260.00,'Ocak 2025');
select *from Bills

create  table Spendings
(
SpendingId int primary key identity(1,1),
SpendingTitle nvarchar(250),
SpendingAmount decimal(18,2),
SpendingDate Date,
CategoryId int ,
Constraint FK_Spending_Categories foreign key (CategoryId) references Categories(CategoryId)
);
 
 insert into Spendings (SpendingTitle,SpendingAmount,SpendingDate,CategoryId)
 values ('Migros Alýþveriþ',350.0,'2025-01-02',2),('Damacana Su',110.0,'2025-01-03',2)
        ,('Aylýk AKbil ',350.0,'2025-01-04',3)

create  table Users
(
UserId int primary Key identity(1,1),
UserName nvarchar(50) not null,
UserPassword nvarchar(50) not null
);

insert into Users (UserName,UserPassword) values ('admin','1234');

CREATE PROCEDURE GetAllSpendings
AS
BEGIN
    SELECT 
        S.SpendingId,
        S.SpendingTitle,
        S.SpendingAmount,
        S.SpendingDate,
        C.CategoryName
    FROM Spendings S
    LEFT JOIN Categories C ON S.CategoryId = C.CategoryId
    ORDER BY S.SpendingDate DESC;
END;

