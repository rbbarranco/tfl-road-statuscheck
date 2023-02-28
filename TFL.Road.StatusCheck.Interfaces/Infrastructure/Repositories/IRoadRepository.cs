using TFL.Road.StatusCheck.Application.Entities.Road.V1.Input;
using TFL.Road.StatusCheck.Application.Entities.Road.V1.Output;

namespace TFL.Road.StatusCheck.Interfaces.Infrastructure.Repositories
{
    public interface IRoadRepository
    {
        GetRoadStatusResponse GetRoadStatus(GetRoadStatusRequest request);
    }
}
