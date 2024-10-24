using LugxGaming.Models;

namespace LugxGaming.Services.Interfaces
{
    public interface IAccountService
	{
        /// <summary>
        /// This method returns all users from the database
        /// </summary>
        /// <returns></returns>
        Task<List<UserViewModel>> GetAllUsersAsync();

        /// <summary>
        /// This method returns all users purchases from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		Task<List<PurchaseGameViewModel>> GetAllPurchasesFromUserAsync(string id);

		/// <summary>
		/// This method sends email to the users email address
		/// </summary>
		/// <param name="email"></param>
		/// <param name="subject"></param>
		/// <param name="confirmationLink"></param>
		/// <returns></returns>
		Task<bool> SendEmailAsync(string email, string subject, string confirmationLink);

        /// <summary>
        /// Edit the selected user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        Task<bool> EditUserAsync(string userId, string email, string firstName, string lastName);

        /// <summary>
        /// Delete the selected user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteUserAsync(string id);

        /// <summary>
        /// Reset user's email
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> ResetEmailAsync(string userId, string email);

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<(bool Success, string ErrorKey, string ErrorMessage)> SignInAsync(string email, string password);

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<(bool Success, string ErrorKey, string ErrorMessage)> SignUpAsync(string email, string firstName, string lastName, string password);

        /// <summary>
        /// When user forgot his password -> sends link to reset it
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<(bool Success, string ErrorKey, string ErrorMessage)> ForgotPasswordAsync(string email);

        /// <summary>
        /// Set the reset password model
        /// </summary>
        /// <param name="isUserSignedIn"></param>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ResetPasswordFormModel> SetPasswordResetModelAsync(bool isUserSignedIn, string userId, string email, string token);

        /// <summary>
        /// Resets user's password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<(bool Success, string ErrorKey, string ErrorMessage)> ResetPasswordAsync(string email, string token, string password);
	}
}
