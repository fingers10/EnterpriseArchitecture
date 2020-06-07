namespace Fingers10.EnterpriseArchitecture.API.Dtos
{
    public class BookDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public long AuthorId { get; set; }
    }
}
