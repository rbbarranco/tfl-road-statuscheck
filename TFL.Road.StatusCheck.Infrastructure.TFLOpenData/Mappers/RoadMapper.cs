using TFL.Road.StatusCheck.Application.Entities.Road.V1.Output;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.DTO;

namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Mappers
{
    public class RoadMapper : IRoadMapper
    {
        public RoadStatusResult? Map(RoadStatus? roadStatus)
        {
            if (roadStatus is null)
                return null;

            return new RoadStatusResult()
            {
                Id = roadStatus.Id,
                Name = roadStatus.DisplayName,
                Status = roadStatus.StatusSeverity,
                StatusDescription = roadStatus.StatusSeverityDescription
            };
        }
    }
}
