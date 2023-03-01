namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Client
{
    public interface ITFLOpenDataClient
    {
        Task<HttpResponseMessage> GetDataAsync(string query);
    }
}
