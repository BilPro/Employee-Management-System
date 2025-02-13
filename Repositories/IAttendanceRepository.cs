using Employee_Management_System.Model;

namespace Employee_Management_System.Repositories
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<Attendance> GetByIdAsync(int id);
        Task AddAsync(Attendance attendance);
        void Update(Attendance attendance);
        void Delete(Attendance attendance);
        Task<bool> SaveChangesAsync();
    }
}
