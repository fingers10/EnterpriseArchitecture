using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Decorators;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class CreateAuthorCommand : ICommand<Author>
    {
        public CreateAuthorCommand(string firstName, string lastName, DateTimeOffset dateOfBirth, string mainCategory,
            IEnumerable<CreateBookDto> books)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            MainCategory = mainCategory;
            Books = books;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public DateTimeOffset DateOfBirth { get; private set; }
        public string MainCategory { get; }
        public IEnumerable<CreateBookDto> Books { get; }

        [AuditLog]
        [DatabaseRetry]
        internal sealed class AddCommandHandler : ICommandHandler<CreateAuthorCommand, Author>
        {
            private readonly IUnitOfWork _unitOfWork;

            public AddCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            }

            public async Task<Result<Author>> Handle(CreateAuthorCommand command)
            {
                Result<Name> nameResult = Name.Create(command.FirstName, command.LastName);
                if (nameResult.IsFailure)
                    return Result.Failure<Author>(nameResult.Error);

                Result<BirthDate> birthDateResult = BirthDate.Create(command.DateOfBirth);
                if (birthDateResult.IsFailure)
                    return Result.Failure<Author>(birthDateResult.Error);

                Result<MainCategory> mainCategoryResult = Entities.Authors.MainCategory.Create(command.MainCategory);
                if (mainCategoryResult.IsFailure)
                    return Result.Failure<Author>(mainCategoryResult.Error);

                var author = new Author(nameResult.Value, birthDateResult.Value, mainCategoryResult.Value);
                //author.AddBooks(command.Books);

                await _unitOfWork.AuthorRepository.AddAsync(author);

                await _unitOfWork.SaveChangesAsync();

                return Result.Success(author);
            }
        }
    }
}
