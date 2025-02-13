using Employee_Management_System.Model;
using Employee_Management_System.Repositories;
using NLog;

namespace Employee_Management_System.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AttendanceService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceRecordsAsync(int? employeeId, DateTime? date)
        {
            try
            {
                var records = await _attendanceRepository.GetAllAsync();
                if (employeeId.HasValue)
                    records = records.Where(a => a.EmployeeID == employeeId.Value);
                if (date.HasValue)
                    records = records.Where(a => a.Date.Date == date.Value.Date);
                return records;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error retrieving attendance records.");
                throw;
            }
        }

        public async Task<Attendance> GetAttendanceRecordAsync(int id)
        {
            try
            {
                return await _attendanceRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error retrieving attendance with ID {id}");
                throw;
            }
        }

        public async Task<bool> MarkAttendanceAsync(Attendance attendance)
        {
            try
            {
                // Check if the employee exists before marking attendance.
                // This check would normally be done in a higher layer or via a different repository.
                // For simplicity, assume it's already been validated.

                await _attendanceRepository.AddAsync(attendance);
                return await _attendanceRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error marking attendance.");
                return false;
            }
        }
    }
}
