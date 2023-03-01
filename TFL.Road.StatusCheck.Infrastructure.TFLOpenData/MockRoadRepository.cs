﻿using TFL.Road.StatusCheck.Application.Entities.Road.V1.Input;
using TFL.Road.StatusCheck.Application.Entities.Road.V1.Output;
using TFL.Road.StatusCheck.Interfaces.Infrastructure.Repositories;

namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData
{
    public class MockRoadRepository : IRoadRepository
    {
        public GetRoadStatusResponse GetRoadStatus(GetRoadStatusRequest request)
        {
            if (request.RoadId.ToUpperInvariant() == "A123")
                return new GetRoadStatusResponse()
                {
                    ResultCode = GetRoadStatusResultCode.InvalidRoad,
                    ResponseNotes = $"The following road id is not recognised: {request.RoadId}"
                };

            if (request.RoadId.ToUpperInvariant() == "B123")
                return new GetRoadStatusResponse()
                {
                    ResultCode = GetRoadStatusResultCode.OtherFailure,
                    ResponseNotes = "An unhandled exception has occurred."
                };

            return new GetRoadStatusResponse()
            {
                ResultCode = GetRoadStatusResultCode.Successful,
                RoadStatus = new RoadStatusResult()
                {
                    Id = request.RoadId,
                    Name = request.RoadId.ToUpperInvariant(),
                    Status = "Good",
                    StatusDescription = "No Exceptional Delays"
                }
            };
        }
    }
}