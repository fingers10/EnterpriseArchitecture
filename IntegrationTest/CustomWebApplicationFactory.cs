using Fingers10.EnterpriseArchitecture.API.IntegrationTest.Fakes;
using Fingers10.EnterpriseArchitecture.API.IntegrationTest.Helpers;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Fingers10.EnterpriseArchitecture.API.IntegrationTest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where
        TStartup : class
    {
        //public FakeDatabase FakeDatabase { get; }

        public CustomWebApplicationFactory()
        {
            //FakeDatabase = FakeDatabase.WithDefaultProducts();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //var descriptor = services.SingleOrDefault(
                //    d => d.ServiceType ==
                //        typeof(Fingers10Context));

                //if (descriptor != null)
                //{
                //    services.Remove(descriptor);
                //}

                //descriptor = services.SingleOrDefault(
                //    d => d.ServiceType ==
                //        typeof(StudentRepository));

                //if (descriptor != null)
                //{
                //    services.Remove(descriptor);
                //}

                //var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
                //var commandsConnectionString = new CommandsConnectionString(connectionStringBuilder.ToString());
                //services.AddSingleton<CommandsConnectionString>(commandsConnectionString);

                var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };
                var connection = new SqliteConnection(connectionStringBuilder.ToString());

                var dbContextOptions = new DbContextOptionsBuilder<FakeDatabase>()
                .UseSqlite(connection)
                .Options;

                services.AddSingleton<DbContextOptions>(dbContextOptions);

                //services.AddTransient<FakeDatabase>(options => new FakeDatabase(
                //    options.GetRequiredService<ConsoleLogging>(),
                //    options.GetRequiredService<EventDispatcher>(),
                //    options.GetRequiredService<IHttpContextAccessor>(),
                //    dbContextOptions
                //    ));
                services.AddTransient<FakeDatabase>();
                services.AddTransient<IUnitOfWork, FakeUnitOfWork>();

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<FakeDatabase>();
                var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                db.Database.OpenConnection();
                db.Database.EnsureCreated();

                try
                {
                    DatabaseHelper.InitialiseDbForTests(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                                        "database with test data. Error: {Message}", ex.Message);
                }
            });
        }
    }
}
