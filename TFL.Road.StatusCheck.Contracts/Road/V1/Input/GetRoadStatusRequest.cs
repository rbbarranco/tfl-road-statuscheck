namespace TFL.Road.StatusCheck.Contracts.Road.V1.Input
{
    public record GetRoadStatusRequest
    {
        public string RoadId { get; init; }
    }
}
