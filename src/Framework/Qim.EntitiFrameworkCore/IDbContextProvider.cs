using Qim.MultiTenancy;

namespace Qim.EntitiFrameworkCore
{
    public interface IDbContextProvider
    {
        EfCoreDbContext GetDbContext(MultiTenancySides? multiTenancySides = null);
    }
}