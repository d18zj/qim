using System.Linq;
using System.Threading.Tasks;
using Qim;
using Qim.Domain.Repositories;
using Qim.Domain.Uow;
using QimErp.Domain.Entity;
using QimErp.Domain.Repositories;
using QimErp.Infrastructure.DomainModel;

namespace QimErp.Domain.Services.Impl
{
    public class UserManager : IUserManager
    {
        private readonly IRepository<DatabaseInfo, int> _databaseRepository;
        private readonly IGlobalAccountRepository _globalAccountRepository;
        private readonly IRepository<Tenant, int> _tenantRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository, IGlobalAccountRepository globalAccountRepository,
            IRepository<Tenant, int> tenantRepository,
            IRepository<DatabaseInfo, int> databaseRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _userRepository = userRepository;
            _globalAccountRepository = globalAccountRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _tenantRepository = tenantRepository;
            _databaseRepository = databaseRepository;
        }

        public async Task RegisterUser(string tenantName, string cellPhone, string userName)
        {
            Ensure.NotNullOrEmpty(tenantName, nameof(tenantName));
            Ensure.NotNullOrEmpty(cellPhone, nameof(cellPhone));
            Ensure.NotNullOrEmpty(userName, nameof(userName));


            var database = await _databaseRepository.FirstOrDefaultAsyncByFunc(q => q.OrderByDescending(e => e.PId));
            if (database == null)
            {
                throw new AppException($"未找到可用的数据库！");
            }

            //检查租户是否已存在
            var tenant = await _tenantRepository.FirstOrDefaultAsync(a => a.TenantName == tenantName);
            if (tenant != null)
            {
                throw new AppException($"公司名称:{tenantName}已注册");
            }

            tenant = Tenant.Create(tenantName, database.PId, cellPhone);
            var tenantId = await _tenantRepository.InsertAndGetIdAsync(tenant);

            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var user = User.Register(cellPhone, userName);
                var globalAccount = GlobalAccount.Create(tenantId, user.PId, user.CellPhone, user.Email);
                _userRepository.Insert(user);
                _globalAccountRepository.Insert(globalAccount);
            }
        }

        public async Task<LoginOutput> Login(string cellPhoneOrEmail, string password)
        {
            var output = new LoginOutput { LoginResult = LoginResult.InvalidUserAccountOrEmail };
            var tenant = await _globalAccountRepository.GetTenantByCellPhoneOrEmail(cellPhoneOrEmail);
            if (tenant == null)
            {
                return output;
            }
            using (_unitOfWorkManager.Current.SetTenantId(tenant.PId))
            {
                var user =
                    await _userRepository.GetUserByCellPhoneOrEmail(cellPhoneOrEmail);
                if (user == null) return output;

                user.UserLogin(password, output);
                if (output.LoginResult == LoginResult.Success)
                {
                    tenant.CheckCanLogin(output);
                }
            }
            return output;
        }
    }
}