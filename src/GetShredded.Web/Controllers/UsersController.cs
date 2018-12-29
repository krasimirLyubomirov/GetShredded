namespace GetShredded.Web.Controllers
{
    using Common;
    using Services.Contracts;
    using ViewModel.Input.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

    public class UsersController : Controller
    {
        public UsersController(IUserService userService)
        {
            this.UserService = userService;
        }

        protected IUserService UserService { get; }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginInputModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var result = UserService.LogUser(loginModel);

            if (result != SignInResult.Success)
            {
                ViewData[GlobalConstants.ModelError] = GlobalConstants.LoginError;
                return View(loginModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterInputModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }
            var result = UserService.RegisterUser(registerModel).Result;
            if (result != SignInResult.Success)
            {
                ViewData[GlobalConstants.ModelError] = 
                    string.Format(GlobalConstants.UsernameUnique, registerModel.Username);
                return View(registerModel);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            UserService.Logout();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route(GlobalConstants.UserProfileRoute)]
        public IActionResult Profile(string username)
        {
            bool fullAccess = User.Identity.Name == username || User.IsInRole(GlobalConstants.Admin);
            var user = UserService.GetUser(username);
            
            if (fullAccess)
            {
                return View("Details", user);
            }

            return View(user);
        }
    }
}