using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.API.Dtos;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Services;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.API.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/students")]
    [ApiController]
    //[Authorize("RequireAdminRole")]
    public class StudentController : BaseController
    {
        private readonly Messages _messages;

        public StudentController(Messages messages)
        {
            _messages = messages;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var list = await _messages.Dispatch(new GetStudentsListQuery());
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterStudent(RegisterStudentDto dto)
        {
            var command = new RegisterStudentCommand(dto.FirstName, dto.LastName, dto.NameSuffixId, dto.Email,
                                                     dto.FavoriteCourseId, dto.FavoriteCourseGrade);

            Result result = await _messages.Dispatch(command);

            return FromResult(result);
        }
    }
}
