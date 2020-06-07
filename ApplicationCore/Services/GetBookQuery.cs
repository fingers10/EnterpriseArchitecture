using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Specifications;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class GetBookQuery : IQuery<Book>
    {
        public GetBookQuery(long authorId, long bookId)
        {
            AuthorId = authorId;
            BookId = bookId;
        }

        public long AuthorId { get; }
        public long BookId { get; }

        internal sealed class GetListQueryHandler : IQueryHandler<GetBookQuery, Book>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetListQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Book> Handle(GetBookQuery query)
            {
                var spec = new BooksByAuthorSpecification(query.AuthorId, query.BookId);

                return await _unitOfWork.BookRepository.FirstOrDefaultAsync(spec);
            }
        }
    }
}
