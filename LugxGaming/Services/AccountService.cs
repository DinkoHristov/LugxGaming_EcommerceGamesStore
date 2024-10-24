using LugxGaming.Data;
using LugxGaming.Data.Enums;
using LugxGaming.Data.Models;
using LugxGaming.Models;
using LugxGaming.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;

namespace LugxGaming.Services
{
    public class AccountService : IAccountService
	{
		private readonly ApplicationDbContext dbContext;
		private readonly UserManager<User> userManager;
		private readonly SignInManager<User> signManager;
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccountService(ApplicationDbContext dbContext, UserManager<User> userManager,
                              SignInManager<User> signManager, IUrlHelperFactory urlHelperFactory,
                              IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
			this.userManager = userManager;
			this.signManager = signManager;
            this.urlHelperFactory = urlHelperFactory;
            this.httpContextAccessor = httpContextAccessor;
        }

		public async Task<List<UserViewModel>> GetAllUsersAsync()
		{
			return await this.dbContext.Users
				.AsNoTracking()
				.Include(u => u.Purchases)
				.Select(u => new UserViewModel
				{
                    Id = u.Id,
					Email = u.Email,
					FirstName = u.FirstName,
					LastName = u.LastName,
					Purchases = u.Purchases.Select(x => new PurchaseGameViewModel
					{
						GameName = x.Game.Name,
						GameGenre = x.Game.Genre.Name,
						Quantity = x.Quantity,
						Price = x.Game.Price
					})
					.ToList()
				})
				.ToListAsync();
		}

		public async Task<List<PurchaseGameViewModel>> GetAllPurchasesFromUserAsync(string id)
		{
			return await this.dbContext.Users
				.AsNoTracking()
				.Include(u => u.Purchases)
				.ThenInclude(p => p.Game)
				.Where(u => u.Id == id)
				.SelectMany(u => u.Purchases)
				.Select(p => new PurchaseGameViewModel
				{
					GameName = p.Game.Name,
					GameGenre = p.Game.Genre.Name,
					Quantity = p.Quantity,
					Price = p.Game.Price
				})
				.ToListAsync();
		}

		public async Task<bool> SendEmailAsync(string email, string subject, string confirmationLink)
		{
			try
			{
				using (MailMessage message = new MailMessage())
				{
					message.From = new MailAddress("dinkodh@gmail.com");
					message.To.Add(email);
					message.Subject = subject;
					message.IsBodyHtml = true;
					message.Body = confirmationLink;

					using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
					{
						smtpClient.Port = 587;

						smtpClient.EnableSsl = true;
						smtpClient.UseDefaultCredentials = false;
						smtpClient.Credentials = new NetworkCredential("dinkodh@gmail.com", "plwi bmib hsjq wtdq");
						smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
						await smtpClient.SendMailAsync(message);

						return true;
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

        public async Task<bool> EditUserAsync(string userId, string email, string firstName, string lastName)
        {
            var dbUser = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (dbUser != null)
            {
                dbUser.NormalizedEmail = email;
                dbUser.Email = email;
                dbUser.FirstName = firstName;
                dbUser.LastName = lastName;
                dbUser.UserName = email;
                dbUser.NormalizedUserName = email;

                await this.dbContext.SaveChangesAsync();

				return true;
            }

			return false;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var dbUser = await this.dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (dbUser != null)
            {
                this.dbContext.Users.Remove(dbUser);

                await this.dbContext.SaveChangesAsync();

				return true;
            }

			return false;
        }

        public async Task<bool> ResetEmailAsync(string userId, string email)
        {
            var existingUser = await this.userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return false;
            }

            var user = await this.userManager.FindByIdAsync(userId);

            var token = await this.userManager.GenerateChangeEmailTokenAsync(user, email);

            var result = await this.userManager.ChangeEmailAsync(user, email, token);

			if (result.Succeeded)
				return true;

			return false;
        }

        public async Task<(bool Success, string ErrorKey, string ErrorMessage)> SignInAsync(string email, string password)
        {
            try
            {
                // Check if user with this email exists!
                var userEmail = await this.userManager.FindByEmailAsync(email);

                if (userEmail == null)
                {
                    return (false, "EmailNotFound", "Email is incorrect!");
                }

                // Check if user has confirmed his email!
                var userEmailConfirmed = await this.userManager.IsEmailConfirmedAsync(userEmail);

                if (!userEmailConfirmed)
                {
                    return (false, "EmailNotConfirmed", "Email is not confirmed!");
                }

                // Check if user password is correct!
                var isPasswordCorrect = await this.userManager.CheckPasswordAsync(userEmail, password);

                if (!isPasswordCorrect)
                {
                    return (false, "IncorrectPassword", "Password is incorrect!");
                }

                await this.signManager.SignInAsync(userEmail, true);

                return (true, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, "Error", ex.Message);
            }
        }

        public async Task<(bool Success, string ErrorKey, string ErrorMessage)> SignUpAsync(string email, string firstName, string lastName, string password)
        {
            var newUser = new User
            {
                UserName = email,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
            };

            var existingUser = await this.userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return (false, "ExistingUser", "Email is already taken!");
            }

            var result = await this.userManager.CreateAsync(newUser, password);

            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));

                return (false, "FailedCreatingUser", errorMessages);
            }

            var urlHelper = this.urlHelperFactory.GetUrlHelper(new ActionContext(this.httpContextAccessor.HttpContext, new RouteData(), new ActionDescriptor()));
            var token = await this.userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var confirmationLink = urlHelper.Action("ConfirmEmail", "Account", new { Email = newUser.Email, Token = token }, this.httpContextAccessor.HttpContext.Request.Scheme);

            await SendConfirmationEmailAsync(newUser.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>clicking here</a>");

            await this.userManager.AddToRoleAsync(newUser, Roles.User.ToString());

            return (true, string.Empty, string.Empty);
        }

        public async Task<(bool Success, string ErrorKey, string ErrorMessage)> ForgotPasswordAsync(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return (false, "UserDoesntExist", "User with this email doesn't exist!");
            }

            var urlHelper = this.urlHelperFactory.GetUrlHelper(new ActionContext(this.httpContextAccessor.HttpContext, new RouteData(), new ActionDescriptor()));
            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
            var callback = urlHelper.Action("ResetPassword", "Account", new { token, email = user.Email }, this.httpContextAccessor.HttpContext.Request.Scheme);

            await SendConfirmationEmailAsync(user.Email, "Reset password email",
                $"To change your password, click on the following <a href='{HtmlEncoder.Default.Encode(callback)}'>link</a>.");

            return (true, string.Empty, string.Empty);
        }

        public async Task<ResetPasswordFormModel> SetPasswordResetModelAsync(bool isUserSignedIn, string userId, string email, string token)
        {
            if (isUserSignedIn)
            {
                var user = await this.userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    email = user.Email;
                    var currentToken = await this.userManager.GeneratePasswordResetTokenAsync(user);
                    token = currentToken;
                }
            }

            return new ResetPasswordFormModel
            {
                Email = email,
                Token = token
            };
        }

        public async Task<(bool Success, string ErrorKey, string ErrorMessage)> ResetPasswordAsync(string email, string token, string password)
        {
            var user = await this.userManager.FindByEmailAsync(email);

            var resetPasswordResult = await this.userManager.ResetPasswordAsync(user, token, password);

            if (!resetPasswordResult.Succeeded)
            {
                var errorMessages = string.Join(", ", resetPasswordResult.Errors.Select(e => e.Description));

                return (false, "FailedCreatingUser", errorMessages);
            }

            return (true, string.Empty, string.Empty);
        }

        private async Task<bool> SendConfirmationEmailAsync(string email, string subject, string confirmationLink)
        {
            return await SendEmailAsync(email, subject, confirmationLink);
        }
    }
}
