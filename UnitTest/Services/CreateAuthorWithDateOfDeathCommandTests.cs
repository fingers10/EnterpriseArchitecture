using AutoFixture;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Services;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using UnitTest.Utils;
using Xunit;
using static Fingers10.EnterpriseArchitecture.ApplicationCore.Services.CreateAuthorWithDeathDateCommand;

namespace UnitTest.Services
{
    public class CreateAuthorWithDateOfDeathCommandTests
    {
        [Fact]
        public async Task Create_Author_With_Same_BirthDate_And_DeathDate_Should_Fail_With_Error()
        {
            //Arrange
            var fixture = new Fixture();
            fixture.Inject(DateTimeOffset.Now.AddDays(-fixture.Create<int>()));
            var command = fixture.Create<CreateAuthorWithDeathDateCommand>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var addAuthorHandler = new AddCommandHandler(mockUnitOfWork.Object);

            //Act
            var result = await addAuthorHandler.Handle(command);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be("Death date should not be less than birth date.");
        }

        [Fact]
        public async Task Create_Author_Should_Call_Add_Method_And_SaveChanges_Once()
        {
            //Arrange
            var fixture = new Fixture();
            var command = fixture.For<CreateAuthorWithDeathDateCommand>()
                                 .With(x => x.DateOfBirth, DateTimeOffset.Now.AddDays(-2))
                                 .With(x => x.DateOfDeath, DateTimeOffset.Now.AddDays(-1))
                                 .Create();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAuthorRepository = new Mock<IAsyncRepository<Author>>();
            mockUnitOfWork.Setup(x => x.AuthorRepository).Returns(mockAuthorRepository.Object);
            var addAuthorHandler = new AddCommandHandler(mockUnitOfWork.Object);

            //Act
            var result = await addAuthorHandler.Handle(command);

            //Assert
            mockAuthorRepository.Verify(x => x.AddAsync(It.IsAny<Author>()), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            result.IsSuccess.Should().BeTrue();
            result.Value.Name.First.Should().Be(command.FirstName);
            result.Value.Name.Last.Should().Be(command.LastName);
            result.Value.DateOfBirth.Value.Should().Be(command.DateOfBirth);
            result.Value.MainCategory.Value.Should().Be(command.MainCategory);
            result.Value.DateOfDeath.Value.Value.Should().Be(command.DateOfDeath);
        }
    }
}
