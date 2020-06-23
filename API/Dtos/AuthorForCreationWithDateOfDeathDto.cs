using System;

namespace Fingers10.EnterpriseArchitecture.API.Dtos
{
    public class AuthorForCreationWithDateOfDeathDto : CreateAuthorDto
    {
        public DateTimeOffset DateOfDeath { get; set; }
    }
}
