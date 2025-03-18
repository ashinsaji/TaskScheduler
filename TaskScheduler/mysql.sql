INSERT INTO Departments (Id, Name) VALUES
                                      (1, 'Fabrication');

INSERT INTO Employees (Id, Name, DepartmentId) VALUES
                                                    (1, 'Bryce', 1),
                                                    (2, 'Daniel', 1),
                                                    (3, 'Josue', 1),
                                                    (4, 'Alex', 1);
INSERT INTO Sops (Id, Name, DepartmentId) VALUES 
                                              (1, 'Add projects into twi and create quality plan', 1);
INSERT INTO Tasks (Id, Name, Tasks.Procedure, SopId) VALUES 
                                                   (Id, 'Create an Excel Import for Spooling', 'Create an Excel import for Spooling', 1);
INSERT INTO EmployeeTasks (TaskId, EmployeeId, PriorityCode) VALUES
                                                                      (1, 1, 'P'),
                                                                      (1, 2, 'B1'),
                                                                      (1, 3, 'P'),
                                                                      (1, 4, 'B2');
