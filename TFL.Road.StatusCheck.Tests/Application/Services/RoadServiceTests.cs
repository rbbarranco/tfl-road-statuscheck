using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using TFL.Road.StatusCheck.Application.Services;
using TFL.Road.StatusCheck.Interfaces.Infrastructure.Repositories;
using TFL.Road.StatusCheck.Interfaces.Mappers.Road;
using Contract = TFL.Road.StatusCheck.Contracts.Road.V1;
using Entity = TFL.Road.StatusCheck.Application.Entities.Road.V1;

namespace TFL.Road.StatusCheck.Tests.Application.Services
{
    public class RoadServiceTests
    {
        private class MockProvider
        {
            private readonly Mock<IValidator<Contract.Input.GetRoadStatusRequest>> _requestValidatorMock = new();
            private readonly Mock<IRoadMapper> _roadMapperMock = new();
            private readonly Mock<IRoadRepository> _roadRepositoryMock = new();

            public RoadService GetService()
            {
                return new RoadService(_requestValidatorMock.Object, _roadMapperMock.Object, _roadRepositoryMock.Object);
            }

            public Contract.Input.GetRoadStatusRequest GetMockRequest()
            {
                return new Contract.Input.GetRoadStatusRequest()
                {
                    RoadId = "roadId"
                };
            }

            public void SetUpRequestValidatorMock(Contract.Input.GetRoadStatusRequest request, ValidationResult? validationResult = null)
            {
                validationResult ??= new ValidationResult();
                _requestValidatorMock.Setup(m => m.Validate(It.Is<Contract.Input.GetRoadStatusRequest>(r =>
                        r.RoadId == request.RoadId)))
                    .Returns(validationResult)
                    .Verifiable();
            }

            public void SetUpMapToEntityRequestMock(Contract.Input.GetRoadStatusRequest contract, Entity.Input.GetRoadStatusRequest entity)
            {
                _roadMapperMock.Setup(m => m.Map(It.Is<Contract.Input.GetRoadStatusRequest>(r =>
                        r.RoadId == contract.RoadId)))
                    .Returns(entity)
                    .Verifiable();
            }

            public void SetUpMapToContractResponseMock(Entity.Output.GetRoadStatusResponse entity, Contract.Output.GetRoadStatusResponse contract)
            {
                _roadMapperMock.Setup(m => m.Map(It.Is<Entity.Output.GetRoadStatusResponse>(r =>
                        r.ResultCode == entity.ResultCode &&
                        r.ResponseNotes == entity.ResponseNotes &&
                        r.RoadStatus.Id == entity.RoadStatus.Id &&
                        r.RoadStatus.Name == entity.RoadStatus.Name &&
                        r.RoadStatus.Status == entity.RoadStatus.Status &&
                        r.RoadStatus.StatusDescription == entity.RoadStatus.StatusDescription)))
                    .Returns(contract)
                    .Verifiable();
            }


            public void SetUpRoadRepositoryMock(Entity.Input.GetRoadStatusRequest request,
                Entity.Output.GetRoadStatusResponse response)
            {
                _roadRepositoryMock.Setup(m => m.GetRoadStatus(It.Is<Entity.Input.GetRoadStatusRequest>(r =>
                    r.RoadId == request.RoadId)))
                    .Returns(response)
                    .Verifiable();
            }

            public void VerifyMocks()
            {
                _requestValidatorMock.VerifyAll();
                _requestValidatorMock.VerifyNoOtherCalls();
                _roadMapperMock.VerifyAll();
                _roadMapperMock.VerifyNoOtherCalls();
                _roadRepositoryMock.VerifyAll();
                _roadRepositoryMock.VerifyNoOtherCalls();
            }
        }

        [Test]
        public void GetRoadStatus_GivenValidationFailed_ThenReturnCorrectResponse()
        {
            var mockProvider = new MockProvider();
            var service = mockProvider.GetService();
            var request = mockProvider.GetMockRequest();

            var validationResult = new ValidationResult(new[] { new ValidationFailure("RoadId", "Error!") });
            
            mockProvider.SetUpRequestValidatorMock(request, validationResult);

            var response = service.GetRoadStatus(request);

            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.ResultCode.Should().Be(Contract.Output.GetRoadStatusResultCode.ValidationFailed);
                response.ResponseNotes.Should().Be(validationResult.Errors.First().ErrorMessage);
                response.RoadStatus.Should().BeNull();

                mockProvider.VerifyMocks();
            }
        }

        [Test]
        public void GetRoadStatus_GivenMappingSuccessful_ThenAllExternalCallsMadeAndRepositoryResponseReturnedCorrectly()
        {
            var mockProvider = new MockProvider();
            var service = mockProvider.GetService();
            var request = mockProvider.GetMockRequest();

            var mappedEntityRequest = new Entity.Input.GetRoadStatusRequest()
            {
                RoadId = "avenue"
            };
            var repoResponse = new Entity.Output.GetRoadStatusResponse()
            {
                ResultCode = Entity.Output.GetRoadStatusResultCode.InvalidRoad,
                ResponseNotes = "hello",
                RoadStatus = new Entity.Output.RoadStatusResult()
                {
                    Status = "stat",
                    StatusDescription = "desc",
                    Id = "xxx",
                    Name = "yyy"
                }
            };
            var mappedContractResponse = new Contract.Output.GetRoadStatusResponse()
            {
                ResultCode = Contract.Output.GetRoadStatusResultCode.Successful,
                ResponseNotes = "world",
                RoadStatus = new Contract.Output.RoadStatusResult()
                {
                    Status = "stats",
                    StatusDescription = "description",
                    Id = "aaa",
                    Name = "bbb"
                }
            };

            mockProvider.SetUpRequestValidatorMock(request);
            mockProvider.SetUpMapToEntityRequestMock(request, mappedEntityRequest);
            mockProvider.SetUpRoadRepositoryMock(mappedEntityRequest, repoResponse);
            mockProvider.SetUpMapToContractResponseMock(repoResponse, mappedContractResponse);

            var response = service.GetRoadStatus(request);

            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.ResultCode.Should().Be(mappedContractResponse.ResultCode);
                response.ResponseNotes.Should().Be(mappedContractResponse.ResponseNotes);
                response.RoadStatus.Id.Should().Be(mappedContractResponse.RoadStatus.Id);
                response.RoadStatus.Name.Should().Be(mappedContractResponse.RoadStatus.Name);
                response.RoadStatus.Status.Should().Be(mappedContractResponse.RoadStatus.Status);
                response.RoadStatus.StatusDescription.Should().Be(mappedContractResponse.RoadStatus.StatusDescription);
                
                mockProvider.VerifyMocks();
            }
        }
    }
}
