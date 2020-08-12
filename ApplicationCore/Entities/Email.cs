using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities
{
    public sealed class Email : ValueObject
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email> Create(string email)
        {
            return Result.SuccessIf(!string.IsNullOrWhiteSpace(email), "Email should not be empty")
                         .Map(() => email.Trim())
                         .Ensure(email => !string.IsNullOrEmpty(email), "Email should not be empty")
                         .Ensure(email => email.Length <= 200, "Email is too long")
                         .Ensure(email => Regex.IsMatch(email, "^(.+)@(.+)$"), "Email is invalid")
                         .Map(email => new Email(email));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(Email email)
        {
            return email.Value;
        }
    }
}
