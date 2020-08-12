using AutoFixture;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using FluentAssertions;
using Xunit;

namespace UnitTest.ValueObjects
{
    public class MainCategoryTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_MainCategory_Should_Fail_For_EmptyNullValue(string mainCategory)
        {
            //Act
            var result = MainCategory.Create(mainCategory);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Main Category is required.");
        }

        [Fact]
        public void Create_MainCategory_Should_Fail_For_MoreThan50CharacterValue()
        {
            //Arrange
            var fixture = new Fixture();
            var mainCategory = string.Join(string.Empty, fixture.CreateMany<string>(2));

            //Act
            var result = MainCategory.Create(mainCategory);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Main Category is too long.");
        }

        [Fact]
        public void Create_MainCategory_Should_Create_For_ValidValue()
        {
            //Arrange
            var fixture = new Fixture();
            var mainCategory = fixture.Create<string>();

            //Act
            var result = MainCategory.Create(mainCategory);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Value.Should().Be(mainCategory.Trim());
        }
    }
}
