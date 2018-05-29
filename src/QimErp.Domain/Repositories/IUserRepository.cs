using System.Threading.Tasks;
using Qim.Domain.Repositories;
using QimErp.Domain.Entity;

namespace QimErp.Domain.Repositories
{
    public interface IUserRepository : IRepository<User, string>
    {

        Task<User> GetUserByCellPhoneOrEmail(string cellPhoneorEmail);
    }
}