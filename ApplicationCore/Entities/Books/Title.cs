using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books
{
    public class Title : ValueObject
    {
        public string Value { get; }

        protected Title()
        {
        }

        private Title(string value) : this()
        {
            Value = value;
        }

        public static Result<Title> Create(string title)
        {
            return Result.SuccessIf(!string.IsNullOrWhiteSpace(title), "Title should not be empty.")
                         .Map(() => title.Trim())
                         .Ensure(title => title.Length <= 100, "Title is too long.")
                         .Map(title => new Title(title));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(Title title)
        {
            return title.Value;
        }
    }
}