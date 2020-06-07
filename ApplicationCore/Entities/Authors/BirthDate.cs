using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors
{
    public class BirthDate : ValueObject
    {
        protected BirthDate()
        {
        }

        private BirthDate(DateTimeOffset value) : this()
        {
            Value = value;
        }

        public DateTimeOffset Value { get; }

        public static Result<BirthDate> Create(DateTimeOffset dateTime)
        {
            if (dateTime.Date > DateTimeOffset.Now.Date)
                return Result.Failure<BirthDate>("Birth Date should not be future date");

            return Result.Success(new BirthDate(dateTime));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator DateTimeOffset(BirthDate birthDate)
        {
            return birthDate.Value;
        }
    }
}