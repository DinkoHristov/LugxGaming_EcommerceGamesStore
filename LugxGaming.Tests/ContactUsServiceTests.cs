using LugxGaming.BusinessLogic.Models.Contact;
using LugxGaming.BusinessLogic.Services;

namespace LugxGaming.Tests
{
    public class ContactUsServiceTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public async Task Test_SendEmailWithQuestionAsync()
        {
            var messageModel = new MessageFormModel()
            {
                Name = "Ivan",
                Surname = "Ivanov",
                Email = "Ivanov@gmail.com",
                Subject = "Test message",
                Message = "Test message description"
            };

            var contactUsService = new ContactUsService();
            var result = await contactUsService.SendEmailWithQuestionAsync(messageModel);

            Assert.AreEqual(true, result);
        }
    }
}