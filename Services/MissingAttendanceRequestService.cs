using Employee_Management_System.Model;
using Employee_Management_System.Repositories;
using NLog;

namespace Employee_Management_System.Services
{
    public class MissingAttendanceRequestService : IMissingAttendanceRequestService
    {
        private readonly IMissingAttendanceRequestRepository _repository;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public MissingAttendanceRequestService(IMissingAttendanceRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MissingAttendanceRequest>> GetAllRequestsAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error retrieving missing attendance requests.");
                throw;
            }
        }

        public async Task<MissingAttendanceRequest> GetRequestByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error retrieving missing attendance request with ID {id}");
                throw;
            }
        }

        public async Task<bool> MarkMissingAttendanceAsync(MissingAttendanceRequest request)
        {
            try
            {
                // Additional business validations (e.g., check if employee exists) can be added here.
                await _repository.AddAsync(request);
                return await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error marking missing attendance.");
                return false;
            }
        }

        public async Task<bool> UpdateRequestStatusAsync(int id, MissingAttendanceRequest update)
        {
            try
            {
                if (id != update.RequestID)
                    throw new ArgumentException("Request ID mismatch.");

                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    return false;

                // Update status and ApprovedBy field.
                existing.Status = update.Status;
                existing.ApprovedBy = update.ApprovedBy;

                _repository.Update(existing);
                return await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error updating missing attendance request with ID {id}");
                return false;
            }
        }
    }
}
