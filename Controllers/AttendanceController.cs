using Employee_Management_System.Model;
using Employee_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        // POST: api/attendance/mark - Mark attendance for an employee
        [HttpPost("mark")]
        public async Task<ActionResult<Attendance>> MarkAttendance(Attendance attendance)
        {
            // Example: You might have additional validation here.
            // For instance, checking if the employee exists can be done through another service.
            bool result = await _attendanceService.MarkAttendanceAsync(attendance);
            if (!result)
                return BadRequest("Error marking attendance or employee not found.");

            return CreatedAtAction(nameof(GetAttendanceRecord), new { id = attendance.AttendanceID }, attendance);
        }

        // GET: api/attendance/{id} - Get a specific attendance record
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> GetAttendanceRecord(int id)
        {
            var attendance = await _attendanceService.GetAttendanceRecordAsync(id);
            if (attendance == null)
                return NotFound();
            return Ok(attendance);
        }

        // GET: api/attendance?employeeId=1&date=2024-02-06 - View attendance records (optionally filtered)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendanceRecords(int? employeeId, DateTime? date)
        {
            var records = await _attendanceService.GetAttendanceRecordsAsync(employeeId, date);
            return Ok(records);
        }
    }
}
