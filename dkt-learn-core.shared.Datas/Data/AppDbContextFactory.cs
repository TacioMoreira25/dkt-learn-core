using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DKT_Learn.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
            var db   = Environment.GetEnvironmentVariable("DB_NAME");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var pass = Environment.GetEnvironmentVariable("DB_PASSWORD");
            
            var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={pass}";

            var builder = new DbContextOptionsBuilder<AppDbContext>();

            builder.UseNpgsql(connectionString);

            return new AppDbContext(builder.Options);
        }
    }
}