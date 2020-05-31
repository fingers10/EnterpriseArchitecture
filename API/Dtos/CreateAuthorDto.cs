using Fingers10.EnterpriseArchitecture.ApplicationCore.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fingers10.EnterpriseArchitecture.API.Dtos
{
    public class CreateAuthorDto
    {
        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string MainCategory { get; set; }

        public IEnumerable<CreateBookDto> Books { get; set; } = new List<CreateBookDto>();
    }
}
