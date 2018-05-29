using Qim;
using Qim.Domain.Entity;
using Qim.MultiTenancy;
using Qim.Timing;
using QimErp.Infrastructure;
using QimErp.Infrastructure.DomainModel;

namespace QimErp.Domain.Entity
{
    /// <summary>
    ///  租户
    /// </summary>
    public class Tenant : BaseTenant
    {
        /// <summary>
        ///     数据库Id
        /// </summary>
        public int DatabaseId { get; set; }

        #region 导航属性


        /// <summary>
        ///     数据库
        /// </summary>
        public virtual DatabaseInfo DatabaseInfo { get; set; }
        #endregion

        #region Method

        public void CheckCanLogin(LoginOutput output)
        {
            Ensure.NotNull(output, nameof(output));
            output.TenantId = PId;
            output.TenantName = TenantName;
            output.TenantOutDate = EndTime;

            if (!IsActive)
            {
                output.LoginResult = LoginResult.TenantIsNotActive;
                return;
            }
            if (Clock.Now > EndTime)
            {
                output.LoginResult = LoginResult.TenantOutDate;
            }

        }


        public static Tenant Create(string tenantName, int databaseId, string userIdOrCellPhone)
        {
            return new Tenant
            {
                TenantName = tenantName,
                DatabaseId = databaseId,
                StartTime = Clock.Now,
                EndTime = Clock.Now.AddDays(30),
                CreateBy = userIdOrCellPhone,
                IsActive = true
            };
        }

        #endregion

    }
}