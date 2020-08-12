using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors
{
    public class Name : ValueObject
    {
        public string First { get; }
        public string Last { get; }

        protected Name()
        {
        }

        private Name(string first, string last) : this()
        {
            First = first;
            Last = last;
        }

        public static Result<Name> Create(string firstName, string lastName)
        {
            var firstNameResult =
                Result.SuccessIf(!string.IsNullOrWhiteSpace(firstName), "First name should not be empty")
                         .Map(() => firstName.Trim())
                         .Ensure(firstName => !string.IsNullOrEmpty(firstName), "First name should not be empty")
                         .Ensure(firstName => firstName.Length <= 50, "First name is too long");

            var lastNameResult =
                Result.SuccessIf(!string.IsNullOrWhiteSpace(lastName), "Last name should not be empty")
                         .Map(() => lastName.Trim())
                         .Ensure(lastName => !string.IsNullOrEmpty(lastName), "Last name should not be empty")
                         .Ensure(lastName => lastName.Length <= 50, "Last name is too long");

            return Result.Combine(firstNameResult, lastNameResult)
                         .Finally(result => result.IsSuccess ?
                                    new Name(firstNameResult.Value, lastNameResult.Value) :
                                    Result.Failure<Name>(result.Error));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return First;
            yield return Last;
        }
    }
}
