using AutoFixture;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using FluentAssertions;
using Xunit;

namespace UnitTest.ValueObjects
{
    public class NameTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void Create_Name_Should_Fail_For_EmptyNullFirstNameAndLastNameValue(string firstName, string lastName)
        {
            //Act
            var result = Name.Create(firstName, lastName);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("First name should not be empty., Last name should not be empty.");
        }

        [Fact]
        public void Create_Name_Should_Fail_For_MoreThan50CharacterFirstNameAndLastNameValue()
        {
            //Arrange
            var fixture = new Fixture();
            var firstName = string.Join(string.Empty, fixture.CreateMany<string>(2));
            var lastName = string.Join(string.Empty, fixture.CreateMany<string>(2));

            //Act
            var result = Name.Create(firstName, lastName);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("First name is too long., Last name is too long.");
        }

        [Fact]
        public void Create_Name_Should_Create_For_ValidValue()
        {
            //Arrange
            var fixture = new Fixture();
            var firstName = fixture.Create<string>();
            var lastName = fixture.Create<string>();

            //Act
            var result = Name.Create(firstName, lastName);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.First.Should().Be(firstName.Trim());
            result.Value.Last.Should().Be(lastName.Trim());
        }
    }
}
