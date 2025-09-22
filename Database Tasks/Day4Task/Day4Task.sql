USE Company_SD;

-- 1-Get the number of employees in each department.

SELECT Dno AS DepartmentNumber, COUNT(*) AS NumberOfEmployees
FROM Employee
GROUP BY Dno;

-- 2-Get the minimum salary in each department.

SELECT MIN(Salary) AS MinSalary
FROM Employee
GROUP BY Dno;

-- 3-Get the average salary for each department.

SELECT AVG(Salary) AS AvgSalary
FROM Employee 
GROUP BY Dno;

-- 4-Get the departments that have more than 3 employees.

SELECT d.Dname AS DepartmentName, COUNT(*) AS NumberOfEmployees
FROM Departments d
JOIN Employee e ON d.Dnum = e.Dno
GROUP BY d.Dname
HAVING COUNT(*) > 3;

-- 5-Get the projects that have more than 2 employees working on them.

SELECT w.Pno AS ProjectNumber, p.Pname AS ProjectName, COUNT(*) AS NumberOfEmployees
FROM Works_for w
JOIN Project p ON w.Pno = p.Pnumber
JOIN Employee e ON w.ESSn = e.SSN
GROUP BY w.Pno, p.Pname
HAVING COUNT(*) > 2;

-- 6-Get the employees who have the maximum salary.

Select Fname
FROM Employee
WHERE Salary = (SELECT Max(Salary) FROM Employee);

-- 7-Get the employees who have a salary greater than the average salary.

SELECT e.*
FROM Employee e
WHERE Salary > (SELECT AVG(Salary) FROM Employee);

-- 8-Get the names of employees who work on the same projects as "John Smith".

SELECT e.Fname + ' ' + e.Lname AS FullName
FROM Employee e
JOIN Works_for w ON e.SSN = w.ESSn
WHERE w.Pno IN (
    SELECT w2.Pno
    FROM Works_for w2
    JOIN Employee e2 ON w2.ESSn = e2.SSN
    WHERE e2.Fname = 'John' AND e2.Lname = 'Smith'
);

-- 9-Get the departments that control the projects where "Alice" works

SELECT d.Dname
FROM Departments d
JOIN Project p ON p.Dnum = d.Dnum
WHERE p.Pnumber IN (
    SELECT w.Pno
    FROM Works_for w
    JOIN Employee e ON w.ESSn = e.SSN
    WHERE e.Fname = 'Alice');

-- 10-Create a view to display employees with their department name and salary.

CREATE VIEW EmployeeDeptSalary AS
SELECT e.Fname + ' ' + e.Lname AS FullName, d.Dname AS DepartmentName, e.Salary
FROM Employee e
JOIN Departments d ON d.Dnum = e.Dno;

-- 11-Select all data from the view.

SELECT * FROM EmployeeDeptSalary;

-- 12-Create a view for projects with their department name.

CREATE VIEW [ProjectDept] AS
SELECT p.Pname, d.Dname
FROM Project p
JOIN Departments d ON p.Dnum = d.Dnum;

-- 13-Display employees ordered by salary descending.

SELECT * FROM Employee
ORDER BY Salary DESC;

-- 14-Display projects ordered by project name alphabetically.

SELECT * FROM Project
ORDER BY Pname;

-- 15-Get the top 3 highest paid employees.

SELECT TOP 3 * FROM Employee 
ORDER BY Salary DESC;

-- 16-Get the top 2 departments with the largest number of employees.

SELECT TOP 2 d.Dname, COUNT(*) AS NumEmployees
FROM Departments d
JOIN Employee e ON e.Dno = d.Dnum
GROUP BY d.Dname
ORDER BY COUNT(*) DESC;

-- 17-Get each project with the number of employees working on it.

SELECT p.Pname, COUNT(*) AS NumEmployees
FROM Project p
JOIN Works_for w ON w.Pno = p.Pnumber
JOIN Employee e ON e.SSN = w.ESSn
GROUP BY p.Pname;

-- 18-Create a simple view for courses & Delete a project from the view

CREATE VIEW [CourseView] AS
SELECT Pnumber, Pname, Dnum
FROM Project;

DELETE FROM CourseView
WHERE Pname = 'Al Rehab';

-- 19-Create a view for employees & Increase salary of 'John Smith' by 10%

CREATE VIEW [EmpView] AS
SELECT SSN, Fname, Lname, Salary
FROM Employee;

UPDATE EmpView SET Salary = Salary * 1.1 WHERE Fname = 'John' AND Lname = 'Smith';

-- 20- Get employees who earn more than the average salary of their own department.

SELECT e.*
FROM Employee e
WHERE e.Salary > (
    SELECT AVG(e2.Salary)
    FROM Employee e2
    WHERE e2.Dno = e.Dno
);