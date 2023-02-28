using TFL.Road.StatusCheck.Interfaces.Mappers.Road;
using Contract = TFL.Road.StatusCheck.Contracts.Road.V1;
using Entity = TFL.Road.StatusCheck.Application.Entities.Road.V1;

namespace TFL.Road.StatusCheck.Application.Mappers
{
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

        private Contract.Output.GetRoadStatusResultCode Map(Entity.Output.GetRoadStatusResultCode resultCode)
        {
            return resultCode switch
            {
                Entity.Output.GetRoadStatusResultCode.Successful => Contract.Output.GetRoadStatusResultCode.Successful,
                Entity.Output.GetRoadStatusResultCode.InvalidRoad => Contract.Output.GetRoadStatusResultCode.InvalidRoad,
                Entity.Output.GetRoadStatusResultCode.OtherFailure => Contract.Output.GetRoadStatusResultCode.OtherFailure
            };
        }

        private Contract.Output.RoadStatusResult? Map(Entity.Output.RoadStatusResult? roadStatusResult)
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
