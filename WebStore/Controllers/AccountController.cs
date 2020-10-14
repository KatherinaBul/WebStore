using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        #region Процесс регистрации нового пользвоателя

        public IActionResult Register() => View(new RegisterUserViewModel());
        
        public async Task<IActionResult> IsNameFree(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return Json(user is null ? "true" : "Пользователь с таким именем уже существует");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            using (_logger.BeginScope("Регистрация пользователя {0}", model.UserName))
            {
                _logger.LogInformation("Начало процесса регистрации нового пользователя {0}", model.UserName);

                var user = new User
                {
                    UserName = model.UserName
                };

                var registrationResult = await _userManager.CreateAsync(user, model.Password);
                if (registrationResult.Succeeded)
                {
                    _logger.LogInformation("Пользователь {0} успешно зарегистрирован", user.UserName);

                    await _userManager.AddToRoleAsync(user, Role.User);

                    _logger.LogInformation("Пользователь {0} наделён ролью {1}", user.UserName, Role.User);

                    await _signInManager.SignInAsync(user, false);
                    _logger.LogInformation("Пользователь {0} автоматически вошёл в систему после регистрации",
                        user.UserName);

                    return RedirectToAction("Index", "Home");
                }

                _logger.LogWarning("Ошибка при регистрации нового пользователя {0}\r\n",
                    model.UserName,
                    string.Join(Environment.NewLine, registrationResult.Errors.Select(error => error.Description)));

                foreach (var error in registrationResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        #endregion

        #region Процесс входа пользователя в систему

        public IActionResult Login(string returnUrl) => View(new LoginViewModel {ReturnUrl = returnUrl});

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var loginResult = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                false);

            _logger.LogInformation("Попытка входа пользователя {0} в систему", model.UserName);

            if (loginResult.Succeeded)
            {
                _logger.LogInformation("Пользователь {0} успешно вошёл в систему", model.UserName);

                if (Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                return RedirectToAction("Index", "Home");
            }

            _logger.LogWarning("Ошибка имени пользователя, или пароля при попытке входа {0}", model.UserName);

            ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль!");

            return View(model);
        }

        #endregion

        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity.Name;
            await _signInManager.SignOutAsync();

            _logger.LogInformation("Пользователь {0} вышел из системы", userName);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}