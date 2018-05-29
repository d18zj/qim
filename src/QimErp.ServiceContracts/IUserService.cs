using System.Collections.Generic;
using System.Threading.Tasks;
using Qim;
using QimErp.Infrastructure.DomainModel;
using QimErp.ServiceContracts.Dto;

namespace QimErp.ServiceContracts
{
    public interface IUserService : IApplicationService
    {

        //Task AddUser(AddUserInput input);

        Task RegisterUser(RegisterUserInput input);

        Task<List<UserListDto>> GetAllUser();

        Task<LoginOutput> Login(string userAccountOrCellPhoneOrEmail, string password);
    }
}
