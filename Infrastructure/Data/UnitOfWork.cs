using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Author;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Fingers10Context _context;
        private IAsyncRepository<Student> _studentRepository;
        private IAsyncRepository<Author> _authorRepository;

        public UnitOfWork(Fingers10Context context)
        {
            _context = context;
        }

        public IAsyncRepository<Student> StudentRepository
        {
            get
            {
                return _studentRepository ??= new StudentRepository(_context);
            }
        }

        public IAsyncRepository<Author> AuthorRepository
        {
            get
            {
                return _authorRepository ??= new AuthorRepository(_context);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
