using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Entities.Author;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.Infrastructure.Data
{
    public class Fingers10Context : DbContext
    {
        private static readonly Type[] _enumerationTypes = { typeof(Course), typeof(Suffix) };

        private readonly string _connectionString;
        private readonly bool _useConsoleLogger;
        private readonly EventDispatcher _eventDispatcher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        public Fingers10Context(CommandsConnectionString connectionString, ConsoleLogging consoleLogging, EventDispatcher eventDispatcher, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = connectionString?.Value ?? throw new ArgumentNullException(nameof(connectionString));
            _useConsoleLogger = consoleLogging?.Enable ?? throw new ArgumentNullException(nameof(consoleLogging));
            _eventDispatcher = eventDispatcher;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                    .AddDebug();
            });

            optionsBuilder
                .UseSqlServer(_connectionString);

            if (_useConsoleLogger)
            {
                optionsBuilder?
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder?.ApplyConfigurationsFromAssembly(typeof(Fingers10Context).Assembly);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry> enumerationEntries = ChangeTracker.Entries()
                .Where(x => _enumerationTypes.Contains(x.Entity.GetType()));

            foreach (EntityEntry enumerationEntry in enumerationEntries)
            {
                enumerationEntry.State = EntityState.Unchanged;
            }

            //ChangeTracker.DetectChanges();

            //var timestamp = DateTimeOffset.Now;
            //var user = GetCurrentUser();
            //var entityEntries = ChangeTracker
            //                    .Entries()
            //                    .Where(e => e.Entity is Entity
            //                                && (e.State.Equals(EntityState.Added) || e.State.Equals(EntityState.Modified) || e.State.Equals(EntityState.Deleted))
            //                                && !e.Metadata.IsOwned());

            //foreach (var entry in entityEntries)
            //{
            //    entry.Property("ModifiedAt").CurrentValue = timestamp;
            //    entry.Property("ModifiedBy").CurrentValue = user;

            //    if (entry.State.Equals(EntityState.Added))
            //    {
            //        entry.Property("CreatedAt").CurrentValue = timestamp;
            //        entry.Property("CreatedBy").CurrentValue = user;
            //        //entry.Property("Active").CurrentValue = true;
            //    }
            //}

            List<Entity> entities = ChangeTracker
                .Entries()
                .Where(x => x.Entity is Entity)
                .Select(x => (Entity)x.Entity)
                .ToList();

            var result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            foreach (Entity entity in entities)
            {
                _eventDispatcher.Dispatch(entity.DomainEvents);
                entity.ClearDomainEvents();
            }

            return result;
        }

        private string GetCurrentUser()
        {
            // master
            var userId = Guid.Empty.ToString();

            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext != null)
            {
                // If it returns null, even when the user was authenticated, you may try to get the value of a specific claim 
                userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? _httpContextAccessor.HttpContext.User.FindFirst("sub")?.Value
                      ?? userId;

                // TODO use name to set the shadow property value like in the following post: https://www.meziantou.net/2017/07/03/entity-framework-core-generate-tracking-columns
            }

            return userId; // TODO implement your own logic

            // If you are using ASP.NET Core, you should look at this answer on StackOverflow
            // https://stackoverflow.com/a/48554738/2996339
        }
    }
}
