using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using Microsoft.EntityFrameworkCore;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Specifications
{
    public sealed class BooksByAuthorSpecification : BaseSpecification<Book>
    {
        public BooksByAuthorSpecification(long authorId, string searchTitle) 
            : base(x => x.Author.Id == authorId && EF.Functions.Like(x.Title, $"%{searchTitle}%"))
        {
            AddInclude(x => x.Author);
        }

        public BooksByAuthorSpecification(long authorId, string searchTitle, int pageNumber, int pageSize) 
            : base(x => x.Author.Id == authorId && EF.Functions.Like(x.Title, $"%{searchTitle}%"))
        {
            AddInclude(x => x.Author);
            ApplyPaging((pageNumber - 1) * pageSize, pageSize);
        }

        public BooksByAuthorSpecification(long authorId, long bookId) : base(x => x.Id == bookId && x.Author.Id == authorId)
        {
            AddInclude(x => x.Author);
        }
    }
}
