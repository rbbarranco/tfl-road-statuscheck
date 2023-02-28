using TFL.Road.StatusCheck.Application.Entities.Road.V1.Input;
using TFL.Road.StatusCheck.Application.Entities.Road.V1.Output;
using TFL.Road.StatusCheck.Interfaces.Infrastructure.Repositories;

namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData
{
    public class RoadRepository : IRoadRepository
    {
        public GetRoadStatusResponse GetRoadStatus(GetRoadStatusRequest request)
        {
            throw new NotImplementedException();
        }
    }
}