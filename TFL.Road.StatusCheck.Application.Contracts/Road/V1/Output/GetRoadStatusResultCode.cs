﻿namespace TFL.Road.StatusCheck.Application.Contracts.Road.V1.Output
{
    public enum GetRoadStatusResultCode
    {
        Successful = 0,
        ValidationFailed = 1,
        InvalidRoad = 100,
        OtherFailure = 900
    }
}