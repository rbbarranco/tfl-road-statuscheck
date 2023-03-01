namespace TFL.Road.StatusCheck.Application.Entities.Road.V1.Output
{
    /// <summary>
    /// Possible result codes when doing a request to get a road's status
    /// These are the possible result codes that can be returned by the Infrastructure layer to the Application layer
    /// Used only for communications between the Application layer and Infrastructure layer
    /// </summary>
    public enum GetRoadStatusResultCode
    {
        Successful = 0,
        InvalidRoad = 100,
        OtherFailure = 900
    }
}
