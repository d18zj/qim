using System.Threading.Tasks;
using QimErp.Domain.Entity;
using QimErp.Infrastructure.DomainModel;

namespace QimErp.Domain.Services
{
    public interface IUserManager
    {

        Task RegisterUser(string tenantName, string cellPhone, string userName);

        Task<LoginOutput> Login(string cellPhoneOrEmail, string password);
    }
}