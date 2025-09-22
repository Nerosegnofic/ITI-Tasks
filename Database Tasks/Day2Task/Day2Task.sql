CREATE DATABASE CompanyTask;

USE CompanyTask;

-- Database & Tables

CREATE TABLE Employees (
	Id INT PRIMARY KEY,
	FirstName VARCHAR(20) NOT NULL,
	LastName VARCHAR(20),
	Address VARCHAR(255),
	Gender CHAR(1),
	BirthDate DATE,
	SupervisorId INT,
	DepartmentNumberId INT,
	CONSTRAINT FK_Supervisor
		FOREIGN KEY (SupervisorId) REFERENCES Employees(Id),
	CONSTRAINT FK_DepartmentNumber
		FOREIGN KEY (DepartmentNumberId) REFERENCES Departments(DNumber)
);

CREATE TABLE Departments (
	DNumber INT PRIMARY KEY,
	DName VARCHAR(20) NOT NULL,
	ManagerId INT,
	HiringDate DATE,
	CONSTRAINT FK_Manager
		FOREIGN KEY (ManagerId) REFERENCES Employees(Id)
);

CREATE TABLE Projects (
	PNumber INT PRIMARY KEY,
	PName VARCHAR(255) NOT NULL,
	Location VARCHAR(255),
	City VARCHAR(255),
	DeptNum INT,
	CONSTRAINT FK_Dept
		FOREIGN KEY (DeptNum) REFERENCES Departments(DNumber)
);

CREATE TABLE Employee_Projects (
	EId INT,
	PNum INT,
	WorkingHOURS INT,
	PRIMARY KEY (EId, PNum),
	CONSTRAINT FK_E
		FOREIGN KEY (EId) REFERENCES Employees(Id),
	CONSTRAINT FK_P
		FOREIGN KEY (PNum) REFERENCES Projects(PNumber)
);

INSERT INTO Employees VALUES (0, 'Ahmed', 'Abdelnabi', 'Egypt', 'M', '2004-04-16', NULL, NULL);
INSERT INTO Employees VALUES (1, 'Sara', 'Hassan', 'Cairo', 'F', '1998-07-12', NULL, NULL);
INSERT INTO Employees VALUES (2, 'Omar', 'Ali', 'Giza', 'M', '1995-03-25', NULL, NULL);
INSERT INTO Employees VALUES (3, 'Nour', 'Ibrahim', 'Alex', 'F', '1992-09-10', NULL, NULL);
INSERT INTO Employees VALUES (4, 'Hany', 'Khaled', 'Cairo', 'M', '2000-01-05', NULL, NULL);

UPDATE Employees SET SupervisorID = 1 WHERE ID = 0;
UPDATE Employees SET SupervisorID = 2 WHERE ID = 1;
UPDATE Employees SET SupervisorID = 3 WHERE ID = 2;
UPDATE Employees SET SupervisorID = 4 WHERE ID = 3;
UPDATE Employees SET SupervisorID = NULL WHERE ID = 4;

UPDATE Employees SET DepartmentNumberID = 0 WHERE ID = 0;
UPDATE Employees SET DepartmentNumberID = 0 WHERE ID = 1;
UPDATE Employees SET DepartmentNumberID = 1 WHERE ID = 2;
UPDATE Employees SET DepartmentNumberID = 1 WHERE ID = 3;
UPDATE Employees SET DepartmentNumberID = 2 WHERE ID = 4;

-- Insert Data

INSERT INTO Departments VALUES (0, 'CS', 0, '2024-03-16');
INSERT INTO Departments VALUES (1, 'IT', 1, '2023-02-18');
INSERT INTO Departments VALUES (2, 'HR', 2, '2021-06-20');

INSERT INTO Projects VALUES (0, 'Project0', NULL, NULL, 0);
INSERT INTO Projects VALUES (1, 'Project1', NULL, NULL, 1);
INSERT INTO Projects VALUES (2, 'Project2', NULL, NULL, 2);

INSERT INTO Employee_Projects VALUES (0, 0, 30);
INSERT INTO Employee_Projects VALUES (1, 0, 35);
INSERT INTO Employee_Projects VALUES (2, 1, 40);
INSERT INTO Employee_Projects VALUES (3, 1, 25);
INSERT INTO Employee_Projects VALUES (4, 2, 44);

-- Queries

SELECT * FROM Employees WHERE DepartmentNumberId = 1;
SELECT FirstName, LastName FROM Employees WHERE Address = 'Cairo';
SELECT * FROM Employees WHERE BirthDate BETWEEN '1999-01-01' AND '2002-01-01';
SELECT PName FROM Projects WHERE PNumber IN (SELECT PNum FROM Employee_Projects WHERE EId = 2);
SELECT * FROM Employees ORDER BY LastName DESC;
SELECT * FROM Employees WHERE SupervisorId IS NULL;

-- Update & Delete

UPDATE Employees SET Address = 'Alex' WHERE Id = 3;
DELETE FROM Employees WHERE Id = 4;

-- Extra

SELECT * FROM Employees WHERE FirstName LIKE 'A%';
SELECT DISTINCT Address FROM Employees;
SELECT * FROM Employees ORDER BY FirstName, LastName DESC;
SELECT * FROM Employees WHERE FirstName LIKE '____';
SELECT * FROM Employees WHERE FirstName LIKE 'A_m%';
SELECT * FROM Employees WHERE FirstName LIKE 'A_m%';
SELECT * FROM Employees WHERE FirstName LIKE '[A-M]%';
SELECT * FROM Employees WHERE FirstName LIKE '[^C]%';

CREATE DATABASE SchoolDB;
USE SchoolDB;

CREATE TABLE Student (
	Id INT PRIMARY KEY,
	Name VARCHAR(20)
);

INSERT INTO Student VALUES (0, 'Ahmed');
INSERT INTO Student VALUES (1, 'Muhammad');