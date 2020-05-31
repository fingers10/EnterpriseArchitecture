using Fingers10.EnterpriseArchitecture.ApplicationCore.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class GetAuthorsListQuery : IQuery<List<AuthorDto>>
    {
        public GetAuthorsListQuery(string mainCategory)
        {
            MainCategory = mainCategory;
        }

        public string MainCategory { get; }

        internal sealed class GetListQueryHandler : IQueryHandler<GetAuthorsListQuery, List<AuthorDto>>
        {
            private readonly IAuthorReadonlyRepository _authorRepository;

            public GetListQueryHandler(IAuthorReadonlyRepository authorRepository)
            {
                _authorRepository = authorRepository;
            }

            public async Task<List<AuthorDto>> Handle(GetAuthorsListQuery query)
            {
                return await _authorRepository.GetAllAsync(query.MainCategory);
            }
        }
    }
}
