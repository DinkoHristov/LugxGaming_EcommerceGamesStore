using LugxGaming.Models;
using LugxGaming.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LugxGaming.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IContactUsService contactUsService;

        public ContactUsController(IContactUsService contactUsService)
        {
            this.contactUsService = contactUsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(MessageFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isMessageSent = await SendMessage(model);

            if (isMessageSent)
            {
                ViewBag.IsMessageSent = true;

                return View();
            }

            ViewBag.IsMessageSent = false;

            return View();
        }

        private async Task<bool> SendMessage(MessageFormModel model)
        {
            var isMessageSent = await this.contactUsService.SendEmailWithQuestionAsync(model);

            if (isMessageSent)
            {
                ModelState.Clear();
            }

            return isMessageSent;
        }
    }
}
