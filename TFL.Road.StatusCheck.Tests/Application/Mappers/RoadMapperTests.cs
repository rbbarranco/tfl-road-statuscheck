using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using TFL.Road.StatusCheck.Application.Mappers;
using Contract = TFL.Road.StatusCheck.Contracts.Road.V1;
using Entity = TFL.Road.StatusCheck.Application.Entities.Road.V1;

namespace TFL.Road.StatusCheck.Tests.Application.Mappers
{
    public class RoadMapperTests
    {
        #region GetRoadStatusRequest Contract to Entity Mapping test(s)

        [Test]
        public void Map_GetRoadStatusRequest_ContractToEntity_GivenNull_ShouldReturnNull()
        {
            var mapper = new RoadMapper();

            var entity = mapper.Map(default(Contract.Input.GetRoadStatusRequest));

            entity.Should().BeNull();
        }

        [Test]
        public void Map_GetRoadStatusRequest_ContractToEntity_GivenValidScenario_ShouldReturnCorrectMapping()
        {
            var mapper = new RoadMapper();

            var contract = new Contract.Input.GetRoadStatusRequest
            {
                RoadId = "someRoadId"
            };

            var entity = mapper.Map(contract);

            using (new AssertionScope())
            {
                entity.Should().NotBeNull();
                entity.RoadId.Should().Be(contract.RoadId);
            }
        }

        #endregion

        #region GetRoadStatusRequest Contract to Entity Mapping test(s)

        [Test]
        public void Map_GetRoadStatusResponse_EntityToContract_GivenNull_ShouldReturnNull()
        {
            var mapper = new RoadMapper();

            var contract = mapper.Map(default(Entity.Output.GetRoadStatusResponse));

            contract.Should().BeNull();
        }

        [Test]
        public void Map_GetRoadStatusResponse_EntityToContract_GivenValidScenario_ShouldReturnCorrectMapping()
        {
            var mapper = new RoadMapper();

            var entity = new Entity.Output.GetRoadStatusResponse()
            {
                ResultCode = Entity.Output.GetRoadStatusResultCode.OtherFailure,
                RoadStatus = new Entity.Output.RoadStatusResult()
                {
                    Id = "id",
                    Name = "name",
                    Status = "status",
                    StatusDescription = "description"
                },
                ResponseNotes = "notes"
            };

            var contract = mapper.Map(entity);

            using (new AssertionScope())
            {
                contract.Should().NotBeNull();
                contract.RoadStatus.Should().NotBeNull();
                contract.RoadStatus.Id.Should().Be(entity.RoadStatus.Id);
                contract.RoadStatus.Name.Should().Be(entity.RoadStatus.Name);
                contract.RoadStatus.Status.Should().Be(entity.RoadStatus.Status);
                contract.RoadStatus.StatusDescription.Should().Be(entity.RoadStatus.StatusDescription);
                contract.ResponseNotes.Should().Be(entity.ResponseNotes);
            }
        }

        #endregion

        #region Road status result mapping test(s)

        [Test]
        public void Map_RoadStatusResult_EntityToContract_GivenNull_ShouldReturnNull()
        {
            var mapper = new RoadMapper();

            var contract = mapper.Map(default(Entity.Output.RoadStatusResult));

            contract.Should().BeNull();
        }

        [Test]
        public void Map_RoadStatusResult_EntityToContract_GivenValidScenario_ShouldReturnCorrectMapping()
        {
            var mapper = new RoadMapper();

            var entity = new Entity.Output.RoadStatusResult()
            {
                Id = "id",
                Name = "name",
                Status = "status",
                StatusDescription = "description"
            };

            var contract = mapper.Map(entity);

            using (new AssertionScope())
            {
                contract.Should().NotBeNull();
                contract.Id.Should().Be(entity.Id);
                contract.Name.Should().Be(entity.Name);
                contract.Status.Should().Be(entity.Status);
                contract.StatusDescription.Should().Be(entity.StatusDescription);
            }
        }

        #endregion

        #region Result code mapping test(s)

        [TestCase(Entity.Output.GetRoadStatusResultCode.Successful, Contract.Output.GetRoadStatusResultCode.Successful)]
        [TestCase(Entity.Output.GetRoadStatusResultCode.InvalidRoad, Contract.Output.GetRoadStatusResultCode.InvalidRoad)]
        [TestCase(Entity.Output.GetRoadStatusResultCode.OtherFailure, Contract.Output.GetRoadStatusResultCode.OtherFailure)]
        public void Map_GetRoadStatusResultCode_EntityToContract_GivenDifferentResultCodes_ShouldReturnCorrectMapping(Entity.Output.GetRoadStatusResultCode input, Contract.Output.GetRoadStatusResultCode expectedOutput)
        {
            var mapper = new RoadMapper();

            var contract = mapper.Map(input);

            contract.Should().Be(expectedOutput);
        }

        [Test]
        public void Map_GetRoadStatusResultCode_EntityToContract_GivenInvalidResultCode_ShouldReturnCorrectMapping()
        {
            var mapper = new RoadMapper();

            Assert.Throws<ArgumentOutOfRangeException>(() => mapper.Map((Entity.Output.GetRoadStatusResultCode)1000));
        }

        #endregion
    }
}
