namespace TFL.Road.StatusCheck.Application.Contracts.Road.V1.Output
{
    /// <summary>
    /// Represents a road status
    /// </summary>
    public record RoadStatusResult
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Status { get; init; }
        public string StatusDescription { get; init; }
    }
}
