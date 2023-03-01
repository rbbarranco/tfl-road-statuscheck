namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.DTO
{
    public record RoadStatus
    {
        public string Id { get; init; }
        public string DisplayName { get; init; }
        public string StatusSeverity { get; init; }
        public string StatusSeverityDescription { get; init; }
    }
}