using Contract = TFL.Road.StatusCheck.Contracts.Road.V1;
using Entity = TFL.Road.StatusCheck.Application.Entities.Road.V1;

namespace TFL.Road.StatusCheck.Interfaces.Mappers.Road
{
    public interface IRoadMapper
    {
        Entity.Input.GetRoadStatusRequest? Map(Contract.Input.GetRoadStatusRequest? request);
        Contract.Output.GetRoadStatusResponse? Map(Entity.Output.GetRoadStatusResponse? response);
    }
}
