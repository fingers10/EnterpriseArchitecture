using AutoFixture;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Services;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using static Fingers10.EnterpriseArchitecture.ApplicationCore.Services.DeleteAuthorCommand;

namespace UnitTest.Services
{
    public class DeleteAuthorCommandTests
    {
        [Fact]
        public async Task Delete_Author_Should_Fail_For_Null_Author()
        {
            //Arrange
            var fixture = new Fixture();
            var command = fixture.Create<DeleteAuthorCommand>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAuthorRepository = new Mock<IAsyncRepository<Author>>();
            mockAuthorRepository.Setup(x => x.FindAsync(It.IsAny<long>())).ReturnsAsync(It.IsAny<Author>());
            mockUnitOfWork.Setup(x => x.AuthorRepository).Returns(mockAuthorRepository.Object);
            var removeAuthorHandler = new RemoveCommandHandler(mockUnitOfWork.Object);

            //Act
            var result = await removeAuthorHandler.Handle(command);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be($"No Author found for Id {command.Id}.");
        }

        [Fact]
        public async Task Delete_Author_Should_Call_Remove_Method_And_SaveChanges_Once()
        {
            //Arrange
            var fixture = new Fixture();
            var command = fixture.Create<DeleteAuthorCommand>();
            fixture.Customizations.Add(new CurrentDateTimeGenerator());
            fixture.Register(() => Name.Create(fixture.Create<string>(), fixture.Create<string>()).Value);
            fixture.Register(() => BirthDate.Create(fixture.Create<DateTimeOffset>().AddDays(-fixture.Create<int>())).Value);
            fixture.Register(() => DeathDate.Create(fixture.Create<DateTimeOffset>().AddDays(-fixture.Create<int>())).Value);
            fixture.Register(() => MainCategory.Create(fixture.Create<string>()).Value);
            var author = fixture.Create<Author>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAuthorRepository = new Mock<IAsyncRepository<Author>>();
            mockAuthorRepository.Setup(x => x.FindAsync(It.IsAny<long>())).ReturnsAsync(author);
            mockUnitOfWork.Setup(x => x.AuthorRepository).Returns(mockAuthorRepository.Object);
            var removeAuthorHandler = new RemoveCommandHandler(mockUnitOfWork.Object);

            //Act
            var result = await removeAuthorHandler.Handle(command);

            //Assert
            mockAuthorRepository.Verify(x => x.Remove(It.IsAny<Author>()), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
            result.IsSuccess.Should().BeTrue();
        }
    }
}
