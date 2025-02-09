namespace Employee_Management_System.Model
{
    public class MissingAttendanceRequest
    {
        public int RequestID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ApprovedBy { get; set; }

        // Navigation property
        public Employee? Employee { get; set; }
    }
}