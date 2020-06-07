using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books
{
    public class Book : Entity, IAggregateRoot
    {
        protected Book()
        {
        }

        public Book(Title title, Description description, Author author) : this()
        {
            Title = title;
            Description = description;
            Author = author;
        }

        public Title Title { get; }

        public Description Description { get; }

        public virtual Author Author { get; }
    }
}
