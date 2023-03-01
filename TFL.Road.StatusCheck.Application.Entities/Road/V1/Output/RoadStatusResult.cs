namespace TFL.Road.StatusCheck.Application.Entities.Road.V1.Output
{
    /// <summary>
    /// Represents a road status
    /// Used only for communications between the Application layer and Infrastructure layer
    /// </summary>
    public record RoadStatusResult
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Status { get; init; }
        public string StatusDescription { get; init; }
    }
}
