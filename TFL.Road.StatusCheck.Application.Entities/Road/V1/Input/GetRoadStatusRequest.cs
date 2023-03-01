namespace TFL.Road.StatusCheck.Application.Entities.Road.V1.Input
{
    /// <summary>
    /// Request contract to get a road's status
    /// Used only for communications between the Application layer and Infrastructure layer
    /// </summary>
    public record GetRoadStatusRequest
    {
        public string RoadId { get; init; }
    }
}
