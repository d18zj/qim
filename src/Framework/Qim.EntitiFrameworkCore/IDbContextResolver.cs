namespace Qim.EntitiFrameworkCore
{
    public interface IDbContextResolver
    {
        EfCoreDbContext Resolve(string connectionString = null);
    }
}