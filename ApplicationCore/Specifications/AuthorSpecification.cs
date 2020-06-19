using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Specifications
{
    public sealed class AuthorSpecification : BaseSpecification<Author>
    {
        public AuthorSpecification(string mainCategory) : base(x => x.MainCategory == mainCategory)
        {
        }

        public AuthorSpecification(string mainCategory, int pageNumber, int pageSize)
            : base(x => x.MainCategory == mainCategory)
        {
            ApplyPaging((pageNumber - 1) * pageSize, pageSize);
        }

        public AuthorSpecification(int pageNumber, int pageSize)
        {
            ApplyPaging((pageNumber - 1) * pageSize, pageSize);
        }
    }
}
