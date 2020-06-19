using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Helpers;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class GetAuthorsListQuery : IQuery<PagedList<Author>>
    {
        public GetAuthorsListQuery(string mainCategory, int pageNumber, int pageSize)
        {
            MainCategory = mainCategory;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public string MainCategory { get; }
        public int PageNumber { get; }
        public int PageSize { get; }

        internal sealed class GetListQueryHandler : IQueryHandler<GetAuthorsListQuery, PagedList<Author>>
        {
            private readonly IUnitOfWork _unitOfWork;

            //private readonly IAuthorReadonlyRepository _authorRepository;

            //public GetListQueryHandler(IAuthorReadonlyRepository authorRepository)
            //{
            //    _authorRepository = authorRepository;
            //}

            //public async Task<List<Author>> Handle(GetAuthorsListQuery query)
            //{
            //    return await _authorRepository.GetAllAsync(query.MainCategory);
            //}

            public GetListQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<PagedList<Author>> Handle(GetAuthorsListQuery query)
            {
                IReadOnlyList<Author> items;
                int count;

                if (!string.IsNullOrWhiteSpace(query.MainCategory))
                {
                    var countSpec = new AuthorSpecification(query.MainCategory);
                    var listSpec = new AuthorSpecification(query.MainCategory,
                        query.PageNumber, query.PageSize);

                    count = await _unitOfWork.AuthorRepository.CountAsync(countSpec);
                    items = await _unitOfWork.AuthorRepository.ListAsync(listSpec);
                }
                else
                {
                    var listSpec = new AuthorSpecification(query.PageNumber, query.PageSize);
                    count = (await _unitOfWork.AuthorRepository.ListAllAsync()).Count;
                    items = await _unitOfWork.AuthorRepository.ListAsync(listSpec);
                }

                return PagedList<Author>.Create(count, items.ToList(), query.PageNumber, query.PageSize);
            }
        }
    }
}
