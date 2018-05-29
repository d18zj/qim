using System;
using Qim.Configuration;
using Qim.MultiTenancy;

namespace Qim.Domain.Uow
{
    public class ConnectionStringResolver : IConnectionStringResolver
    {

        public string GetNameOrConnectionString(MultiTenancySides sides, int? tenantId = null)
        {
            if (sides == MultiTenancySides.Tenant )
            {
                if (tenantId == null)
                {
                    throw new InvalidOperationException();
                }
                return Bootstrapper.Configuration.DefaultNameOrConnectionString.Replace("QimErpHost", "QimErp");
            }

            return Bootstrapper.Configuration.DefaultNameOrConnectionString;
        }
    }
}