using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors
{
    public class MainCategory : ValueObject
    {
        protected MainCategory()
        {
        }

        private MainCategory(string value) : this()
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<MainCategory> Create(string mainCategory)
        {
            return Result.SuccessIf(!string.IsNullOrWhiteSpace(mainCategory), "Main Category is required.")
                         .Map(() => mainCategory.Trim())
                         .Ensure(mainCategory => mainCategory.Length <= 50, "Main Category is too long.")
                         .Map(mainCategory => new MainCategory(mainCategory));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(MainCategory mainCategory)
        {
            return mainCategory.Value;
        }
    }
}