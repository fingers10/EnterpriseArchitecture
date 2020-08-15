using AutoFixture;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Services;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using static Fingers10.EnterpriseArchitecture.ApplicationCore.Services.CreateAuthorCommand;

namespace UnitTest.Services
{
    public class CreateAuthorCommandTests
    {
        [Fact]
        public async Task Create_Author_Should_Call_Add_Method_And_SaveChanges_Once()
        {
            //Arrange
            var fixture = new Fixture();
            fixture.Inject(DateTimeOffset.Now.AddDays(-fixture.Create<int>()));
            var command = fixture.Create<CreateAuthorCommand>();
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
            result.Value.DateOfDeath?.Value.Should().BeNull();
        }
    }
}
