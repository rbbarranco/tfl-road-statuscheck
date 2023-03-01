using System.Runtime.CompilerServices;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TFL.Road.StatusCheck.Application.Mappers;
using TFL.Road.StatusCheck.Application.Services;
using TFL.Road.StatusCheck.Application.Validators;
using TFL.Road.StatusCheck.Contracts.Road.V1.Input;using TFL.Road.StatusCheck.Contracts.Road.V1.Output;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData;
using TFL.Road.StatusCheck.Interfaces.Infrastructure.Repositories;
using TFL.Road.StatusCheck.Interfaces.Mappers.Road;
using TFL.Road.StatusCheck.Interfaces.Services;

var roadId = GetRoadId(args);

try
{
    using IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices(services =>
        {
            //Application layer
            services.AddSingleton<IRoadService, RoadService>();
            services.AddSingleton<IValidator<GetRoadStatusRequest>, GetRoadStatusRequestValidator>();
            services.AddSingleton<IRoadMapper, RoadMapper>();

            //Infrastructure layer
            services.AddSingleton<IRoadRepository, MockRoadRepository>();
        }).Build();
    var roadService = host.Services.GetService<IRoadService>();

    var roadStatus = GetRoadStatus(roadId, roadService);
    
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
    if (args.Length >= 1)
        return args[0];
    return
        string.Empty;
}

static GetRoadStatusResponse GetRoadStatus(string roadId, IRoadService roadService)
{
    var request = new GetRoadStatusRequest() { RoadId = roadId };
    return roadService.GetRoadStatus(request);
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