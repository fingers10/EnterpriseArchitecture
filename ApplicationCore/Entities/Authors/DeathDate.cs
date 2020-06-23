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

        public static Result<DeathDate> Create(DateTimeOffset? dateTime)
        {
            if (dateTime.HasValue && dateTime.Value.Date > DateTimeOffset.Now.Date)
                return Result.Failure<DeathDate>("Death Date should not be future date");

            return Result.Success(new DeathDate(dateTime));
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