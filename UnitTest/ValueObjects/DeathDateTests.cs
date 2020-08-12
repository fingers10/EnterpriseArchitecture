using AutoFixture;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using FluentAssertions;
using System;
using Xunit;

namespace UnitTest.ValueObjects
{
    public class DeathDateTests
    {
        [Fact]
        public void Create_DeathDate_Should_Fail_For_NullDate()
        {
            //Arrange
            DateTimeOffset? deathDate = null;

            //Act
            var result = DeathDate.Create(deathDate);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Death Date should not be null.");
        }

        [Fact]
        public void Create_DeathDate_Should_Fail_For_FutureDate()
        {
            //Arrange
            var fixture = new Fixture();
            var deathDate = fixture.Create<DateTimeOffset>().AddDays(fixture.Create<int>());

            //Act
            var result = DeathDate.Create(deathDate);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Death Date should not be future date.");
        }

        [Fact]
        public void Create_DeathDate_Should_Create_For_PastDate()
        {
            //Arrange
            var fixture = new Fixture();
            var deathDate = fixture.Create<DateTimeOffset>().AddDays(-fixture.Create<int>());

            //Act
            var result = DeathDate.Create(deathDate);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().BeSameDateAs(deathDate);
        }
    }
}
