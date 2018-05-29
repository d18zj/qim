using System.Security.Claims;

namespace Qim.WorkContext
{
    public interface IPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }
}