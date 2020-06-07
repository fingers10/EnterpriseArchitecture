using Fingers10.EnterpriseArchitecture.ApplicationCore.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class GetAuthorQuery : IQuery<AuthorDto>
    {
        public GetAuthorQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }

        internal sealed class GetListQueryHandler : IQueryHandler<GetAuthorQuery, AuthorDto>
        {
            private readonly IAuthorReadonlyRepository _authorRepository;

            public GetListQueryHandler(IAuthorReadonlyRepository authorRepository)
            {
                _authorRepository = authorRepository;
            }

            public async Task<AuthorDto> Handle(GetAuthorQuery query)
            {
                return await _authorRepository.GetAuthorAsync(query.Id);
            }
        }
    }
}
