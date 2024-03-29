﻿using FluentAssertions;
using NUnit.Framework;
using TFL.Road.StatusCheck.Application.Contracts.Road.V1.Input;
using TFL.Road.StatusCheck.Application.Validators;

namespace TFL.Road.StatusCheck.Tests.Application.Validators
{
    /// <summary>
    /// The purpose of this test is not to test FluentValidation's feature
    /// This is here to ensure that the validation rules are in place
    /// </summary>
    public class GetRoadStatusRequestValidatorTests
    {
        #region Road Id test(s)
        
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void RoadId_GivenEmptyValues_ThenReturnFailure(string roadId)
        {
            var validator = new GetRoadStatusRequestValidator();
            var request = new GetRoadStatusRequest()
            {
                RoadId = roadId
            };

            var result = validator.Validate(request);

            var propertyName = nameof(GetRoadStatusRequest.RoadId);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].PropertyName.Should().Be(propertyName);
        }

        [TestCase("/")]
        [TestCase("\\")]
        [TestCase("APP_ID")]
        [TestCase("app_KEY")]
        public void RoadId_GivenInvalidValues_ThenReturnFailure(string roadId)
        {
            var validator = new GetRoadStatusRequestValidator();
            var request = new GetRoadStatusRequest()
            {
                RoadId = roadId
            };

            var result = validator.Validate(request);

            var propertyName = nameof(GetRoadStatusRequest.RoadId);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].PropertyName.Should().Be(propertyName);
        }

        #endregion
    }
}
