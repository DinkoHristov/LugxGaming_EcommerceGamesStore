using LugxGaming.Models;

namespace LugxGaming.Services.Interfaces
{
	public interface IContactUsService
	{
		/// <summary>
		/// This method sends email to the email address with certain question
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public Task<bool> SendEmailWithQuestionAsync(MessageFormModel model);
	}
}
