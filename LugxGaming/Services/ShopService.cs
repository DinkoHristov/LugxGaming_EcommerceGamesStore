using LugxGaming.Data;
using LugxGaming.Models;
using LugxGaming.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LugxGaming.Services
{
	public class ShopService : IShopService
	{
		private readonly ApplicationDbContext dbContext;

        public ShopService(ApplicationDbContext dbContext)
        {
			this.dbContext = dbContext;
        }

        public async Task<List<ShopGameModel>> GetAllGames()
		{
			return await this.dbContext.Games
				.AsNoTracking()
				.Select(g => new ShopGameModel
				{
					GameName = g.Name,
					GameGenre = g.Genre.Name,
					Price = g.Price,
					ImageUrl = g.ImageUrl
				})
				.OrderBy(g => g.GameName)
				.ThenBy(g => g.Price)
				.ToListAsync();
		}

		public async Task<List<ShopGameModel>> GetSearchedGames(string gameGenre)
		{
			return await this.dbContext.Games
				.AsNoTracking()
				.Where(g => g.Genre.Name == gameGenre)
				.Select(g => new ShopGameModel
				{
					GameName = g.Name,
					GameGenre = g.Genre.Name,
					Price = g.Price,
					ImageUrl = g.ImageUrl
				})
				.OrderBy(g => g.GameName)
				.ThenBy(g => g.Price)
				.ToListAsync();
		}

		public async Task<GameDetailsModel?> GetGame(string? gameName)
		{
			return await this.dbContext.Games
				.Where(g => g.Name == gameName)
				.Select(g => new GameDetailsModel
				{
					GameName = g.Name,
					GameGenre = g.Genre.Name,
					USDPrice = g.Price,
					ImageUrl = g.ImageUrl,
					Description = g.Description
				})
				.FirstOrDefaultAsync();
		}

		public async Task<GameDetailsModel?> GetFirstGame()
		{
			return await this.dbContext.Games.Select(g => new GameDetailsModel
			{
				GameName = g.Name,
				GameGenre = g.Genre.Name,
				USDPrice = g.Price,
				ImageUrl = g.ImageUrl,
				Description = g.Description
			})
			.FirstOrDefaultAsync();
		}

        public async Task<List<ShopGameModel>> FillRelatedGames(string gameGenre, string gameName)
        {
			var searchedGames = await GetSearchedGames(gameGenre);

			var topThreeNonRelatedGames = searchedGames
				.Where(g => g.GameName != gameName)
				.Take(3)
				.ToList();

			if (!topThreeNonRelatedGames.Any())
			{
				var allGames = await GetAllGames();
				
				topThreeNonRelatedGames = allGames
					.Where(g => g.GameName != gameName)
                    .Take(3)
                    .ToList();
            }

			return topThreeNonRelatedGames;
        }
    }
}
