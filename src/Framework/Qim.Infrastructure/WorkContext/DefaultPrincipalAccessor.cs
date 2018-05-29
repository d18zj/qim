using System.Security.Claims;

namespace Qim.WorkContext
{
    public class DefaultPrincipalAccessor : IPrincipalAccessor
    {
        public ClaimsPrincipal Principal => ClaimsPrincipal.Current;
    }
}