using FluentValidation;
using TFL.Road.StatusCheck.Application.Contracts.Road.V1.Input;

namespace TFL.Road.StatusCheck.Application.Validators
{
    public class GetRoadStatusRequestValidator : AbstractValidator<GetRoadStatusRequest>
    {
        public GetRoadStatusRequestValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(r => r.RoadId).NotEmpty();
        }
    }
}
