namespace TFL.Road.StatusCheck.Application.Contracts.Road.V1.Output
{
    /// <summary>
    /// Possible result codes when doing a request to get a road's status
    /// These are the possible result codes that can be returned by the Application layer to the consumer
    /// </summary>
    public enum GetRoadStatusResultCode
    {
        Successful = 0,
        ValidationFailed = 1,
        InvalidRoad = 100,
        OtherFailure = 900
    }
}
