using System.Threading.Tasks;
using Qim.Domain.Repositories;
using QimErp.Domain.Entity;

namespace QimErp.Domain.Repositories
{
    public interface IGlobalAccountRepository:IRepository<GlobalAccount,int>
    {
        Task<Tenant> GetTenantByCellPhoneOrEmail(string cellPhoneOrEmail);
    }
}