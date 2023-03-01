namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Serialiser
{
    public interface IResponseSerialiser
    {
        Task<T> DeserialiseResponseAsync<T>(HttpResponseMessage responseMessage);
    }
}
