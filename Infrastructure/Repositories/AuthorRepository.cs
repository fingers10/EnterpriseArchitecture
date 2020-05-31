using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Author;
using Fingers10.EnterpriseArchitecture.Infrastructure.Data;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Repositories
{
    public class AuthorRepository : GenericEFRepository<Author>
    {
        public AuthorRepository(Fingers10Context context) : base(context)
        {
        }
    }
}
