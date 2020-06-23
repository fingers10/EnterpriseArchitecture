using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors
{
    /// <summary>
    /// Author Domain Entity
    /// </summary>
    public class Author : Entity, IAggregateRoot
    {
        private readonly List<Book> _books = new List<Book>();
        public virtual IReadOnlyList<Book> Books => _books.ToList();

        protected Author()
        {
        }

        public Author(Name name, BirthDate dateOfBirth, DeathDate dateOfDeath, MainCategory mainCategory) : this()
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            DateOfDeath = dateOfDeath;
            MainCategory = mainCategory;
        }

        public virtual Name Name { get; private set; }

        public BirthDate DateOfBirth { get; private set; }

        public DeathDate DateOfDeath { get; private set; }

        public MainCategory MainCategory { get; private set; }

        public void AddBooks(List<Book> books)
        {
            _books.AddRange(books);
        }
    }
}
