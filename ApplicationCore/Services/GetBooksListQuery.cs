using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class GetBooksListQuery : IQuery<IReadOnlyList<Book>>
    {
        public GetBooksListQuery(long authorId)
        {
            AuthorId = authorId;
        }

        public long AuthorId { get; }

        internal sealed class GetListQueryHandler : IQueryHandler<GetBooksListQuery, IReadOnlyList<Book>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetListQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<IReadOnlyList<Book>> Handle(GetBooksListQuery query)
            {
                var spec = new BooksByAuthorSpecification(query.AuthorId);

                return await _unitOfWork.BookRepository.ListAsync(spec);
            }
        }
    }
}
