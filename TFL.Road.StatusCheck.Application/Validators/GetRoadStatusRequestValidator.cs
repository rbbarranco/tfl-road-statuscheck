using FluentValidation;
using TFL.Road.StatusCheck.Application.Contracts.Road.V1.Input;

namespace TFL.Road.StatusCheck.Application.Validators
{
    /// <summary>
    /// Fluent validator for the GetRoadStatusRequest contract
    /// </summary>
    public class GetRoadStatusRequestValidator : AbstractValidator<GetRoadStatusRequest>
    {
        public GetRoadStatusRequestValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(r => r.RoadId).NotEmpty().WithMessage("RoadId is required.")
                .DependentRules(() =>
                {
                    RuleFor(r => r.RoadId).Must(v =>
                            !v.ToLowerInvariant().Contains("/") &&
                            !v.ToLowerInvariant().Contains("\\") &&
                            !v.ToLowerInvariant().Contains("app_id") &&
                            !v.ToLowerInvariant().Contains("app_key"))
                        .WithMessage("Invalid characters in RoadId.");
                });
        }
    }
}