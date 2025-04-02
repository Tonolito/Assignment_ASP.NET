using Business.Models;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IStatusService
    {
        Task<StatusResult<Status>> GetStatusByIdAsync(int id);
        Task<StatusResult<Status>> GetStatusByNameAsync(string statusName);
        Task<StatusResult<IEnumerable<Status>>> GetStatusesAsnyc();
    }
}