USE Company_SD;

-- Display the data of the department which has the smallest employee ID over all employees' ID.

SELECT TOP 1 d.*
FROM Departments d
ORDER BY (SELECT MIN(e.SSN) FROM Employee e WHERE e.Dno = d.Dnum);

-- For each department, retrieve the department name and the maximum, minimum and average salary of its employees.

SELECT d.Dname AS DepartmentName,
       (SELECT MAX(e.Salary) FROM Employee e WHERE e.Dno = d.Dnum) AS MaxSalary,
       (SELECT MIN(e.Salary) FROM Employee e WHERE e.Dno = d.Dnum) AS MinSalary,
       (SELECT AVG(e.Salary) FROM Employee e WHERE e.Dno = d.Dnum) AS AvgSalary
FROM Departments d;

-- List the full name of all managers who have no dependents.

SELECT e.Fname + ' ' + e.Lname AS FullName
FROM Employee e
JOIN Departments d ON d.MGRSSN = e.SSN
WHERE e.SSN NOT IN (SELECT ESSN FROM Dependent);

/* For each department-- if its average salary is less than the average salary of all employees 
display its number, name and number of its employees. */

SELECT d.Dnum, d.Dname,
       (SELECT COUNT(*) FROM Employee e WHERE e.Dno = d.Dnum) AS NumEmployees
FROM Departments d
WHERE (SELECT AVG(Salary) FROM Employee e WHERE e.Dno = d.Dnum) < (SELECT AVG(Salary) FROM Employee);

/* Retrieve a list of employee’s names and the projects names they are working on ordered by
department number and within each department, ordered alphabetically by last name, first name. */

SELECT e.Fname + ' ' + e.Lname AS FullName, p.Pname
FROM Employee e
JOIN Works_for w ON w.ESSn = e.SSN
JOIN Project p ON p.Pnumber = w.Pno
ORDER BY e.Dno, e.Lname, e.Fname;

/* In the department table insert new department called "DEPT IT”, with id 100, employee with
SSN = 112233 as a manager for this department. The start date for this manager is '1-11-2006' */

INSERT INTO Departments VALUES ('DEPT IT', 100, '112233', '2006-11-01');

/* Do what is required if you know that : Mrs.Noha Mohamed(SSN=968574) moved to be the
manager of the new department (id = 100),
and they give you(your SSN =102672) her position (Dept. 20 manager) */

-- a - First try to update her record in the department table
UPDATE Departments SET MGRSSN = 968574 WHERE Dnum = 100;
UPDATE Employee SET Dno = 100 WHERE SSN = 968574;

-- b - Update your record to be department 20 manager.
UPDATE Departments SET MGRSSN = 102672 WHERE Dnum = 20;
UPDATE Employee SET Dno = 20 WHERE SSN = 102672;

-- c - Update the data of employee number=102660 to be in your teamwork (he will be supervised by you) (your SSN =102672)
UPDATE Employee SET Superssn = 102672 WHERE SSN = 102660;

-- Display the department number, department name, and the manager’s SSN, first name, and last name.

SELECT d.Dnum, d.Dname, d.MGRSSN, e.Fname, e.Lname
FROM Departments d
JOIN Employee e ON d.MGRSSN = e.SSN;

-- Display each department name with the projects that belong to it.

SELECT d.Dname, p.Pname
FROM Departments D
JOIN Project p ON p.Dnum = d.Dnum;

-- Display all dependent information with the full name of the related employee.

SELECT d.*, e.Fname + ' ' + e.Lname AS FullName
FROM Dependent d
JOIN Employee e ON d.ESSN = e.SSN;

-- Display the project number, project name, and location for projects located in Cairo or Alex.

SELECT Pnumber, Pname, Plocation 
FROM Project WHERE Plocation = 'Cairo' OR Plocation = 'Alex';

-- Display all projects whose names start with the letter A.

SELECT Pname
FROM Project WHERE Pname LIKE 'A%';

-- Display all employees who work in department 30 and whose salary is between 1000 and 2000.

SELECT e.*
FROM Employee e WHERE Dno = 30 AND Salary BETWEEN 1000 AND 2000;

-- Display the first names of employees in department 10 who worked 10 hours or more on the project named Al Rabwah

SELECT e.Fname
FROM Employee e 
JOIN Works_for w ON w.ESSn = e.SSN
JOIN Project p ON w.Pno = p.Pnumber
WHERE w.Hours >= 10 AND p.Pname = 'Al Rabwah';

-- Display the first names of employees who are supervised by an employee named Kamel.

SELECT e1.Fname
FROM Employee e1
JOIN Employee e2 ON e1.SuperSSN = e2.SSN
WHERE e2.Fname = 'Kamel';

-- Display employees’ first names along with the project names they work on, ordered by the project name.

SELECT e.Fname, p.Pname
FROM Employee e
JOIN Works_for w ON w.ESSn = e.SSN
JOIN Project p ON w.Pno = p.Pnumber
ORDER BY P.Pname;

-- Display the project number, project name, department name, manager’s last name, and manager’s address.

SELECT p.Pnumber, p.Pname, d.Dname, e.Lname, e.Address
FROM Project p
JOIN Departments d ON p.Dnum = d.Dnum
JOIN Employee e ON d.MGRSSN = e.SSN;