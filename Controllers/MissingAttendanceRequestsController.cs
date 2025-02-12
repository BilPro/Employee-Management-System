using Employee_Management_System.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MissingAttendanceRequestsController : ControllerBase
    {
        private readonly EmployeeContext _context;
        public MissingAttendanceRequestsController(EmployeeContext context)
        {
            _context = context;
        }

        // POST: api/missingattendancerequests/mark - Mark missing attendance (submit a request)
        [HttpPost("mark")]
        public async Task<ActionResult<MissingAttendanceRequest>> MarkMissingAttendance(MissingAttendanceRequest request)
        {
            if (!await _context.Employees.AnyAsync(e => e.EmployeeID == request.EmployeeID))
                return BadRequest("Employee not found.");

            _context.MissingAttendanceRequests.Add(request);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMissingAttendanceRequest), new { id = request.RequestID }, request);
        }

        // GET: api/missingattendancerequests/{id} - Get a specific missing attendance request
        [HttpGet("{id}")]
        public async Task<ActionResult<MissingAttendanceRequest>> GetMissingAttendanceRequest(int id)
        {
            var request = await _context.MissingAttendanceRequests.FindAsync(id);
            if (request == null)
                return NotFound();
            return request;
        }

        // GET: api/missingattendancerequests - List all missing attendance requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MissingAttendanceRequest>>> GetMissingAttendanceRequests() =>
            await _context.MissingAttendanceRequests.ToListAsync();

        // PUT: api/missingattendancerequests/{id} - Approve or reject a missing attendance request
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMissingAttendanceRequestStatus(int id, [FromBody] MissingAttendanceRequest update)
        {
            if (id != update.RequestID)
                return BadRequest("Request ID mismatch.");

            var request = await _context.MissingAttendanceRequests.FindAsync(id);
            if (request == null)
                return NotFound("Missing attendance request not found.");

            // Update status and the ApprovedBy field (if applicable)
            request.Status = update.Status; // Expected values: "Approved" or "Rejected"
            request.ApprovedBy = update.ApprovedBy.ToString();

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
