using LugxGaming.Data;
using LugxGaming.Infrastructure;
using LugxGaming.Models;
using LugxGaming.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LugxGaming.Controllers
{
	public class ShopController : Controller
    {
        private readonly ICurrencyService currencyService;
        private readonly IShopService shopService;

        public ShopController(ICurrencyService currencyService, IShopService shopService)
        {
            this.currencyService = currencyService;
            this.shopService = shopService;
        }

        public async Task<IActionResult> Index(int? pageNumber, string? searchString)
        {
            int pageSize = 8;

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

            selectedGame.RelatedGames = await this.shopService.FillRelatedGames(selectedGame.GameGenre, selectedGame.GameName);

            var ethPriceInUsd = await this.currencyService.GetEthPriceInUsdAsync();
            selectedGame.ETHPrice = selectedGame.USDPrice / ethPriceInUsd;

			return View(selectedGame);
        }
    }
}
