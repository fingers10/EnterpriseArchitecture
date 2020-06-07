using Fingers10.EnterpriseArchitecture.ApplicationCore.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces
{
    public interface IAuthorReadonlyRepository
    {
        Task<List<AuthorDto>> GetAllAsync(string mainCategory);
        Task<AuthorDto> GetAuthorAsync(long id);
    }
}
