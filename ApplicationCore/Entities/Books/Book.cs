using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System;

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

        public Title Title { get; private set; }

        public Description Description { get; private set; }

        public virtual Author Author { get; private set; }

        public void EditBook(Title title, Description description, Author author)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Author = author ?? throw new ArgumentNullException(nameof(author));
        }
    }
}
