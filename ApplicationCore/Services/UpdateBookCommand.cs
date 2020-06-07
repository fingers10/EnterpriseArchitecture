using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Decorators;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class UpdateBookCommand : ICommand<Book>
    {
        public UpdateBookCommand(long bookId, string title, string description, long authorId)
        {
            BookId = bookId;
            Title = title;
            Description = description;
            AuthorId = authorId;
        }

        public long BookId { get; set; }
        public string Title { get; }
        public string Description { get; }
        public long AuthorId { get; }

        [AuditLog]
        [DatabaseRetry]
        internal sealed class AddCommandHandler : ICommandHandler<UpdateBookCommand, Book>
        {
            private readonly IUnitOfWork _unitOfWork;

            public AddCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            }

            public async Task<Result<Book>> Handle(UpdateBookCommand command)
            {
                var author = await _unitOfWork.AuthorRepository.FindAsync(command.AuthorId);
                if (author is null)
                    return Result.Failure<Book>("Author not found");

                Result<Title> titleResult = Entities.Books.Title.Create(command.Title);
                if (titleResult.IsFailure)
                    return Result.Failure<Book>(titleResult.Error);

                Result<Description> descriptionResult = Entities.Books.Description.Create(command.Description);
                if (descriptionResult.IsFailure)
                    return Result.Failure<Book>(descriptionResult.Error);

                var book = await _unitOfWork.BookRepository.FindAsync(command.BookId);
                book.EditBook(titleResult.Value, descriptionResult.Value, author);

                _unitOfWork.BookRepository.Update(book);

                await _unitOfWork.SaveChangesAsync();

                return Result.Success(book);
            }
        }
    }
}
