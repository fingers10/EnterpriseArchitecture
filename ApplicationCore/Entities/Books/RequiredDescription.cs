using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books
{
    public class RequiredDescription : Description
    {
        public static Result<Description> Create(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<Description>("Description is required");

            return Description.Create(description);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}