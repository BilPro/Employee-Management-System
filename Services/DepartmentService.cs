using Employee_Management_System.Model;
using Employee_Management_System.Repositories;
using NLog;

namespace Employee_Management_System.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            try
            {
                return await _departmentRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error retrieving departments.");
                throw;
            }
        }

        public async Task<Department> GetDepartmentAsync(int id)
        {
            try
            {
                return await _departmentRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error retrieving department with ID {id}");
                throw;
            }
        }

        public async Task<bool> CreateDepartmentAsync(Department department)
        {
            try
            {
                await _departmentRepository.AddAsync(department);
                return await _departmentRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error creating department.");
                return false;
            }
        }

        public async Task<bool> UpdateDepartmentAsync(Department department)
        {
            try
            {
                _departmentRepository.Update(department);
                return await _departmentRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error updating department with ID {department.DepartmentID}");
                return false;
            }
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(id);
                if (department == null)
                    return false;

                _departmentRepository.Delete(department);
                return await _departmentRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error deleting department with ID {id}");
                return false;
            }
        }
    }
}
