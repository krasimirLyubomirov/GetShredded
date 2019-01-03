namespace GetShredded.Web.Controllers
{
    using Common;
    using Services.Contracts;
    using ViewModel.Input.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

    [Authorize]
    public class UsersController : Controller
    {
        public UsersController(IUserService userService)
        {
            UserService = userService;
        }

        protected IUserService UserService { get; }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginInputModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(loginModel);
            }

            var result = this.UserService.LogUser(loginModel);

            if (result != SignInResult.Success)
            {
                this.ViewData[GlobalConstants.ModelError] = GlobalConstants.LoginError;
                return this.View(loginModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterInputModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(registerModel);
            }

            var result = this.UserService.RegisterUser(registerModel).Result;

            if (result != SignInResult.Success)
            {
                this.ViewData[GlobalConstants.ModelError] = 
                    string.Format(GlobalConstants.UsernameUnique, registerModel.Username);
                return this.View(registerModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            this.UserService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route(GlobalConstants.UserProfileRoute)]
        public IActionResult Profile(string username, bool seeProfile = false)
        {
            bool fullAccess = this.User.Identity.Name == username || this.User.IsInRole(GlobalConstants.Admin);
            var user = this.UserService.GetUser(username);
            
            if (fullAccess && !seeProfile)
            {
                return this.View("Details", user);
            }

            return this.View(user);
        }
    }
}