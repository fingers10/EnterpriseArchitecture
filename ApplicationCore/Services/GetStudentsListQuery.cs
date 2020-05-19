using Fingers10.EnterpriseArchitecture.ApplicationCore.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Services
{
    public sealed class GetStudentsListQuery : IQuery<List<StudentDto>>
    {
        internal sealed class GetListQueryHandler : IQueryHandler<GetStudentsListQuery, List<StudentDto>>
        {
            private readonly IStudentReadonlyRepository _studentRepository;

            public GetListQueryHandler(IStudentReadonlyRepository studentRepository)
            {
                _studentRepository = studentRepository;
            }

            public async Task<List<StudentDto>> Handle(GetStudentsListQuery query)
            {
                return await _studentRepository.GetAllAsync();
            }
        }
    }
}
