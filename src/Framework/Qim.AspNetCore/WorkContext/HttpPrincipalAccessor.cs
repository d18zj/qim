using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Qim.WorkContext;

namespace Qim.AspNetCore.WorkContext
{
    internal class HttpPrincipalAccessor : IPrincipalAccessor
    {
        private readonly IHttpContextAccessor _accessor;

        public HttpPrincipalAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public ClaimsPrincipal Principal => _accessor.HttpContext.User;
    }
}