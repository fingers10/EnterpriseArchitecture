using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Books
{
    public class RequiredDescription : Description
    {
        public static Result<Description> Create(string description)
        {
            return Result.SuccessIf(!string.IsNullOrWhiteSpace(description), "Description is required.")
                         .Finally(result => result.IsSuccess ?
                                    Description.Create(description) :
                                    Result.Failure<Description>(result.Error));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}