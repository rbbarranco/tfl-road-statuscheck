namespace TFL.Road.StatusCheck.Application.Contracts.Road.V1.Input
{
    public record GetRoadStatusRequest
    {
        public string RoadId { get; init; }
    }
}
