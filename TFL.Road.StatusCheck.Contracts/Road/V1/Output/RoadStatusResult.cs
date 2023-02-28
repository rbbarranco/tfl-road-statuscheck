namespace TFL.Road.StatusCheck.Contracts.Road.V1.Output
{
    public record RoadStatusResult
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Status { get; init; }
        public string StatusDescription { get; init; }
    }
}
