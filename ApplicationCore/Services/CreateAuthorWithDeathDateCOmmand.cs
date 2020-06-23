using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Decorators;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System;
using System.Threading.Tasks;

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
                Result<Name> nameResult = Name.Create(command.FirstName, command.LastName);
                if (nameResult.IsFailure)
                    return Result.Failure<Author>(nameResult.Error);

                Result<BirthDate> birthDateResult = BirthDate.Create(command.DateOfBirth);
                if (birthDateResult.IsFailure)
                    return Result.Failure<Author>(birthDateResult.Error);

                Result<DeathDate> deathDateResult = DeathDate.Create(command.DateOfDeath);
                if (deathDateResult.IsFailure)
                    return Result.Failure<Author>(deathDateResult.Error);

                Result<MainCategory> mainCategoryResult = Entities.Authors.MainCategory.Create(command.MainCategory);
                if (mainCategoryResult.IsFailure)
                    return Result.Failure<Author>(mainCategoryResult.Error);

                var author = new Author(nameResult.Value, birthDateResult.Value, deathDateResult.Value, mainCategoryResult.Value);

                await _unitOfWork.AuthorRepository.AddAsync(author);

                await _unitOfWork.SaveChangesAsync();

                return Result.Success(author);
            }
        }
    }
}
