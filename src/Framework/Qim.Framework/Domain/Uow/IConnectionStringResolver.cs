using Qim.MultiTenancy;

namespace Qim.Domain.Uow
{
    public interface IConnectionStringResolver
    {
        string GetNameOrConnectionString(MultiTenancySides sides, int? tenantId = null);
    }
}