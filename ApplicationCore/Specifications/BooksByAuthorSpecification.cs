using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Specifications
{
    public sealed class BooksByAuthorSpecification : BaseSpecification<Book>
    {
        public BooksByAuthorSpecification(long authorId) : base(x => x.Author.Id == authorId)
        {
            AddInclude(x => x.Author);
        }

        public BooksByAuthorSpecification(long authorId, int pageNumber, int pageSize) : base(x => x.Author.Id == authorId)
        {
            AddInclude(x => x.Author);
            ApplyPaging((pageNumber -1) * pageSize, pageSize);
        }

        public BooksByAuthorSpecification(long authorId, long bookId) : base(x => x.Id == bookId && x.Author.Id == authorId)
        {
            AddInclude(x => x.Author);
        }
    }
}
