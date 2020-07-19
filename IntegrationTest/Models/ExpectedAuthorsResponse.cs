using Fingers10.EnterpriseArchitecture.API.Dtos;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.API.IntegrationTest.Models
{
    public class ExpectedAuthorsResponse
    {
        public IEnumerable<AuthorDto> Value { get; set; }
    }
}
