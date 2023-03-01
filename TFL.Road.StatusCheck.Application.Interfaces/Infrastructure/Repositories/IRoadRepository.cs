using TFL.Road.StatusCheck.Application.Entities.Road.V1.Input;
using TFL.Road.StatusCheck.Application.Entities.Road.V1.Output;

namespace TFL.Road.StatusCheck.Application.Interfaces.Infrastructure.Repositories
{
    /// <summary>
    /// The interface that must be implemented by the Infrastructure layer implementing the repository for road statuses
    /// </summary>
    public interface IRoadRepository
    {
        Task<GetRoadStatusResponse> GetRoadStatusAsync(GetRoadStatusRequest request);
    }
}
