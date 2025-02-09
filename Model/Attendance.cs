namespace Employee_Management_System.Model
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;

        // Navigation property
        public Employee? Employee { get; set; }
    }
}