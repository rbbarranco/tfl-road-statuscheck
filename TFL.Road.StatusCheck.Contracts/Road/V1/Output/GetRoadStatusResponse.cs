namespace TFL.Road.StatusCheck.Contracts.Road.V1.Output
{
    public record GetRoadStatusResponse
    {
        public GetRoadStatusResultCode ResultCode { get; init; }
        public RoadStatusResult? RoadStatus { get; init; }
        public string? ResponseNotes { get; init; }
    }
}
