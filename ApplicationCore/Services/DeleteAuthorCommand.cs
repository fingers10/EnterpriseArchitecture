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
    public sealed class DeleteAuthorCommand : ICommand<Author>
    {
        public DeleteAuthorCommand(long id)
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
                var author = await _unitOfWork.AuthorRepository.FindAsync(command.Id).ConfigureAwait(false);

                if (author is null)
                    return Result.Failure<Author>($"No Author found for Id {command.Id}.");

                _unitOfWork.AuthorRepository.Remove(author);

                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return Result.Success(author);
            }
        }
    }
}
