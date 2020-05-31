using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Decorators;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Author;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class DeleteAuthorCommand : ICommand<Author>
    {
        public DeleteAuthorCommand(int id)
        {
            Id = id;
        }

        public long Id { get; }

        [AuditLog]
        [DatabaseRetry]
        internal sealed class RemoveCommandHandler : ICommandHandler<DeleteAuthorCommand, Author>
        {
            private readonly IUnitOfWork _unitOfWork;

            public RemoveCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            }

            public async Task<Result<Author>> Handle(DeleteAuthorCommand command)
            {
                var author = await _unitOfWork.AuthorRepository.FindAsync(command.Id);

                if (author is null)
                    return Result.Failure<Author>($"No Author found for Id {command.Id}");

                _unitOfWork.AuthorRepository.Remove(author);

                await _unitOfWork.SaveChangesAsync();

                return Result.Success(author);
            }
        }
    }
}
