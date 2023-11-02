namespace HouseRentingSystem.Web.Controllers
{
    using HouseRentingSystem.Data.Models;
    using HouseRentingSystem.Web.ViewModels.User;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;

        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userStore = userStore;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser user = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            await this.userManager.SetEmailAsync(user, model.Email);
            await this.userManager.SetUserNameAsync(user, model.Email);

            IdentityResult result = await this.userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }

                return View(model);
            }

            await this.signInManager.SignInAsync(user, false);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
