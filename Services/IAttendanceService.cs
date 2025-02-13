using Employee_Management_System.Model;

namespace Employee_Management_System.Services
{
    public interface IAttendanceService
    {
        Task<IEnumerable<Attendance>> GetAttendanceRecordsAsync(int? employeeId, DateTime? date);
        Task<Attendance> GetAttendanceRecordAsync(int id);
        Task<bool> MarkAttendanceAsync(Attendance attendance);
    }
}
