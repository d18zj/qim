using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qim.EntitiFrameworkCore;
using QimErp.Domain.Entity;
using QimErp.Domain.Repositories;

namespace QimErp.Repositories
{
    public class GlobalAccountRepository : EfCoreRepository<GlobalAccount, int>, IGlobalAccountRepository
    {
        public GlobalAccountRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<Tenant> GetTenantByCellPhoneOrEmail(string cellPhoneOrEmail)
        {
            return
                await
                    GetTableNoTracking().Where(a => a.CellPhone == cellPhoneOrEmail || a.Email == cellPhoneOrEmail)
                        .Select(a => a.Tenant)
                        .FirstOrDefaultAsync();
        }
    }
}