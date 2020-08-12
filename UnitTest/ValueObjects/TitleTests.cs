using AutoFixture;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using FluentAssertions;
using Xunit;

namespace UnitTest.ValueObjects
{
    public class TitleTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_Title_Should_Fail_For_EmptyNullValue(string title)
        {
            //Act
            var result = Title.Create(title);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Title should not be empty.");
        }

        [Fact]
        public void Create_Title_Should_Fail_For_MoreThan100CharacterValue()
        {
            //Arrange
            var fixture = new Fixture();
            var title = string.Join(string.Empty, fixture.CreateMany<string>(3));

            //Act
            var result = Title.Create(title);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Title is too long.");
        }

        [Fact]
        public void Create_Title_Should_Create_For_ValidValue()
        {
            //Arrange
            var fixture = new Fixture();
            var title = fixture.Create<string>();

            //Act
            var result = Title.Create(title);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(title.Trim());
        }
    }
}
