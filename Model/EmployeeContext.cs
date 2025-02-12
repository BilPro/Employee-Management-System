using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Model
{
    public class EmployeeContext : DbContext
    {

        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("Application has started.");
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Attendance> AttendanceRecords { get; set; }
        public DbSet<MissingAttendanceRequest> MissingAttendanceRequests { get; set; }

        public DbSet<LogEntry> LogEntries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<Attendance>().ToTable("Attendance");
            modelBuilder.Entity<MissingAttendanceRequest>().ToTable("MissingAttendanceRequests");
            modelBuilder.Entity<LogEntry>().ToTable("LogEntry");

            modelBuilder.Entity<MissingAttendanceRequest>()
                .HasKey(m => m.RequestID);  // Ensure primary key is set

            modelBuilder.Entity<MissingAttendanceRequest>()
                .HasOne(m => m.Employee)
                .WithMany()
                .HasForeignKey(m => m.EmployeeID);

            base.OnModelCreating(modelBuilder);
        }

    }
}