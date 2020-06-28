using System;

namespace Fingers10.EnterpriseArchitecture.API.Dtos
{
    public class BookWithConcatenatedAuthorNameDto
    {
        public long Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public long AuthorId { get; set; }
    }
}
