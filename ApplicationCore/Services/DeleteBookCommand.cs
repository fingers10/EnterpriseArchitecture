using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Decorators;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class DeleteBookCommand : ICommand<Book>
    {
        public DeleteBookCommand(long id)
        {
            Id = id;
        }

        public long Id { get; }

        [AuditLog]
        [DatabaseRetry]
        internal sealed class RemoveCommandHandler : ICommandHandler<DeleteBookCommand, Book>
        {
            private readonly IUnitOfWork _unitOfWork;

            public RemoveCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            }

            public async Task<Result<Book>> Handle(DeleteBookCommand command)
            {
                var book = await _unitOfWork.BookRepository.FindAsync(command.Id);

                if (book is null)
                    return Result.Failure<Book>($"No Book found for Id {command.Id}");

                _unitOfWork.BookRepository.Remove(book);

                await _unitOfWork.SaveChangesAsync();

                return Result.Success(book);
            }
        }
    }
}
