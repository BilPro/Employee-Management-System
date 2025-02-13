using Employee_Management_System.Model;

namespace Employee_Management_System.Repositories
{
    public interface IMissingAttendanceRequestRepository
    {
        Task<IEnumerable<MissingAttendanceRequest>> GetAllAsync();
        Task<MissingAttendanceRequest> GetByIdAsync(int id);
        Task AddAsync(MissingAttendanceRequest request);
        void Update(MissingAttendanceRequest request);
        void Delete(MissingAttendanceRequest request);
        Task<bool> SaveChangesAsync();
    }
}
