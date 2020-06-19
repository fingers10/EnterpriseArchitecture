using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces
{
    public interface IAuthorReadonlyRepository
    {
        Task<List<Author>> GetAllAsync(string mainCategory);
        Task<Author> GetAuthorAsync(long id);
    }
}
