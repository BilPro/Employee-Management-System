using Employee_Management_System.Model;

namespace Employee_Management_System.Services
{
    public interface IMissingAttendanceRequestService
    {
        Task<IEnumerable<MissingAttendanceRequest>> GetAllRequestsAsync();
        Task<MissingAttendanceRequest> GetRequestByIdAsync(int id);
        Task<bool> MarkMissingAttendanceAsync(MissingAttendanceRequest request);
        Task<bool> UpdateRequestStatusAsync(int id, MissingAttendanceRequest update);
    }
}
