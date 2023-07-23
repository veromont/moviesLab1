using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using moviesAPI.Areas.Identity.Data;
using moviesAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace moviesAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        private readonly string DEFAULT_ROUTE_CONTROLLER = "Home";
        private readonly string DEFAULT_ROUTE_ACTION = "Index";

        public AccountController(
            SignInManager<User> signInManager, 
            UserManager<User> userManager, 
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = fillUserInfo(user);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileInputModel profileInput)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                var model = fillUserInfo(user);
                return View(model);
            }
            return View();
        }
        [NonAction]
        private ProfileModel fillUserInfo(User user)
        {
            var model = new ProfileModel();
            model.Username = user.UserName;
            model.Input.PhoneNumber = user.PhoneNumber;
            model.Input.Name = user.Name;
            model.Input.Surname = user.Surname;
            return model;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { 
                    Email = model.Email, 
                    UserName = model.UserName, 
                    Name = model.Name, 
                    Surname = model.Surname };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                        $"Підтвердіть реєстрацію за посиланням: <a href='{callbackUrl}'>link</a>");

                    return Content("Для завершення реєстрації перевірте електронну пошту та перейдіть за посиланням, вказаним у листі");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // перевіряємо, чи належить URL додатку
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction(DEFAULT_ROUTE_ACTION, DEFAULT_ROUTE_CONTROLLER);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неправильний логін чи(та) пароль");
                }
            }
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(DEFAULT_ROUTE_ACTION, DEFAULT_ROUTE_CONTROLLER);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return Content("Помилка");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Content("Помилка");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return Content("Успішно підтверджено");
            else
                return Content("Помилка");
        }
    }
}
