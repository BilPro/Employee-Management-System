using Employee_Management_System.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly EmployeeContext _context;
        public AttendanceController(EmployeeContext context)
        {
            _context = context;
        }

        // POST: api/attendance/mark - Mark attendance for an employee
        [HttpPost("mark")]
        public async Task<ActionResult<Attendance>> MarkAttendance(Attendance attendance)
        {
            if (!await _context.Employees.AnyAsync(e => e.EmployeeID == attendance.EmployeeID))
                return BadRequest("Employee not found.");

            _context.AttendanceRecords.Add(attendance);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAttendanceRecord), new { id = attendance.AttendanceID }, attendance);
        }

        // GET: api/attendance/{id} - Get a specific attendance record
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> GetAttendanceRecord(int id)
        {
            var attendance = await _context.AttendanceRecords.FindAsync(id);
            if (attendance == null)
                return NotFound();
            return attendance;
        }

        // GET: api/attendance?employeeId=1&date=2024-02-06 - View attendance records (optionally filtered)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendanceRecords(int? employeeId, DateTime? date)
        {
            var query = _context.AttendanceRecords.AsQueryable();
            if (employeeId.HasValue)
                query = query.Where(a => a.EmployeeID == employeeId.Value);
            if (date.HasValue)
                query = query.Where(a => a.Date.Date == date.Value.Date);
            return await query.ToListAsync();
        }
    }
}
