using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Fingers10.EnterpriseArchitecture.Infrastructure.Data;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Repositories
{
    public class StudentRepository : GenericEFRepository<Student>
    {
        public StudentRepository(Fingers10Context context) : base(context)
        {
        }
    }
}
