using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using Fingers10.EnterpriseArchitecture.Infrastructure.Repositories;

namespace Fingers10.EnterpriseArchitecture.API.IntegrationTest.Fakes
{
    public class FakeBookRepository : GenericEFRepository<Book, FakeDatabase>
    {
        public FakeBookRepository(FakeDatabase context) : base(context)
        {
        }
    }
}
