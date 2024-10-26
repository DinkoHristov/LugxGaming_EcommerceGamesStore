using LugxGaming.Data.Models;
using LugxGaming.Infrastructure;
using LugxGaming.Models;
using LugxGaming.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LugxGaming.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signManager;
        private readonly IAccountService accountService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signManager,
            IAccountService accountService)
        {
            this.userManager = userManager;
            this.signManager = signManager;
            this.accountService = accountService;
        }

        public async Task<IActionResult> Users(int? pageNumber)
        {
            int pageSize = 8;

            var users = await this.accountService.GetAllUsersAsync();

            var usersPerPage = PaginatedList<UserViewModel>.Create(users, pageNumber ?? 1, pageSize);

            return View(usersPerPage);
        }

        public async Task<IActionResult> AllPurchases(string id, int? pageNumber)
        {
            int pageSize = 10;

            var userPurchases = await this.accountService.GetAllPurchasesFromUserAsync(id);

            var gamesPerPage = PaginatedList<PurchaseGameViewModel>.Create(userPurchases, pageNumber ?? 1, pageSize);

            return View(gamesPerPage);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            var isUserEdited = await this.accountService.EditUserAsync(model.Id, model.Email, model.FirstName, model.LastName);
            
            if (isUserEdited)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var isUserDeleted = await this.accountService.DeleteUserAsync(id);

            if (isUserDeleted)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        public IActionResult ResetEmail()
        {
            return View();
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> ResetEmail(ResetEmailFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var isEmailChanged = await this.accountService.ResetEmailAsync(userId, model.Email);

            if (isEmailChanged)
            {
                ViewBag.Reset = true;

                return View();
            }

            ViewBag.Reset = false;

            return RedirectToAction(nameof(AccountController.UserInformation));
        }

        public async Task<IActionResult> UserInformation()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
	        var user = await this.userManager.GetUserAsync(User);

            var isUserSignedIn = this.signManager.IsSignedIn(User);

            if (isUserSignedIn)
            {
                return View(user);
            }

            return RedirectToAction(nameof(AccountController.SignIn));
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.InitialError = true;

                return View(model);
            }

            var (success, errorKey, errorMessage) = await this.accountService.SignInAsync(model.Email, model.Password);

            if (!success)
            {
                ModelState.AddModelError(errorKey, errorMessage);

                return View(model);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.InitialError = true;

                return View(model);
            }

            var (success, errorKey, errorMessage) = await this.accountService.SignUpAsync(model.Email, model.FirstName, model.LastName, model.Password);

            if (!success)
            {
                ModelState.TryAddModelError(errorKey, errorMessage);

                return View(model);
            }

            ViewBag.Registered = true;

            ModelState.Clear();

            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);

            if (user == null) 
            {
                ViewBag.UserDoesntExist = true;

                return View();
            }

            await this.userManager.ConfirmEmailAsync(user, token);

            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            HttpContext.Session.Clear();

			await this.signManager.SignOutAsync();

			return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, errorKey, errorMessage) = await this.accountService.ForgotPasswordAsync(model.Email);

            if (!success)
            {
                ViewBag.UserDoesntExist = true;

                ModelState.AddModelError(errorKey, errorMessage);

                return View(model);
            }

            ViewBag.EmailSend = true;

            ModelState.Clear();

            return View();
        }

        public async Task<IActionResult> ResetPasswordAsync(string email, string token)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var isUserSignedIn = this.signManager.IsSignedIn(User);

            var model = await this.accountService.SetPasswordResetModelAsync(isUserSignedIn, userId, email, token);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordFormModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.InitialError = true;

                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return View();
            }

            var (success, errorKey, errorMessage) = await this.accountService.ResetPasswordAsync(model.Email, model.Token, model.Password);

            if (!success)
            {
                ViewBag.PasswordError = true;

                ModelState.TryAddModelError(errorKey, errorMessage);

                return View();
            }

            return RedirectToAction("ResetPasswordConfirmation");
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
