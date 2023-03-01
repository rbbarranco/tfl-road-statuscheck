using TFL.Road.StatusCheck.Application.Entities.Road.V1.Output;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.DTO;

namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Mappers
{
    /// <summary>
    /// The interface to be implemented by the contract mapper in the Infrastructure layer
    /// Contains the signatures for mapping between DTOs and entities and vice versa
    /// </summary>
    public interface IRoadMapper
    {
        RoadStatusResult? Map(RoadStatus? roadStatus);
    }
}
