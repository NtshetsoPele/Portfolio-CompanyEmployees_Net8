using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Repository;

namespace CompanyEmployees.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                (SqlServerDbContextOptionsBuilder b) => b.MigrationsAssembly("CompanyEmployees"));
        
        return new RepositoryContext(builder.Options);
    }
}