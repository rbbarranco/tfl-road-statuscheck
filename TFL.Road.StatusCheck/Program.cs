using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TFL.Road.StatusCheck.Application.Contracts.Road.V1.Input;
using TFL.Road.StatusCheck.Application.Contracts.Road.V1.Output;
using TFL.Road.StatusCheck.Application.Interfaces.Infrastructure.Repositories;
using TFL.Road.StatusCheck.Application.Interfaces.Services;
using TFL.Road.StatusCheck.Application.Services;
using TFL.Road.StatusCheck.Application.Validators;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Client;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Repositories;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Serialiser;
using ApplicationMapperInterfaces = TFL.Road.StatusCheck.Application.Interfaces.Mappers;
using ApplicationMappers = TFL.Road.StatusCheck.Application.Mappers;
using InfraMappers = TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Mappers;

var roadId = GetRoadId(args);

try
{
    using IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            //Application layer
            services.AddSingleton<IRoadService, RoadService>();
            services.AddSingleton<IValidator<GetRoadStatusRequest>, GetRoadStatusRequestValidator>();
            services.AddSingleton<ApplicationMapperInterfaces.IRoadMapper, ApplicationMappers.RoadMapper>();

            //Infrastructure layer
            services.AddSingleton<IRoadRepository, RoadRepository>();
            services.AddSingleton<ITFLOpenDataClient, TFLOpenDataClient>();
            services.AddSingleton<IResponseSerialiser, ResponseSerialiser>();
            services.AddSingleton<InfraMappers.IRoadMapper, InfraMappers.RoadMapper>();
        }).Build();
    var roadService = host.Services.GetService<IRoadService>();

    var roadStatus = await GetRoadStatusAsync(roadId, roadService);
    PrintResponse(roadStatus, roadId);
    Environment.Exit((int)roadStatus.ResultCode);
}
catch (Exception ex)
{
    var response = new GetRoadStatusResponse()
    {
        ResultCode = GetRoadStatusResultCode.OtherFailure,
        ResponseNotes = ex.Message
    };

    PrintResponse(response, roadId);
    Environment.Exit((int)response.ResultCode);
}

static string GetRoadId(string[] args)
{
    return "A2";

    if (args.Length >= 1)
        return args[0];
    return
        string.Empty;
}

static async Task<GetRoadStatusResponse> GetRoadStatusAsync(string roadId, IRoadService roadService)
{
    var request = new GetRoadStatusRequest() { RoadId = roadId };
    return await roadService.GetRoadStatusAsync(request);
}

static void PrintResponse(GetRoadStatusResponse response, string originalRoadId)
{
    switch (response.ResultCode)
    {
        case GetRoadStatusResultCode.Successful:
        {
            Console.WriteLine($"The status of {response.RoadStatus?.Name} is as follows");
            Console.WriteLine($"Road Status is {response.RoadStatus?.Status}");
            Console.WriteLine($"Road Status Description is {response.RoadStatus?.StatusDescription}");
                break;
        }
        case GetRoadStatusResultCode.InvalidRoad:
        {
            Console.WriteLine($"{originalRoadId} is not a valid road");
            break;
        }
        case GetRoadStatusResultCode.ValidationFailed:
        {
            Console.WriteLine($"Parameter(s) wrong - {response.ResponseNotes}. Please check your input.");
            break;
        }
        case GetRoadStatusResultCode.OtherFailure:
        {
            Console.WriteLine($"Something went wrong - {response.ResponseNotes}. Please check with your admin.");
            break;
        }
    }
}