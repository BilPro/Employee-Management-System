using Employee_Management_System.Model;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Repositories
{
    public class MissingAttendanceRequestRepository : IMissingAttendanceRequestRepository
    {
        private readonly EmployeeContext _context;
        public MissingAttendanceRequestRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MissingAttendanceRequest>> GetAllAsync()
        {
            return await _context.MissingAttendanceRequests.ToListAsync();
        }

        public async Task<MissingAttendanceRequest> GetByIdAsync(int id)
        {
            return await _context.MissingAttendanceRequests.FindAsync(id);
        }

        public async Task AddAsync(MissingAttendanceRequest request)
        {
            await _context.MissingAttendanceRequests.AddAsync(request);
        }

        public void Update(MissingAttendanceRequest request)
        {
            _context.MissingAttendanceRequests.Update(request);
        }

        public void Delete(MissingAttendanceRequest request)
        {
            _context.MissingAttendanceRequests.Remove(request);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
