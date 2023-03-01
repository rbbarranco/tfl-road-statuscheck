using TFL.Road.StatusCheck.Application.Contracts.Road.V1.Input;
using TFL.Road.StatusCheck.Application.Contracts.Road.V1.Output;

namespace TFL.Road.StatusCheck.Application.Interfaces.Services
{
    public interface IRoadService
    {
        Task<GetRoadStatusResponse> GetRoadStatusAsync(GetRoadStatusRequest request);
    }
}