using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces
{
    public interface IUnitOfWork
    {
        IAsyncRepository<Student> StudentRepository { get; }

        Task SaveChangesAsync();
    }
}
