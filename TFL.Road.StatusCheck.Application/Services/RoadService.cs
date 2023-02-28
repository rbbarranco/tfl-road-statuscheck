using FluentValidation;
using TFL.Road.StatusCheck.Contracts.Road.V1.Input;
using TFL.Road.StatusCheck.Contracts.Road.V1.Output;
using TFL.Road.StatusCheck.Interfaces.Infrastructure.Repositories;
using TFL.Road.StatusCheck.Interfaces.Mappers.Road;
using TFL.Road.StatusCheck.Interfaces.Services;

namespace TFL.Road.StatusCheck.Application.Services
{
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

        public GetRoadStatusResponse GetRoadStatus(GetRoadStatusRequest request)
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
            var repoResponse = _roadRepository.GetRoadStatus(repoRequest);
            var response = _roadMapper.Map(repoResponse);

            return response;
        }
    }
}