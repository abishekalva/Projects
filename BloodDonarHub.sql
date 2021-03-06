/****** Script for SelectTopNRows command from SSMS  ******/
Create TABLE Users
(
	[EmployeeID] Varchar(50)Not Null,
	[EmployeeName] Varchar(50) Not Null,
	[Password] Varchar(50) Not Null,
	[City] Varchar(100)Not Null,
	[BloodGroup] Varchar(10)Not Null,
	[Email]  Varchar(100)Not Null,
	[PhoneNumber]  Varchar(50)Not Null,
	CONSTRAINT pk_EmpID PRIMARY KEY([EmployeeID])
);

Create TABLE Requests
(
	[RequestID] INT IDENTITY(1,1),
	[EmployeeID] Varchar(50) Not Null,
	[BloodGroupRequested] Varchar(10) Not Null,
	[Location] Varchar(50) Not Null,
	CONSTRAINT pk_RID PRIMARY KEY([RequestID]),
	CONSTRAINT fk_EmpID FOREIGN KEY([EmployeeID])REFERENCES Users([EmployeeID]) 
	On Update Cascade
	On Delete Cascade
);

Create Table Donors
( 
[EmployeeID] Varchar(50)Not Null,
[LastDonatedDate] date Not Null,
[DonateCount] Int not null,
CONSTRAINT fkdonor_EmpID FOREIGN KEY([EmployeeID])REFERENCES Users([EmployeeID])
On Update Cascade
On Delete Cascade
);