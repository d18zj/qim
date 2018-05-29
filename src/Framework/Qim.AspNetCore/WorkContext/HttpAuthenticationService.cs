using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Qim.WorkContext;
using IAuthenticationService = Qim.WorkContext.IAuthenticationService;

namespace Qim.AspNetCore.WorkContext
{
    internal class HttpAuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly string _authenticationScheme;

        public HttpAuthenticationService(IHttpContextAccessor accessor,IOptions<AuthenticationOptions> options)
        {
            Ensure.NotNull(accessor, nameof(accessor));
            Ensure.NotNull(options, nameof(options));
            // Ensure.NotNullOrWhiteSpace(authScheme, nameof(authScheme));
            _accessor = accessor;
            _authenticationScheme = options.Value.DefaultAuthenticateScheme;
        }

        #region protected

        protected HttpContext HttpContext => _accessor.HttpContext;

        #endregion

        public async Task SignInAsync(string userId, int? tenantId = null, bool isPersistent = false)
        {
            Ensure.NotNullOrEmpty(userId, nameof(userId));

            var identity = new ClaimsIdentity(_authenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            identity.AddClaim(
                new Claim(QimClaimTypes.TENANT_ID, tenantId.HasValue ? tenantId.ToString() : string.Empty));

            //_authenticationService.SignInAsync(HttpContext,_authenticationScheme,identity,)
            await HttpContext.SignInAsync(_authenticationScheme, new ClaimsPrincipal(identity),
                new AuthenticationProperties {IsPersistent = isPersistent});
        }

        public async Task SignOutAsync()
        {
            await HttpContext.SignOutAsync(_authenticationScheme);
        }

        public bool IsSignedIn()
        {
            var principal = HttpContext.User;
            return principal?.Identities != null &&
                   principal.Identities.Any(i => i.AuthenticationType == _authenticationScheme);
        }
    }
}