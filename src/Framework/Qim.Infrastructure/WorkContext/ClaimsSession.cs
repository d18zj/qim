using System;
using System.Linq;
using System.Security.Claims;

namespace Qim.WorkContext
{
    public class ClaimsSession : IQimSession
    {
        private readonly IPrincipalAccessor _accessor;
        public ClaimsSession(IPrincipalAccessor accessor)
        {
            _accessor = accessor;
        }

        public virtual string UserId
        {
            get
            {
                var userIdClaim = Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                return userIdClaim?.Value;
            }
        }

        public virtual int? TenantId
        {
            get
            {
                var tenantIdClaim = Principal?.Claims.FirstOrDefault(c => c.Type == QimClaimTypes.TENANT_ID);
                if (string.IsNullOrEmpty(tenantIdClaim?.Value))
                {
                    return null;
                }

                return Convert.ToInt32(tenantIdClaim.Value);
            }
        }

        public virtual string CultureName
        {
            get
            {
#if NET451
                string cluterName = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
#else
                string cluterName = System.Globalization.CultureInfo.CurrentCulture.Name;
#endif
                return cluterName;
            }
        }

        protected virtual ClaimsPrincipal Principal => _accessor.Principal;
    }
}