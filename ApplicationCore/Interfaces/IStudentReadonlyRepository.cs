using Fingers10.EnterpriseArchitecture.ApplicationCore.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces
{
    public interface IStudentReadonlyRepository
    {
        Task<List<StudentDto>> GetAllAsync();
    }
}
