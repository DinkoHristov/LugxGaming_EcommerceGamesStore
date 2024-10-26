using LugxGaming.Data.Models;
using LugxGaming.Infrastructure;
using LugxGaming.Models;
using LugxGaming.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LugxGaming.Controllers
{
    public class ShopController : Controller
    {
        private readonly ICurrencyService currencyService;
        private readonly IShopService shopService;
        private readonly UserManager<User> userManager;

        public ShopController(ICurrencyService currencyService, IShopService shopService, UserManager<User> userManager)
        {
            this.currencyService = currencyService;
            this.shopService = shopService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int? pageNumber, string? searchString, int pageSize = 8)
        {
            ViewData["SearchString"] = searchString;

            var games = await this.shopService.GetAllGames();
            if (searchString != null)
            {
                games = await this.shopService.GetSearchedGames(searchString);
            }

            var gamesPerPage = PaginatedList<ShopGameModel>.Create(games, pageNumber ?? 1, pageSize);

            return View(gamesPerPage);
        }

        public async Task<IActionResult> GameDetails(string? gameName)
        {
            var selectedGame = await this.shopService.GetGame(gameName);

            if (selectedGame == null)
            {
                selectedGame = await this.shopService.GetFirstGame();
            }

            var allReviews = await this.shopService.GetAllReviewsAssociatedToGameAsync(selectedGame.GameName);
            var topSixReviews = allReviews.Take(6).ToList();

            selectedGame.RelatedGames = await this.shopService.FillRelatedGames(selectedGame.GameGenre, selectedGame.GameName);
            selectedGame.Reviews = topSixReviews;

            var ethPriceInUsd = await this.currencyService.GetEthPriceInUsdAsync();
            selectedGame.ETHPrice = selectedGame.USDPrice / ethPriceInUsd;

            ViewData["AllReviewsCount"] = allReviews.Count;

			return View(selectedGame);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> WriteReview(ReviewViewModel model)
        {
            var user = await this.userManager.GetUserAsync(User);

            var result = await this.shopService.WriteReviewAsync(model, user);

            if (!result.Success)
            {
                return RedirectToAction(nameof(ShopController.Index));
            }    

            return RedirectToAction(nameof(ShopController.GameDetails), "Shop", new { gameName = model.GameName });
        }

        public async Task<IActionResult> AllReviews(string? gameName, int? pageNumber, int pageSize = 7)
        {
            var allReviews = await this.shopService.GetAllReviewsAssociatedToGameAsync(gameName);

            var reviewsPerPage = PaginatedList<ReviewViewModel>.Create(allReviews, pageNumber ?? 1, pageSize);

            return View(reviewsPerPage);
        }
    }
}