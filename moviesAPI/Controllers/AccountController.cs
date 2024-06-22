using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using moviesAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using moviesAPI.Repositories;
using moviesAPI.Models.EntityModels;

namespace moviesAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly GenericCinemaRepository _repository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        private readonly string DEFAULT_ROUTE_CONTROLLER = "Home";
        private readonly string DEFAULT_ROUTE_ACTION = "Index";

        public AccountController(
            GenericCinemaRepository repository,
            SignInManager<User> signInManager, 
            UserManager<User> userManager, 
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = await createProfileModelFromUser(user);
            var genres = await _repository.GetAll<Genre>();

            ViewBag.Genres = genres;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageFile.CopyTo(memoryStream);
                    user.Image = memoryStream.ToArray();
                }
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction(DEFAULT_ROUTE_ACTION, DEFAULT_ROUTE_CONTROLLER);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            model.UpdateUser(user);

            var selectedGenres = Request.Form["selectedGenres"];
            handleGenreSelection(user, selectedGenres.ToList());

            await _userManager.UpdateAsync(user);
            return RedirectToAction(DEFAULT_ROUTE_ACTION, DEFAULT_ROUTE_CONTROLLER);
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Неправильний логін чи(та) пароль");
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction(DEFAULT_ROUTE_ACTION, DEFAULT_ROUTE_CONTROLLER);
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
            {
                return Content("Успішно підтверджено");
            }

            return Content("Помилка");
        }

        [NonAction]
        private async Task<IEnumerable<UserGenreConnection>> getRelevantUserGenreConnections(string username)
        {
            var userGenreConnections = await _repository.GetAll<UserGenreConnection>();
            return userGenreConnections.Where(connection => connection.Username == username) ?? new List<UserGenreConnection>();
        }

        [NonAction]
        private async Task<IEnumerable<Genre>> getSelectedGenres(string username)
        {
            var relevantUserGenreConnections = await getRelevantUserGenreConnections(username);
            var selectedGenreIds = relevantUserGenreConnections.Select(c => c.GenreId);

            var selectedGenres = new List<Genre>();
            foreach (var id in selectedGenreIds)
            {
                var genre = await _repository.GetById<Genre>(id);
                if (genre != null)
                {
                    selectedGenres.Add(genre);
                }
            }
            return selectedGenres ?? new List<Genre>();
        }

        [NonAction]
        private async Task<ProfileModel> createProfileModelFromUser(User user)
        {
            var selectedGenres = await getSelectedGenres(user.UserName);
            return new ProfileModel(user, selectedGenres);

        }

        [NonAction]
        private UserGenreConnection findCorrespondingConnection(Genre genre, IEnumerable<UserGenreConnection> RelevantConnections)
        {
            return RelevantConnections.Where(c => c.GenreId == genre.Id).First();

        }

        [NonAction]
        private async Task handleGenreSelection(User user, ICollection<string> newSelectedGenres)
        {
            string username = user.UserName;
            var SelectedGenres = await getSelectedGenres(username);
            var RelevantConnections = await getRelevantUserGenreConnections(username);

            foreach (var genre in  SelectedGenres)
            {
                if (newSelectedGenres.Contains(genre.Name))
                {
                    newSelectedGenres.Remove(genre.Name);
                    continue;
                }
                var CorrespondingConnection = findCorrespondingConnection(genre, RelevantConnections);
                if (CorrespondingConnection != null)
                {
                    await _repository.Delete<UserGenreConnection>(CorrespondingConnection.Id);
                }
            }
            foreach (var genre in newSelectedGenres)
            {
                var clientGenreConnection = new UserGenreConnection();
                var genres = await _repository.GetAll<Genre>();
                var g = genres.Select(g => g).Where(g => g.Name == genre).First();
                if (g == null) continue;
                clientGenreConnection.GenreId = g.Id;
                clientGenreConnection.Username = user.UserName;
                await _repository.Insert(clientGenreConnection);
            }
            await _repository.Save();
        }
    }
}
