namespace TFL.Road.StatusCheck.Application.Entities.Road.V1.Output
{
    /// <summary>
    /// Response contract for a request to get a road's status
    /// Used only for communications between the Application layer and Infrastructure layer
    /// </summary>
    public record GetRoadStatusResponse
    {
        public GetRoadStatusResultCode ResultCode { get; init; }
        public RoadStatusResult? RoadStatus { get; init; }
        public string? ResponseNotes { get; init; }
    }
}
