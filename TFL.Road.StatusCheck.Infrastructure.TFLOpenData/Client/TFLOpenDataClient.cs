using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Client
{
    /// <summary>
    /// Implementation of the actual http client calls to TFL Open Data
    /// Extracted from the repository implementation to make it testable
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TFLOpenDataClient : ITFLOpenDataClient
    {
        private readonly IConfiguration _configuration;

        public TFLOpenDataClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage> GetDataAsync(string query)
        {
            using (var httpClient = new HttpClient())
            {
                var uriBuilder = new StringBuilder();
                uriBuilder
                    .Append(_configuration.GetSection("TFLOpenData:BaseUrl").Value).Append("/")
                    .Append(HttpUtility.HtmlEncode(query));
                if (query.Contains('?'))
                    uriBuilder.Append("&");
                else
                    uriBuilder.Append("?");
                uriBuilder.Append("app_id").Append("=").Append(_configuration.GetSection("TFLOpenData:AppId").Value).Append("&");
                uriBuilder.Append("app_key").Append("=").Append(_configuration.GetSection("TFLOpenData:AppKey").Value);

                var uri = uriBuilder.ToString();
                
                httpClient.BaseAddress = new Uri(uri);

                return await httpClient.GetAsync(uri);
            }
        }
    }
}
