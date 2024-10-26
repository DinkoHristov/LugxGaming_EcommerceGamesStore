using LugxGaming.Infrastructure;
using LugxGaming.Models;
using LugxGaming.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;
using System.Security.Claims;

namespace LugxGaming.Controllers
{
    public class PaymentController : Controller
    {
        private readonly StripeSettings stripe;
        private readonly IPaymentService paymentService;

        public PaymentController(IOptions<StripeSettings> stripeSettigns, IPaymentService paymentService)
        {
            this.stripe = stripeSettigns.Value;
            this.paymentService = paymentService;
        }

        public string SessionId { get; set; }

        [Authorize]
        public async Task<IActionResult> PayWithStripe(string amount)
        {
            var result = await this.paymentService.PayWithCard(amount);

            if (!result.OK)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return Redirect(result.SessionUrl);
        }

        public async Task<IActionResult> PaymentSuccess()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var result = await this.paymentService.SavePurchases(userId, cart);

            if (!result.OK)
            {
                return RedirectToAction(nameof(PaymentController.PaymentFailed));
            }

			HttpContext.Session.Clear();

			return View();
        }

		public IActionResult PaymentFailed()
		{
			return View();
		}

		[HttpGet]
        public IActionResult GetAccount()
        {
            var account = HttpContext.Session.GetString("MetaMaskAccount");
            if (account != null)
            {
                return Ok(account);
            }

            return NotFound();
        }

		[HttpPost]
		public IActionResult SaveAccount([FromBody] MetaMaskAccountModel model)
		{
			if (model.Account != null)
			{
				HttpContext.Session.SetString("MetaMaskAccount", model.Account);

				return Ok();
			}

			return BadRequest();
		}

		[HttpPost]
        public IActionResult LogoutAccount()
        {
            if (HttpContext.Session.Keys.Contains("MetaMaskAccount"))
            {
                HttpContext.Session.Remove("MetaMaskAccount");

                return Ok();
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAmountInWei(string amount)
        {
            if (decimal.TryParse(amount, out decimal amountInEthers))
            {
                var amountInWei = Web3.Convert.ToWei(amountInEthers).ToHex(false);
                return Ok(amountInWei.ToString());
            }
            return BadRequest("Invalid amount");
        }
    }
}
