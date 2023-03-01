namespace TFL.Road.StatusCheck.Application.Contracts.Road.V1.Output
{
    /// <summary>
    /// Response contract for a request to get a road's status
    /// </summary>
    public record GetRoadStatusResponse
    {
        public GetRoadStatusResultCode ResultCode { get; init; }
        public RoadStatusResult? RoadStatus { get; init; }
        public string? ResponseNotes { get; init; }
    }
}
