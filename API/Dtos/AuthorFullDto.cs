using System;

namespace Fingers10.EnterpriseArchitecture.API.Dtos
{
    public class AuthorFullDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string MainCategory { get; set; }
    }
}
