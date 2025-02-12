using Employee_Management_System.Model;
using Employee_Management_System.Repositories;
using NLog;

namespace Employee_Management_System.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private static readonly NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            try
            {
                logger.Info("Retrieving employees success");
                return await _employeeRepository.GetAllEmployeesAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error retrieving employees.");
                throw; // rethrow or handle as needed
            }
        }

        public async Task<Employee> GetEmployeeAsync(int id)
        {
            try
            {
                return await _employeeRepository.GetEmployeeByIdAsync(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error retrieving employee with ID {id}");
                throw;
            }
        }

        public async Task<bool> CreateEmployeeAsync(Employee employee)
        {
            try
            {
                await _employeeRepository.AddEmployeeAsync(employee);
                await _employeeRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error creating employee.");
                return false;
            }
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                _employeeRepository.UpdateEmployee(employee);
                await _employeeRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error updating employee with ID {employee.EmployeeID}");
                return false;
            }
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                    return false;

                _employeeRepository.DeleteEmployee(employee);
                await _employeeRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error deleting employee with ID {id}");
                return false;
            }
        }
    }
}
