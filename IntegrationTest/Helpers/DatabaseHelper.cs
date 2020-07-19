using Fingers10.EnterpriseArchitecture.API.IntegrationTest.Fakes;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Authors;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fingers10.EnterpriseArchitecture.API.IntegrationTest.Helpers
{
    public static class DatabaseHelper
    {
        public static async void InitialiseDbForTests(FakeDatabase dbContext)
        {
            await dbContext.Authors.AddAsync(
                    new Author(
                        Name.Create("Abdul Rahman", "Shabeek Mohamed").Value,
                        BirthDate.Create(new DateTime(1993, 2, 10)).Value,
                        null,
                        MainCategory.Create("SciFi").Value));

            await dbContext.SaveChangesAsync();
        }

        public static async void ResetDbForTests(FakeDatabase dbContext)
        {
            var authors = await dbContext.Authors.ToArrayAsync();

            dbContext.Authors.RemoveRange(authors);

            await dbContext.SaveChangesAsync();

            InitialiseDbForTests(dbContext);
        }
    }
}
