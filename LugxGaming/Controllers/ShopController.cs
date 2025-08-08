﻿using LugxGaming.BusinessLogic.Extensions;
using LugxGaming.BusinessLogic.Interfaces;
using LugxGaming.BusinessLogic.Models.Shop;
using LugxGaming.Data.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LugxGaming.Controllers
{
    public class ShopController : Controller
    {
        private readonly ICurrencyInterface currencyService;
        private readonly IShopInterface shopService;
        private readonly UserManager<User> userManager;

        public ShopController(ICurrencyInterface currencyService, IShopInterface shopService, UserManager<User> userManager)
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

            var isPromoPriceUsed = selectedGame.USDPromoPrice != 0 && selectedGame.USDPromoPrice < selectedGame.USDPrice;
            var ethPriceInUsd = await this.currencyService.GetEthPriceInUsdAsync();
            selectedGame.ETHPrice = isPromoPriceUsed 
                ? selectedGame.USDPromoPrice / ethPriceInUsd
                : selectedGame.USDPrice / ethPriceInUsd;

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