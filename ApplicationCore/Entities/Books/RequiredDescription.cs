using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books
{
    public class RequiredDescription : Description
    {
        public static Result<Description> Create(string description)
        {
            return Result.SuccessIf(!string.IsNullOrWhiteSpace(description), "Description is required")
                         .Map(() => Description.Create(description)).Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}