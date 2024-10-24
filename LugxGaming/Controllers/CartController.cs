using LugxGaming.Data;
using LugxGaming.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LugxGaming.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICurrencyService currencyService;
        private readonly ICartService cartService;

        public CartController(ApplicationDbContext context, ICurrencyService currencyService,
                              ICartService cartService)
        {
            dbContext = context;
            this.currencyService = currencyService;
            this.cartService = cartService;     
        }

        public async Task<IActionResult> Index()
        {
            var (cartModel, ethereumAccount) = await this.cartService.SetCartModelAsync();

			ViewBag.EthereumAccount = ethereumAccount;

            return View(cartModel);
        }

        [Authorize]
        public async Task<IActionResult> Add(int quantity, string gameName)
        {
            var isItemQuantityIncreased = await this.cartService.AddItemToCartAsync(quantity, gameName);

            if (!isItemQuantityIncreased)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Authorize]
        public async Task<IActionResult> Decrease(string gameName)
        {
            var isItemQuantityDecreased = await this.cartService.DecreaseItemQuantityAsync(gameName);

            if (!isItemQuantityDecreased)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Authorize]
        public async Task<IActionResult> Remove(string gameName)
        {
            var isItemRemoved = await this.cartService.RemoveItemFromCartAsync(gameName);

            if (!isItemRemoved)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return Redirect(Request.Headers["Referer"].ToString());
		}

        [Authorize]
        public IActionResult Clear()
        {
            var isCartCleared = this.cartService.RemoveCartItems();

            if (!isCartCleared)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return Redirect(Request.Headers["Referer"].ToString());
		}
	}
}
