using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.DTO;
using TFL.Road.StatusCheck.Infrastructure.TFLOpenData.Mappers;

namespace TFL.Road.StatusCheck.Tests.Infrastructure.Mappers
{
    public class RoadMapperTests
    {
        [Test]
        public void Map_RoadStatus_DTOToEntity_GivenNull_ShouldReturnNull()
        {
            var mapper = new RoadMapper();
            
            var entity = mapper.Map(default(RoadStatus));

            Assert.IsNull(entity);
        }

        [Test]
        public void Map_RoadStatus_DTOToEntity_GivenValidScenario_ShouldReturnCorrectMapping()
        {
            var mapper = new RoadMapper();

            var dto = new RoadStatus()
            {
                Id = "id",
                DisplayName = "name",
                StatusSeverity = "severity",
                StatusSeverityDescription = "description"
            };

            var entity = mapper.Map(dto);

            using (new AssertionScope())
            {
                entity.Should().NotBeNull();
                entity.Id.Should().Be(dto.Id);
                entity.Name.Should().Be(dto.DisplayName);
                entity.Status.Should().Be(dto.StatusSeverity);
                entity.StatusDescription.Should().Be(dto.StatusSeverityDescription);
            }
        }
    }
}
