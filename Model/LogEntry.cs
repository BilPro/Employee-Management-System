using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model
{
    public class LogEntry
    {
        [Key]
        public int Id { get; set; }
        public DateTime Logged { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
