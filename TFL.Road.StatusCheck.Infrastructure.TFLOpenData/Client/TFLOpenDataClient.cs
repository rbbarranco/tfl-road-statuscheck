﻿using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Client
{
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
                    .Append(query);
                if (query.Contains('?'))
                    uriBuilder.Append("&");
                else
                    uriBuilder.Append("?");
                uriBuilder.Append("app_key").Append("=").Append(_configuration.GetSection("TFLOpenData:AppKey").Value);

                var uri = uriBuilder.ToString();
                
                httpClient.BaseAddress = new Uri(uri);

                return await httpClient.GetAsync(uri);
            }
        }
    }
}