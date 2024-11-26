using LugxGaming.BusinessLogic.Models.Contact;

namespace LugxGaming.BusinessLogic.Interfaces
{
    public interface IContactUsInterface
    {
        /// <summary>
        /// This method sends email to the email address with certain question
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<bool> SendEmailWithQuestionAsync(MessageFormModel model);
    }
}