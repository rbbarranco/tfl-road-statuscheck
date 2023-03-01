using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Serialiser
{
    [ExcludeFromCodeCoverage]
    public class ResponseSerialiser : IResponseSerialiser
    {
        public async Task<T> DeserialiseResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
