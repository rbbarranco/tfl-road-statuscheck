using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Serialiser
{
    /// <summary>
    /// Implementation of the response serialiser
    /// Extracted from the actual repository implementation to make it testable.
    /// </summary>
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
