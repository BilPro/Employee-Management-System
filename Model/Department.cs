﻿namespace Employee_Management_System.Model
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }

        // Navigation property: A department may have many employees.
        public ICollection<Employee>? Employees { get; set; }
    }
}