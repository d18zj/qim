using Microsoft.EntityFrameworkCore;

namespace Qim.EntitiFrameworkCore.Configuration
{
    public class DbContextConfiguration
    {
        public string ConnectionString { get; }

        public DbContextOptionsBuilder<EfCoreDbContext> Builder { get; }

        public DbContextConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
            Builder = new DbContextOptionsBuilder<EfCoreDbContext>();
        }
    }
}