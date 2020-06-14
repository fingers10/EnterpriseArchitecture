using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Helpers;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Specifications;
using System.Linq;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class GetBooksListQuery : IQuery<PagedList<Book>>
    {
        public GetBooksListQuery(long authorId, int pageNumber, int pageSize)
        {
            AuthorId = authorId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public long AuthorId { get; }
        public int PageNumber { get; }
        public int PageSize { get; }

        internal sealed class GetListQueryHandler : IQueryHandler<GetBooksListQuery, PagedList<Book>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetListQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<PagedList<Book>> Handle(GetBooksListQuery query)
            {
                var countSpec = new BooksByAuthorSpecification(query.AuthorId);
                var listSpec = new BooksByAuthorSpecification(query.AuthorId, query.PageNumber, query.PageSize);

                var count = await _unitOfWork.BookRepository.CountAsync(countSpec);
                var items = await _unitOfWork.BookRepository.ListAsync(listSpec);

                return PagedList<Book>.Create(count, items.ToList(), query.PageNumber, query.PageSize);
            }
        }
    }
}
