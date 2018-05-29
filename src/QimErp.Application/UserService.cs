using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Qim;
using Qim.Application;
using Qim.Domain.Uow;
using QimErp.Domain.Entity;
using QimErp.Domain.Repositories;
using QimErp.Domain.Services;
using QimErp.Infrastructure.DomainModel;
using QimErp.ServiceContracts;
using QimErp.ServiceContracts.Dto;

namespace QimErp.Application
{
    public class UserService : BaseApplicationService, IUserService
    {
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, IUnitOfWorkManager manager,
            IUserManager userManager) : base(manager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public virtual async Task RegisterUser(RegisterUserInput input)
        {
            Ensure.NotNull(input, nameof(input));

            await _userManager.RegisterUser(input.TenantName, input.CellPhone, input.UserName);
        }


        public virtual Task<List<UserListDto>> GetAllUser()
        {

            var list =
                _userRepository.GetTableNoTracking().ProjectTo<UserListDto>().ToList();
            return Task.FromResult(list);

        }

        public async Task<LoginOutput> Login(string cellPhoneOrEmail, string password)
        {
            Ensure.NotNullOrWhiteSpace(cellPhoneOrEmail, nameof(cellPhoneOrEmail));
            Ensure.NotNullOrWhiteSpace(password, nameof(password));

            return await _userManager.Login(cellPhoneOrEmail, password);
        }


        public virtual async Task AddUser(AddUserInput input)
        {
            Ensure.NotNull(input, nameof(input));
            var user = User.Create(input.UserAccount, input.UserName, input.Password,  input.Email,
                input.CellPhone,
                input.Description);
            using (var work = CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                _userRepository.Insert(user);

                await CurrentUnitOfWork.CommitAsync();
            }
        }
    }
}