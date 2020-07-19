using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.API.IntegrationTest.Fakes
{
    public class FakeUnitOfWork : IUnitOfWork
    {
        private readonly FakeDatabase _context;
        private IAsyncRepository<Student> _studentRepository;
        private IAsyncRepository<Author> _authorRepository;
        private IAsyncRepository<Book> _bookRepository;

        public FakeUnitOfWork(FakeDatabase context)
        {
            _context = context;
        }

        public IAsyncRepository<Student> StudentRepository
        {
            get
            {
                return _studentRepository ??= new FakeStudentRepository(_context);
            }
        }

        public IAsyncRepository<Author> AuthorRepository
        {
            get
            {
                return _authorRepository ??= new FakeAuthorRepository(_context);
            }
        }

        public IAsyncRepository<Book> BookRepository
        {
            get
            {
                return _bookRepository ??= new FakeBookRepository(_context);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
