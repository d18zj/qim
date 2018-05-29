using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Qim.WorkContext;
using QimErp.Infrastructure.DomainModel;
using QimErp.ServiceContracts;
using QimErp.ServiceContracts.Dto;
using QimErp.Web.Model;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace QimErp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public AccountController(IAuthenticationService authenticationService,
            IUserService userService
            )
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        // GET: /<controller>/
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Login(model.UserAccount, model.Password);
                switch (result.LoginResult)
                {
                    case LoginResult.Success:
                        await _authenticationService.SignInAsync(result.UserAccount, result.TenantId, model.Remember);
                        return Redirect("/Home");
                    case LoginResult.InvalidUserAccountOrEmail:
                    case LoginResult.InvalidPassword:
                        ModelState.AddModelError("", "错误的用户名或密码！");
                        break;
                    case LoginResult.UserIsNotActive:
                        ModelState.AddModelError("", "用户已停用!");
                        break;
                    case LoginResult.TenantIsNotActive:
                        ModelState.AddModelError("", $"公司：{result.TenantName} 已停用！");
                        break;
                    case LoginResult.TenantOutDate:
                        ModelState.AddModelError("", $"公司：{result.TenantName} 的最后截止使用日期：{result.TenantOutDate:yyyy-MM-dd},请尽快续费充值！");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
            return View(model);
        }

        public async Task<IActionResult> Logoff()
        {
            await _authenticationService.SignOutAsync();
            return RedirectToAction("Login");
        }


        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserInput input)
        {
            if (ModelState.IsValid)
            {
                await _userService.RegisterUser(input);
                return RedirectToAction("Login");
            }
            return View(input);
        }
    }
}