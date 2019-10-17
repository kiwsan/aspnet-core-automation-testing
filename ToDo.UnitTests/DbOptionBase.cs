using Common.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.UnitTests
{
    public abstract class DbOptionBase
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private const string InMemoryDatabaseConnectionString = "InMemoryDb";

        protected DbContextOptions<AppDbContext> InMemoryWithSqlite()
        {
            SqliteConnection sqlite = new SqliteConnection(InMemoryConnectionString);
            sqlite.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(sqlite)
                .Options;

            return options;
        }

        protected DbContextOptions<AppDbContext> InMemoryWithEfDatabase()
        {
            // Create a new service provider to create a new in-memory database.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance using an in-memory database and 
            // IServiceProvider that the context should resolve all of its 
            // services from.
            var builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(InMemoryDatabaseConnectionString)
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

    }
}
