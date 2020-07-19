using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.Infrastructure.Repositories;

namespace Fingers10.EnterpriseArchitecture.API.IntegrationTest.Fakes
{
    public class FakeAuthorRepository : GenericEFRepository<Author, FakeDatabase>
    {
        public FakeAuthorRepository(FakeDatabase context) : base(context)
        {
        }
    }
}
