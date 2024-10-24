using LugxGaming.Models;
using LugxGaming.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace LugxGaming.Services
{
    public class ContactUsService : IContactUsService
	{
		public async Task<bool> SendEmailWithQuestionAsync(MessageFormModel model)
		{
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(model.Email);
                    message.To.Add("dinkodh@gmail.com");
                    message.Subject = model.Subject;
                    message.IsBodyHtml = true;
                    message.Body = model.Message;

                    message.ReplyToList.Add(new MailAddress(model.Email));

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
	}
}
