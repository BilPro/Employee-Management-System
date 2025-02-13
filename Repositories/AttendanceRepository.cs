using Employee_Management_System.Model;

namespace Employee_Management_System.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Employee_Management_System.Repositories;
    using Microsoft.EntityFrameworkCore;

    namespace EmployeeManagementAPI.Repositories
    {
        public class AttendanceRepository : IAttendanceRepository
        {
            private readonly EmployeeContext _context;
            public AttendanceRepository(EmployeeContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Attendance>> GetAllAsync()
            {
                return await _context.AttendanceRecords.ToListAsync();
            }

            public async Task<Attendance> GetByIdAsync(int id)
            {
                return await _context.AttendanceRecords.FindAsync(id);
            }

            public async Task AddAsync(Attendance attendance)
            {
                await _context.AttendanceRecords.AddAsync(attendance);
            }

            public void Update(Attendance attendance)
            {
                _context.AttendanceRecords.Update(attendance);
            }

            public void Delete(Attendance attendance)
            {
                _context.AttendanceRecords.Remove(attendance);
            }

            public async Task<bool> SaveChangesAsync()
            {
                return await _context.SaveChangesAsync() > 0;
            }
        }
    }

}
