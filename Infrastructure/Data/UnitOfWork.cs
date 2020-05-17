using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Fingers10Context _context;
        private IAsyncRepository<Student> _studentRepository;

        public UnitOfWork(Fingers10Context context)
        {
            _context = context;
        }

        public IAsyncRepository<Student> StudentRepository
        {
            get
            {
                return _studentRepository ?? (_studentRepository = new StudentRepository(_context));
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
