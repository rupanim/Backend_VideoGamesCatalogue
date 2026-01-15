using System.Data.Common;
using Backend_VideoGamesCatalogue.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Backend_VideoGamesCatalogue.Tests
{
    public class TestAppFactory : WebApplicationFactory<Program>
    {
        private DbConnection? _conn;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.UseContentRoot(AppContext.BaseDirectory)
                .ConfigureServices(services =>
            {
                // remove SQL Express DB context to use SQLite
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDBContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // SQLite conn creation
                _conn = new SqliteConnection("DataSource=:memory:");
                _conn.Open();

                services.AddDbContext<AppDBContext>(opt => opt.UseSqlite(_conn!));
                
                // ensure that the schema exists
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();
                db.Database.EnsureCreated();
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _conn?.Dispose();
        }
    }
}
