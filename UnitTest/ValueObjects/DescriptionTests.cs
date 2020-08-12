using AutoFixture;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using FluentAssertions;
using Xunit;

namespace UnitTest.ValueObjects
{
    public class DescriptionTests
    {
        [Fact]
        public void Create_RequiredDescription_Should_Fail_For_EmptyValue()
        {
            //Arrange
            var requiredDescription = string.Empty;

            //Act
            var result = RequiredDescription.Create(requiredDescription);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Description is required.");
        }

        [Fact]
        public void Create_RequiredDescription_Should_Fail_For_MoreThan1500CharacterValue()
        {
            //Arrange
            var fixture = new Fixture();
            var requiredDescription = string.Join(string.Empty, fixture.CreateMany<string>(100));

            //Act
            var result = RequiredDescription.Create(requiredDescription);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Description is too long.");
        }

        [Fact]
        public void Create_RequiredDescription_Should_Create_For_ValidValue()
        {
            //Arrange
            var fixture = new Fixture();
            var requiredDescription = fixture.Create<string>();

            //Act
            var result = RequiredDescription.Create(requiredDescription);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(requiredDescription.Trim());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_Description_Should_Create_For_EmptyOrNullValue(string description)
        {
            //Act
            var result = Description.Create(description);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(description);
        }
    }
}
