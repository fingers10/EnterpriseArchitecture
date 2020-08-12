using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors
{
    public class DeathDate : ValueObject
    {
        protected DeathDate()
        {
        }

        private DeathDate(DateTimeOffset? value) : this()
        {
            Value = value;
        }

        public DateTimeOffset? Value { get; }

        public static Result<DeathDate> Create(DateTimeOffset? deathDate)
        {
            return Result.SuccessIf(deathDate.HasValue, "Death Date should not be null.")
                         .Ensure(() => deathDate.Value.Date < DateTimeOffset.Now.Date, "Death Date should not be future date.")
                         .Map(() => new DeathDate(deathDate));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator DateTimeOffset?(DeathDate deathDate)
        {
            return deathDate.Value;
        }
    }
}