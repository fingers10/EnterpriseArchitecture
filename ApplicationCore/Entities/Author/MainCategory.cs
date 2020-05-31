using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Author
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
            if (string.IsNullOrWhiteSpace(mainCategory))
                return Result.Failure<MainCategory>("Main Category is required.");

            mainCategory = mainCategory.Trim();

            if (mainCategory.Length > 50)
                return Result.Failure<MainCategory>("Main Category is too long.");

            return Result.Success(new MainCategory(mainCategory));
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