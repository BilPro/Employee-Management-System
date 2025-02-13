using Employee_Management_System.Model;
using Employee_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MissingAttendanceRequestsController : ControllerBase
    {
        private readonly IMissingAttendanceRequestService _service;

        public MissingAttendanceRequestsController(IMissingAttendanceRequestService service)
        {
            _service = service;
        }

        // POST: api/missingattendancerequests/mark - Mark missing attendance (submit a request)
        [HttpPost("mark")]
        public async Task<ActionResult<MissingAttendanceRequest>> MarkMissingAttendance(MissingAttendanceRequest request)
        {
            // Optionally validate that the employee exists in a separate service or repository.
            var result = await _service.MarkMissingAttendanceAsync(request);
            if (!result)
                return BadRequest("Error marking missing attendance or employee not found.");

            return CreatedAtAction(nameof(GetMissingAttendanceRequest), new { id = request.RequestID }, request);
        }

        // GET: api/missingattendancerequests/{id} - Get a specific missing attendance request
        [HttpGet("{id}")]
        public async Task<ActionResult<MissingAttendanceRequest>> GetMissingAttendanceRequest(int id)
        {
            var request = await _service.GetRequestByIdAsync(id);
            if (request == null)
                return NotFound();
            return Ok(request);
        }

        // GET: api/missingattendancerequests - List all missing attendance requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MissingAttendanceRequest>>> GetMissingAttendanceRequests()
        {
            var requests = await _service.GetAllRequestsAsync();
            return Ok(requests);
        }

        // PUT: api/missingattendancerequests/{id} - Approve or reject a missing attendance request
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMissingAttendanceRequestStatus(int id, [FromBody] MissingAttendanceRequest update)
        {
            if (id != update.RequestID)
                return BadRequest("Request ID mismatch.");

            var result = await _service.UpdateRequestStatusAsync(id, update);
            if (!result)
                return NotFound("Missing attendance request not found or error updating.");
            return NoContent();
        }
    }
}
