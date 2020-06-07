using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces
{
    public interface IUnitOfWork
    {
        IAsyncRepository<Student> StudentRepository { get; }

        IAsyncRepository<Author> AuthorRepository { get; }

        IAsyncRepository<Book> BookRepository { get; }

        Task SaveChangesAsync();
    }
}
