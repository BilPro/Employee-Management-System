namespace Employee_Management_System.Model
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public int DepartmentID { get; set; }
        public DateTime HireDate { get; set; }

        // Navigation property
        public Department? Department { get; set; }
    }
}