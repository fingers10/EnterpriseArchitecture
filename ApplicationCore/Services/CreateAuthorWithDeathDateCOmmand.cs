using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Decorators;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("UnitTest")]
namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class CreateAuthorWithDeathDateCommand : ICommand<Author>
    {
        public CreateAuthorWithDeathDateCommand(string firstName, string lastName, DateTimeOffset dateOfBirth,
            DateTimeOffset dateOfDeath, string mainCategory)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            DateOfDeath = dateOfDeath;
            MainCategory = mainCategory;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public DateTimeOffset DateOfBirth { get; private set; }
        public DateTimeOffset DateOfDeath { get; private set; }
        public string MainCategory { get; }

        [AuditLog]
        [DatabaseRetry]
        internal sealed class AddCommandHandler : ICommandHandler<CreateAuthorWithDeathDateCommand, Author>
        {
            private readonly IUnitOfWork _unitOfWork;

            public AddCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            }

            public async Task<Result<Author>> Handle(CreateAuthorWithDeathDateCommand command)
            {
                var nameResult = Name.Create(command.FirstName, command.LastName);
                var birthDateResult = BirthDate.Create(command.DateOfBirth);
                var deathDateResult = DeathDate.Create(command.DateOfDeath);
                var mainCategoryResult = Entities.Authors.MainCategory.Create(command.MainCategory);

                var authorResult = Result.Combine(nameResult, birthDateResult, deathDateResult, mainCategoryResult)
                                         .Ensure(() => birthDateResult.Value.Value.Date < deathDateResult.Value.Value.Value.Date, "Death date should not be less than birth date.")
                                         .Map(() => new Author(nameResult.Value, birthDateResult.Value, deathDateResult.Value, mainCategoryResult.Value));

                if (authorResult.IsFailure)
                    return Result.Failure<Author>(authorResult.Error);

                await _unitOfWork.AuthorRepository.AddAsync(authorResult.Value).ConfigureAwait(false);

                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return Result.Success(authorResult.Value);
            }
        }
    }
}
