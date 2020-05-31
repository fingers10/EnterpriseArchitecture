using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Author;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces
{
    public interface IUnitOfWork
    {
        IAsyncRepository<Student> StudentRepository { get; }

        IAsyncRepository<Author> AuthorRepository { get; }

        Task SaveChangesAsync();
    }
}
