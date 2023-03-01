namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Serialiser
{
    /// <summary>
    /// The interface for to be implemented by the response serialiser
    /// </summary>
    public interface IResponseSerialiser
    {
        Task<T> DeserialiseResponseAsync<T>(HttpResponseMessage responseMessage);
    }
}
