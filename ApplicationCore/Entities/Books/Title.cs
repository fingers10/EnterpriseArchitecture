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
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<Title>("Title should not be empty");

            title = title.Trim();

            if (title.Length > 100)
                return Result.Failure<Title>("Title is too long");

            return Result.Success(new Title(title));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}