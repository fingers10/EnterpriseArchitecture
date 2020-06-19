using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class GetAuthorQuery : IQuery<Author>
    {
        public GetAuthorQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }

        internal sealed class GetListQueryHandler : IQueryHandler<GetAuthorQuery, Author>
        {
            private readonly IUnitOfWork _unitOfWork;

            //private readonly IAuthorReadonlyRepository _authorRepository;

            //public GetListQueryHandler(IAuthorReadonlyRepository authorRepository)
            //{
            //    _authorRepository = authorRepository;
            //}

            //public async Task<AuthorDto> Handle(GetAuthorQuery query)
            //{
            //    return await _authorRepository.GetAuthorAsync(query.Id);
            //}

            public GetListQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Author> Handle(GetAuthorQuery query)
            {
                return await _unitOfWork.AuthorRepository.FindAsync(query.Id);
            }
        }
    }
}
