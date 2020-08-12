using AutoFixture;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using FluentAssertions;
using System;
using Xunit;

namespace UnitTest.ValueObjects
{
    public class BirthDateTests
    {
        [Fact]
        public void Create_BirthDate_Should_Fail_For_FutureDate()
        {
            //Arrange
            var fixture = new Fixture();
            var birthDate = fixture.Create<DateTimeOffset>().AddDays(fixture.Create<int>());

            //Act
            var result = BirthDate.Create(birthDate);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Birth Date should not be future date.");
        }

        [Fact]
        public void Create_BirthDate_Should_Create_For_PastDate()
        {
            //Arrange
            var fixture = new Fixture();
            var birthDate = fixture.Create<DateTimeOffset>().AddDays(-fixture.Create<int>());

            //Act
            var result = BirthDate.Create(birthDate);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().BeSameDateAs(birthDate);
        }
    }
}
