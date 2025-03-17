INSERT INTO Departments (id, name) VALUES
                                      (1, 'IT'),
                                      (2, 'HR'),
                                      (3, 'Finance');
INSERT INTO Employees (id, name, departmentId) VALUES
                                                    (1, 'Alice', 1),
                                                    (2, 'Bob', 2),
                                                    (3, 'Charlie', 1),
                                                    (4, 'David', 3);
INSERT INTO Sops (id, name, departmentId) VALUES
                                              (1, 'Security SOP', 1),
                                              (2, 'Recruitment SOP', 2),
                                              (3, 'Financial Auditing SOP', 3);
INSERT INTO Tasks (id, name, procedure, IsAssigned, SopId) VALUES
                                                                 (1, 'Implement Firewall', 'Install and configure firewall', 1, 1),
                                                                 (2, 'Recruitment Process', 'Conduct interviews and evaluations', 1, 2),
                                                                 (3, 'Financial Review', 'Review financial statements', 0, 3),
                                                                 (4, 'Network Security', 'Monitor and update security policies', 1, 1);
INSERT INTO EmployeeTasks (taskId, employeeId, priorityCode) VALUES
                                                                     (1, 1, 'High'),
                                                                     (2, 2, 'Medium'),
                                                                     (3, 4, 'Low'),
                                                                     (4, 3, 'High'),
                                                                     (2, 1, 'Low');
SELECT * FROM Departments;
SELECT * FROM Employees;
SELECT * FROM Sops;
SELECT * FROM Tasks;
SELECT * FROM EmployeeTasks;


