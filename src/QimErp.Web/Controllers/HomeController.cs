using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QimErp.ServiceContracts;
using QimErp.ServiceContracts.Dto;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace QimErp.Web.Controllers
{
    
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var list = await _userService.GetAllUser();

            return View(list);
        }

        //public IActionResult Add()
        //{
        //    return View();
        //}

       
        //public async Task<IActionResult> Register(RegisterUserInput input)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _userService.RegisterUser(input);
        //        return RedirectToAction("Index");
        //    }
        //    return View(input);
        //}
    }
}