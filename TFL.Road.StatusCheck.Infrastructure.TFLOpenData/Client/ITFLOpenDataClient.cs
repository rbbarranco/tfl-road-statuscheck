namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Client
{
    /// <summary>
    /// The interface to be implemented by the http client
    /// </summary>
    public interface ITFLOpenDataClient
    {
        Task<HttpResponseMessage> GetDataAsync(string query);
    }
}
