using Employee_Management_System.Model;
using Employee_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            var departments = await _departmentService.GetDepartmentsAsync();
            return Ok(departments);
        }

        // GET: api/departments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _departmentService.GetDepartmentAsync(id);
            if (department == null)
                return NotFound();
            return Ok(department);
        }

        // POST: api/departments
        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment(Department department)
        {
            var result = await _departmentService.CreateDepartmentAsync(department);
            if (result)
            {
                return CreatedAtAction(nameof(GetDepartment), new { id = department.DepartmentID }, department);
            }
            else
            {
                return StatusCode(500, "Error creating department.");
            }
        }

        // PUT: api/departments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, Department department)
        {
            if (id != department.DepartmentID)
                return BadRequest("Department ID mismatch.");

            var result = await _departmentService.UpdateDepartmentAsync(department);
            if (result)
                return NoContent();
            else
                return StatusCode(500, "Error updating department.");
        }

        // DELETE: api/departments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var result = await _departmentService.DeleteDepartmentAsync(id);
            if (result)
                return NoContent();
            else
                return StatusCode(500, "Error deleting department.");
        }
    }
}
