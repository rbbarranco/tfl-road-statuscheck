namespace TFL.Road.StatusCheck.Application.Entities.Road.V1.Input
{
    public record GetRoadStatusRequest
    {
        public string RoadId { get; init; }
    }
}
