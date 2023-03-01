using Microsoft.Extensions.Configuration;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using TFL.Road.StatusCheck.Application.Entities.Road.V1.Input;
using TFL.Road.StatusCheck.Application.Entities.Road.V1.Output;
using TFL.Road.StatusCheck.Application.Interfaces.Infrastructure.Repositories;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Client;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.DTO;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Mappers;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Serialiser;

[assembly: InternalsVisibleTo("TFL.Road.StatusCheck.Tests")]
namespace TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Repositories
{
    public class RoadRepository : IRoadRepository
    {
        private readonly ITFLOpenDataClient _tflOpenDataClient;
        private readonly IResponseSerialiser _responseSerialiser;
        private readonly IConfiguration _configuration;
        private readonly IRoadMapper _roadMapper;

        public RoadRepository(ITFLOpenDataClient tflOpenDataClient, IResponseSerialiser responseSerialiser, IConfiguration configuration, IRoadMapper roadMapper)
        {
            _tflOpenDataClient = tflOpenDataClient;
            _responseSerialiser = responseSerialiser;
            _configuration = configuration;
            _roadMapper = roadMapper;
        }

        public async Task<GetRoadStatusResponse> GetRoadStatusAsync(GetRoadStatusRequest request)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(_configuration.GetSection("TFLOpenData:Endpoints:Road").Value).Append("/")
                .Append(request.RoadId);
            var query = queryBuilder.ToString();

            var response = await _tflOpenDataClient.GetDataAsync(query);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        var roadStatuses = await _responseSerialiser.DeserialiseResponseAsync<IEnumerable<RoadStatus>>(response);
                        return CreateResponseForOK(roadStatuses);
                    }
                case HttpStatusCode.NotFound:
                    {
                        var errorResponse = await _responseSerialiser.DeserialiseResponseAsync<ErrorResponse>(response);
                        return CreateResponseForNotFound(errorResponse);
                    }
                case HttpStatusCode.TooManyRequests:
                    {
                        var message = await response.Content.ReadAsStringAsync();
                        return CreateResponseForOtherFailures(message);
                    }
                default:
                    {
                        return CreateResponseForOtherFailures($"An error occurred - {response.StatusCode}");
                    }
            }
        }

        internal GetRoadStatusResponse CreateResponseForOK(IEnumerable<RoadStatus> roadStatuses)
        {
            if (roadStatuses is null || !roadStatuses.Any() || roadStatuses.FirstOrDefault() is null)
                return CreateResponseForOtherFailures("No results received.");

            return new GetRoadStatusResponse()
            {
                ResultCode = GetRoadStatusResultCode.Successful,
                RoadStatus = _roadMapper.Map(roadStatuses.First()),
            };
        }

        internal GetRoadStatusResponse CreateResponseForNotFound(ErrorResponse errorResponse)
        {
            if (errorResponse.Message.ToLowerInvariant().Contains("the following road id is not recognised"))
                return new GetRoadStatusResponse()
                {
                    ResultCode = GetRoadStatusResultCode.InvalidRoad
                };

            return CreateResponseForOtherFailures(errorResponse.Message);
        }

        internal GetRoadStatusResponse CreateResponseForOtherFailures(string message)
        {
            return new GetRoadStatusResponse()
            {
                ResultCode = GetRoadStatusResultCode.OtherFailure,
                ResponseNotes = message
            };
        }
    }
}
