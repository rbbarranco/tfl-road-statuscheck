using FluentValidation;
using TFL.Road.StatusCheck.Application.Contracts.Road.V1.Input;
using TFL.Road.StatusCheck.Application.Contracts.Road.V1.Output;
using TFL.Road.StatusCheck.Application.Interfaces.Infrastructure.Repositories;
using TFL.Road.StatusCheck.Application.Interfaces.Mappers;
using TFL.Road.StatusCheck.Application.Interfaces.Services;

namespace TFL.Road.StatusCheck.Application.Services
{
    /// <summary>
    /// Implementation of the road service. This class is merely a coordinator.
    /// This service will be used by consumers for road related operations e.g. getting a road's status
    /// </summary>
    public class RoadService : IRoadService
    {
        private readonly IValidator<GetRoadStatusRequest> _requestValidator;
        private readonly IRoadMapper _roadMapper;
        private readonly IRoadRepository _roadRepository;

        public RoadService(IValidator<GetRoadStatusRequest> requestValidator, IRoadMapper roadMapper, IRoadRepository roadRepository)
        {
            _requestValidator = requestValidator;
            _roadMapper = roadMapper;
            _roadRepository = roadRepository;
        }

        public async Task<GetRoadStatusResponse> GetRoadStatusAsync(GetRoadStatusRequest request)
        {
            var validationResult = _requestValidator.Validate(request);
            if (!validationResult.IsValid)
                return new GetRoadStatusResponse()
                {
                    ResultCode = GetRoadStatusResultCode.ValidationFailed,
                    ResponseNotes = validationResult.Errors.FirstOrDefault()?.ErrorMessage,
                    RoadStatus = null
                };

            var repoRequest = _roadMapper.Map(request);
            var repoResponse = await _roadRepository.GetRoadStatusAsync(repoRequest);
            var response = _roadMapper.Map(repoResponse);

            return response;
        }
    }
}