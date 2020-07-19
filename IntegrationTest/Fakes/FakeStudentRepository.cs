using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Fingers10.EnterpriseArchitecture.Infrastructure.Repositories;

namespace Fingers10.EnterpriseArchitecture.API.IntegrationTest.Fakes
{
    public class FakeStudentRepository : GenericEFRepository<Student, FakeDatabase>
    {
        public FakeStudentRepository(FakeDatabase context) : base(context)
        {
        }
    }
}
