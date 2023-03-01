using TFL.Road.StatusCheck.Application.Contracts.Road.V1.Input;
using TFL.Road.StatusCheck.Application.Contracts.Road.V1.Output;

namespace TFL.Road.StatusCheck.Application.Interfaces.Services
{
    /// <summary>
    /// The interface to be implemented by the road service class
    /// </summary>
    public interface IRoadService
    {
        Task<GetRoadStatusResponse> GetRoadStatusAsync(GetRoadStatusRequest request);
    }
}