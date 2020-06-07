using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using Fingers10.EnterpriseArchitecture.Infrastructure.Data;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Repositories
{
    public class BookRepository : GenericEFRepository<Book>
    {
        public BookRepository(Fingers10Context context) : base(context)
        {
        }
    }
}
