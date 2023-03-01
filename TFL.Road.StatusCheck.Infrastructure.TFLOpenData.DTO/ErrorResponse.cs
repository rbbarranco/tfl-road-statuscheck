namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.DTO
{
    /// <summary>
    /// Represents an error response
    /// Contract where the JSON result for the /Road endpoint is deserialised to (e.g. on 404)
    /// </summary>
    public record ErrorResponse
    {
        public string Message { get; init; }
    }
}
