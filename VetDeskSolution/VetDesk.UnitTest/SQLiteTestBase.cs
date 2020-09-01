using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using VetDesk.Entity;

namespace VetDesk.UnitTest
{
    public abstract class SQLiteTestBase : IDisposable
    {
            private const string InMemoryConnectionString = "DataSource=:memory:";
            private readonly SqliteConnection dbConnection;

            protected readonly VetDeskContext context;

            protected SQLiteTestBase()
            {
                dbConnection = new SqliteConnection(InMemoryConnectionString);
                dbConnection.Open();
                var options = new DbContextOptionsBuilder<VetDeskContext>()
                        .UseSqlite(dbConnection)
                        .Options;
                context = new VetDeskContext(options);
                context.Database.EnsureCreated();
            }

            public void Dispose()
            {
                dbConnection.Close();
            }
        }
}
