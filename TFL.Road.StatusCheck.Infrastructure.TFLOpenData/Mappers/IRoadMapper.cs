using TFL.Road.StatusCheck.Application.Entities.Road.V1.Output;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.DTO;

namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Mappers
{
    public interface IRoadMapper
    {
        RoadStatusResult? Map(RoadStatus? roadStatus);
    }
}
