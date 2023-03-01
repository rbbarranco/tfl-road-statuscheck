using Contract = TFL.Road.StatusCheck.Application.Contracts.Road.V1;
using Entity = TFL.Road.StatusCheck.Application.Entities.Road.V1;

namespace TFL.Road.StatusCheck.Application.Interfaces.Mappers
{
    /// <summary>
    /// The interface to be implemented by the contract mapper in the Application layer
    /// Contains the signatures for mapping between contract and entities and vice versa
    /// </summary>
    public interface IRoadMapper
    {
        Entity.Input.GetRoadStatusRequest? Map(Contract.Input.GetRoadStatusRequest? request);
        Contract.Output.GetRoadStatusResponse? Map(Entity.Output.GetRoadStatusResponse? response);
    }
}
