namespace TFL.Road.StatusCheck.Application.Contracts.Road.V1.Input
{
    /// <summary>
    /// Request contract to get a road's status
    /// </summary>
    public record GetRoadStatusRequest
    {
        public string RoadId { get; init; }
    }
}
