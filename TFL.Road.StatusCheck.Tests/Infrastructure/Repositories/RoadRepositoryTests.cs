using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using TFL.Road.StatusCheck.Application.Entities.Road.V1.Input;
using TFL.Road.StatusCheck.Application.Entities.Road.V1.Output;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Client;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.DTO;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Mappers;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Repositories;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Serialiser;

namespace TFL.Road.StatusCheck.Tests.Infrastructure.Repositories
{
    public class RoadRepositoryTests
    {
        private class MockProvider
        {
            private Mock<ITFLOpenDataClient> _tflOpenDataClientMock = new Mock<ITFLOpenDataClient>();
            private Mock<IResponseSerialiser> _responseSerialiserMock = new Mock<IResponseSerialiser>();
            private Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
            private Mock<IRoadMapper> _roadMapper = new Mock<IRoadMapper>();

            private string _expectedQuery;

            public RoadRepository GetRepository()
            {
                return new RoadRepository(_tflOpenDataClientMock.Object, _responseSerialiserMock.Object,
                    _configurationMock.Object, _roadMapper.Object);
            }

            public GetRoadStatusRequest GetMockRequest(string roadId)
            {
                return new GetRoadStatusRequest() { RoadId = roadId };
            }

            public void SetUpConfigurationGetSectionMock(string value, GetRoadStatusRequest request)
            {
                _configurationMock.Setup(m => m.GetSection("TFLOpenData:Endpoints:Road").Value)
                    .Returns(value)
                    .Verifiable();

                _expectedQuery = $"{value}/{request.RoadId}";
            }

            public void SetUpClientCallMock(HttpResponseMessage response)
            {
                _tflOpenDataClientMock.Setup(m => m.GetDataAsync(_expectedQuery))
                    .ReturnsAsync(response)
                    .Verifiable();
            }

            public void SetUpDeserialiseRoadStatusesMock(HttpResponseMessage response, IEnumerable<RoadStatus> roadStatuses)
            {
                _responseSerialiserMock.Setup(m => m.DeserialiseResponseAsync<IEnumerable<RoadStatus>>(
                        It.Is<HttpResponseMessage>(r =>
                            r.StatusCode == response.StatusCode &&
                            r.Content.ToString() == response.Content.ToString())))
                    .ReturnsAsync(roadStatuses)
                    .Verifiable();
            }

            public void SetUpDeserialiseErrorResponseMock(HttpResponseMessage response, ErrorResponse errorResponse)
            {
                _responseSerialiserMock.Setup(m => m.DeserialiseResponseAsync<ErrorResponse>(
                        It.Is<HttpResponseMessage>(r =>
                            r.StatusCode == response.StatusCode &&
                            r.Content.ToString() == response.Content.ToString())))
                    .ReturnsAsync(errorResponse)
                    .Verifiable();
            }

            public void SetUpRoadMapperMapToEntityMock(RoadStatus input, RoadStatusResult output)
            {
                _roadMapper.Setup(m => m.Map(It.Is<RoadStatus>(r =>
                        r.Id == input.Id &&
                        r.DisplayName == input.DisplayName &&
                        r.StatusSeverity == input.StatusSeverity &&
                        r.StatusSeverityDescription == input.StatusSeverityDescription)))
                    .Returns(output)
                    .Verifiable();
            }

            public void VerifyMocks()
            {
                _tflOpenDataClientMock.VerifyAll();
                _tflOpenDataClientMock.VerifyNoOtherCalls();
                _responseSerialiserMock.VerifyAll();
                _responseSerialiserMock.VerifyNoOtherCalls();
                _configurationMock.VerifyAll();
                _configurationMock.VerifyNoOtherCalls();
                _roadMapper.VerifyAll();
                _roadMapper.VerifyNoOtherCalls();
            }
        }

        #region Response creation test(s)

        #region CreateResponseForOK test(s)

        private static object[] _createResponseForOKInvalidScenarios = new[]
        {
            new object[] { null },
            new object[] { Enumerable.Empty<RoadStatus>() },
            new object[] { new[] { default(RoadStatus) } }
        };

        [TestCaseSource(nameof(_createResponseForOKInvalidScenarios))]
        public void CreateResponseForOK_GivenInvalidValues_ThenErrorResponseReturned(IEnumerable<RoadStatus> roadStatuses)
        {
            var mockProvider = new MockProvider();
            var repo = mockProvider.GetRepository();

            var result = repo.CreateResponseForOK(roadStatuses);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.ResultCode.Should().Be(GetRoadStatusResultCode.OtherFailure);
                result.ResponseNotes.Should().Be("No results received.");

                mockProvider.VerifyMocks();
            }
        }

        [Test]
        public void CreateResponseForOK_GivenValidValues_ThenCorrectResponseReturned()
        {
            var mockProvider = new MockProvider();
            var repo = mockProvider.GetRepository();

            var roadStatuses = new[]
            {
                new RoadStatus() { Id = "1", DisplayName = "2", StatusSeverity = "3", StatusSeverityDescription = "4" },
                new RoadStatus() { Id = "a", DisplayName = "b", StatusSeverity = "c", StatusSeverityDescription = "d" }
            };

            mockProvider.SetUpRoadMapperMapToEntityMock(roadStatuses.First(), new RoadStatusResult());

            var result = repo.CreateResponseForOK(roadStatuses);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.ResultCode.Should().Be(GetRoadStatusResultCode.Successful);
                result.ResponseNotes.Should().BeNull();
                result.RoadStatus.Should().NotBeNull();

                mockProvider.VerifyMocks();
            }
        }
        #endregion

        #region CreateResponseForNotFound test(s)

        [Test]
        public void CreateResponseForNotFound_GivenErrorCausedByInvalidRoad_ThenCorrectResponseReturned()
        {
            var mockProvider = new MockProvider();
            var repo = mockProvider.GetRepository();

            var errorResponse = new ErrorResponse() { Message = "The following road id is not recognised: A112" };

            var result = repo.CreateResponseForNotFound(errorResponse);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.ResultCode.Should().Be(GetRoadStatusResultCode.InvalidRoad);
                result.ResponseNotes.Should().BeNull();
                result.RoadStatus.Should().BeNull();

                mockProvider.VerifyMocks();
            }
        }

        [Test]
        public void CreateResponseForNotFound_GivenErrorNotCausedByInvalidRoad_ThenErrorResponseReturned()
        {
            var mockProvider = new MockProvider();
            var repo = mockProvider.GetRepository();

            var errorResponse = new ErrorResponse() { Message = "Resource not found: http" };

            var result = repo.CreateResponseForNotFound(errorResponse);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.ResultCode.Should().Be(GetRoadStatusResultCode.OtherFailure);
                result.ResponseNotes.Should().Be(errorResponse.Message);

                mockProvider.VerifyMocks();
            }
        }

        #endregion

        #region CreateResponseForOtherFailures test(s)

        [Test]
        public void CreateResponseForOtherFailures_GivenOnlyScenario_ThenCorrectResponseReturned()
        {
            var mockProvider = new MockProvider();
            var repo = mockProvider.GetRepository();

            var message = "This is a test.";

            var result = repo.CreateResponseForOtherFailures(message);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.ResultCode.Should().Be(GetRoadStatusResultCode.OtherFailure);
                result.ResponseNotes.Should().Be(message);

                mockProvider.VerifyMocks();
            }
        }

        #endregion

        #endregion

        #region GetRoadStatusAsync test(s)

        [Test]
        public async Task GetRoadStatusAsync_GivenResponseIsOK_ThenReturnCorrectResponse()
        {
            var mockProvider = new MockProvider();
            var repo = mockProvider.GetRepository();

            var request = mockProvider.GetMockRequest("rd");

            var content = "test";
            var mockedHttpResponse = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent(content) };
            var mockedRoadStatuses = new[] { new RoadStatus() { Id = "rd1" } };
            
            mockProvider.SetUpConfigurationGetSectionMock("Endpoint", request);
            mockProvider.SetUpClientCallMock(mockedHttpResponse);
            mockProvider.SetUpDeserialiseRoadStatusesMock(mockedHttpResponse, mockedRoadStatuses);
            mockProvider.SetUpRoadMapperMapToEntityMock(mockedRoadStatuses.First(), new RoadStatusResult());

            var response = await repo.GetRoadStatusAsync(request);

            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.ResultCode.Should().Be(GetRoadStatusResultCode.Successful);
                response.RoadStatus.Should().NotBeNull();

                mockProvider.VerifyMocks();
            }
        }

        [TestCase("The following road id is not recognised", GetRoadStatusResultCode.InvalidRoad)]
        [TestCase("Test", GetRoadStatusResultCode.OtherFailure)]
        public async Task GetRoadStatusAsync_GivenResponseIsNotFound_ThenReturnCorrectResponse(string errorMessage, GetRoadStatusResultCode expectedResultCode)
        {
            var mockProvider = new MockProvider();
            var repo = mockProvider.GetRepository();

            var request = mockProvider.GetMockRequest("rd");

            var content = "test";
            var mockedHttpResponse = new HttpResponseMessage() { StatusCode = HttpStatusCode.NotFound, Content = new StringContent(content) };
            var mockedErrorResponse = new ErrorResponse() { Message = errorMessage };

            mockProvider.SetUpConfigurationGetSectionMock("Endpoint", request);
            mockProvider.SetUpClientCallMock(mockedHttpResponse);
            mockProvider.SetUpDeserialiseErrorResponseMock(mockedHttpResponse, mockedErrorResponse);
            
            var response = await repo.GetRoadStatusAsync(request);

            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.ResultCode.Should().Be(expectedResultCode);
                response.RoadStatus.Should().BeNull();

                mockProvider.VerifyMocks();
            }
        }

        [Test]
        public async Task GetRoadStatusAsync_GivenResponseIsTooManyRequests_ThenReturnCorrectResponse()
        {
            var mockProvider = new MockProvider();
            var repo = mockProvider.GetRepository();

            var request = mockProvider.GetMockRequest("rd");

            var content = "test";
            var mockedHttpResponse = new HttpResponseMessage() { StatusCode = HttpStatusCode.TooManyRequests, Content = new StringContent(content) };
            
            mockProvider.SetUpConfigurationGetSectionMock("Endpoint", request);
            mockProvider.SetUpClientCallMock(mockedHttpResponse);
            
            var response = await repo.GetRoadStatusAsync(request);

            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.ResultCode.Should().Be(GetRoadStatusResultCode.OtherFailure);
                response.ResponseNotes.Should().Be(content);

                mockProvider.VerifyMocks();
            }
        }

        [TestCase(HttpStatusCode.Accepted)]
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.Locked)]
        public async Task GetRoadStatusAsync_GivenResponseIsAnyOtherHttpStatusCode_ThenReturnCorrectResponse(HttpStatusCode httpStatusCode)
        {
            var mockProvider = new MockProvider();
            var repo = mockProvider.GetRepository();

            var request = mockProvider.GetMockRequest("rd");

            var content = "test";
            var mockedHttpResponse = new HttpResponseMessage() { StatusCode = httpStatusCode, Content = new StringContent(content) };

            mockProvider.SetUpConfigurationGetSectionMock("Endpoint", request);
            mockProvider.SetUpClientCallMock(mockedHttpResponse);

            var response = await repo.GetRoadStatusAsync(request);

            using (new AssertionScope())
            {
                response.Should().NotBeNull();
                response.ResultCode.Should().Be(GetRoadStatusResultCode.OtherFailure);
                response.ResponseNotes.Should().Contain("An error occurred").And.Contain(httpStatusCode.ToString());

                mockProvider.VerifyMocks();
            }
        }

        #endregion
    }
}
