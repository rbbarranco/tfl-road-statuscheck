using TFL.Road.StatusCheck.Application.Entities.Road.V1.Input;
using TFL.Road.StatusCheck.Application.Entities.Road.V1.Output;

namespace TFL.Road.StatusCheck.Application.Interfaces.Infrastructure.Repositories
{
    public interface IRoadRepository
    {
        Task<GetRoadStatusResponse> GetRoadStatusAsync(GetRoadStatusRequest request);
    }
}
