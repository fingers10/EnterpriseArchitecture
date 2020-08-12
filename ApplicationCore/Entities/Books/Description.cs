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
            return Result.SuccessIf(true, description)
                         .Map(() => description?.Trim())
                         .Ensure(description => description is null || description.Length <= 1500, "Description is too long.")
                         .Map(description => new Description(description));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}