using TFL.Road.StatusCheck.Contracts.Road.V1.Input;
using TFL.Road.StatusCheck.Contracts.Road.V1.Output;

namespace TFL.Road.StatusCheck.Interfaces.Services
{
    public interface IRoadService
    {
        GetRoadStatusResponse GetRoadStatus(GetRoadStatusRequest request);
    }
}