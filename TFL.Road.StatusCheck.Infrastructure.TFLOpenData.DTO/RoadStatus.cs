namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.DTO
{
    /// <summary>
    /// Represents a road status
    /// Contract where the JSON result for the /Road endpoint is deserialised to
    /// </summary>
    public record RoadStatus
    {
        public string Id { get; init; }
        public string DisplayName { get; init; }
        public string StatusSeverity { get; init; }
        public string StatusSeverityDescription { get; init; }
    }
}