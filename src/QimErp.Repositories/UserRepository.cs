using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qim.EntitiFrameworkCore;
using QimErp.Domain;
using QimErp.Domain.Entity;
using QimErp.Domain.Repositories;

namespace QimErp.Repositories
{
    public class UserRepository : EfCoreRepository<User,string>,IUserRepository
    {
        public UserRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<User>  GetUserByCellPhoneOrEmail(string cellPhoneorEmail)
        {
            return
                await
                    GetTable().Where(a => a.CellPhone == cellPhoneorEmail || a.Email == cellPhoneorEmail)
                        .FirstOrDefaultAsync();
        }
    }
}