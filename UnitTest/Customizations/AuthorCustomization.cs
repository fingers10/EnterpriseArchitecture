using AutoFixture;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using System;

namespace UnitTest.Customizations
{
    public class AuthorCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new CurrentDateTimeGenerator());
            fixture.Register(() => Name.Create(fixture.Create<string>(), fixture.Create<string>()).Value);
            fixture.Register(() => BirthDate.Create(fixture.Create<DateTimeOffset>().AddDays(-fixture.Create<int>())).Value);
            fixture.Register(() => DeathDate.Create(fixture.Create<DateTimeOffset>().AddDays(-fixture.Create<int>())).Value);
            fixture.Register(() => MainCategory.Create(fixture.Create<string>()).Value);
        }
    }
}
