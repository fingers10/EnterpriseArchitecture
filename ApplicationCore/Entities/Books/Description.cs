using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books
{
    public class Description : ValueObject
    {
        public string Value { get; }

        protected Description()
        {
        }

        private Description(string value) : this()
        {
            Value = value;
        }

        public static Result<Description> Create(string description)
        {
            description = description?.Trim();

            if (description.Length > 1500)
                return Result.Failure<Description>("Description is too long");

            return Result.Success(new Description(description));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}