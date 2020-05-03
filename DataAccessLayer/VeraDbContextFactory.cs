using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccessLayer
{
    public class VeraDbContextFactory : IDesignTimeDbContextFactory<VeraDbContext>
    {
        public VeraDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VeraDbContext>();
            optionsBuilder.UseSqlServer("Data Source=vera-sql-sever.database.windows.net;Initial Catalog=vera;User ID=admininstrator;password=AdminPa$$_123;MultipleActiveResultSets=True;Timeout=300;");

            return new VeraDbContext(optionsBuilder.Options);
        }
    }
}
