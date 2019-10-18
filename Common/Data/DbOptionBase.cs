using Common.Data;
using Common.Data.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Data
{
    public abstract class DbOptionBase
    {
        protected readonly ItemTodo _firstItem = new ItemTodo() { Id = Guid.Parse("965468a8-9c0e-4ebd-9538-4987eb0c995d"), Name = "First Item" };
        protected readonly ItemTodo _secondItem = new ItemTodo() { Id = Guid.Parse("217303d1-26ae-4a1a-aaca-7b920a7cbaaa"), Name = "Second Item" };
        protected ItemTodo[] _items;

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
