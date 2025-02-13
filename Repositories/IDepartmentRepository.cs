using Employee_Management_System.Model;

namespace Employee_Management_System.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(int id);
        Task AddAsync(Department department);
        void Update(Department department);
        void Delete(Department department);
        Task<bool> SaveChangesAsync();
    }
}
