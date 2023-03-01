using System.Runtime.CompilerServices;
using TFL.Road.StatusCheck.Application.Interfaces.Mappers;
using Contract = TFL.Road.StatusCheck.Application.Contracts.Road.V1;
using Entity = TFL.Road.StatusCheck.Application.Entities.Road.V1;

[assembly: InternalsVisibleTo("TFL.Road.StatusCheck.Tests")]
namespace TFL.Road.StatusCheck.Application.Mappers
{
    /// <summary>
    /// Contract mapper implementation for the Application layer
    /// Used for contract to entity mapping and vice versa
    ///
    /// The internal methods are exposed to the test project so they can be tested independent of the methods that are using them
    /// This is also to ensure that the methods using these internal methods won't have overlapping tests
    /// The internal methods weren't made public as there are no actual users of this method
    /// </summary>
    public class RoadMapper : IRoadMapper
    {
        public Entity.Input.GetRoadStatusRequest? Map(Contract.Input.GetRoadStatusRequest? request)
        {
            if (request is null)
                return null;

            return new Entity.Input.GetRoadStatusRequest()
            {
                RoadId = request.RoadId
            };
        }

        public Contract.Output.GetRoadStatusResponse? Map(Entity.Output.GetRoadStatusResponse? response)
        {
            if (response is null)
                return null;

            return new Contract.Output.GetRoadStatusResponse()
            {
                ResultCode = Map(response.ResultCode),
                RoadStatus = Map(response.RoadStatus),
                ResponseNotes = response.ResponseNotes
            };
        }

        internal Contract.Output.GetRoadStatusResultCode Map(Entity.Output.GetRoadStatusResultCode resultCode)
        {
            return resultCode switch
            {
                Entity.Output.GetRoadStatusResultCode.Successful => Contract.Output.GetRoadStatusResultCode.Successful,
                Entity.Output.GetRoadStatusResultCode.InvalidRoad => Contract.Output.GetRoadStatusResultCode.InvalidRoad,
                Entity.Output.GetRoadStatusResultCode.OtherFailure => Contract.Output.GetRoadStatusResultCode.OtherFailure,
                _ => throw new ArgumentOutOfRangeException(nameof(resultCode), resultCode, null)
            };
        }

        internal Contract.Output.RoadStatusResult? Map(Entity.Output.RoadStatusResult? roadStatusResult)
        {
            if (roadStatusResult is null)
                return null;

            return new Contract.Output.RoadStatusResult()
            {
                Id = roadStatusResult.Id,
                Name = roadStatusResult.Name,
                Status = roadStatusResult.Status,
                StatusDescription = roadStatusResult.StatusDescription
            };
        }
    }
}
