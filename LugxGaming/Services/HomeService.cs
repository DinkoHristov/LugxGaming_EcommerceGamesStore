using LugxGaming.Data;
using LugxGaming.Data.Models;
using LugxGaming.Models;
using LugxGaming.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LugxGaming.Services
{
    public class HomeService : IHomeService
	{
		private readonly ApplicationDbContext dbContext;

        public HomeService(ApplicationDbContext dbContext)
        {
			this.dbContext = dbContext;
        }

        public async Task<List<HomeGameModel>> GetOneGameFromEachCategory()
		{
			var games = await GetGames();

			var eachCategoryGames = games.Select(g => new HomeGameModel
			{
				Image = g.ImageUrl,
				GameName = g.Name,
				GenreName = g.Genre.Name,
				Price = g.Price
			})
			.ToList();

			return eachCategoryGames;
		}

		public async Task<List<HomeGameModel>?> GetTopGames()
		{
			var topGames = await this.dbContext.Games
                .AsNoTracking()
                .Select(g => new HomeGameModel
                {
                    Image = g.ImageUrl,
                    GameName = g.Name,
                    GenreName = g.Genre.Name,
                    Price = g.Price
                })
                .Take(6)
                .ToListAsync();

            return topGames;
		}

		public async Task<List<HomeGameModel>> GetTopCategoriesGame()
		{
			var games = await GetGames();

			var eachCategoryGames = games.Select(g => new HomeGameModel
			{
				Image = g.ImageUrl,
				GameName = g.Name,
				GenreName = g.Genre.Name,
				Price = g.Price
			})
			.ToList();

			return eachCategoryGames;
		}

		private async Task<List<Game>> GetGames()
		{
			return new List<Game>()
			{
				await this.dbContext.Games.Include(g => g.Genre).AsNoTracking().FirstOrDefaultAsync(g => g.Genre.Name == "Action"),
                await this.dbContext.Games.Include(g => g.Genre).AsNoTracking().FirstOrDefaultAsync(g => g.Genre.Name == "Adventure"),
                await this.dbContext.Games.Include(g => g.Genre).AsNoTracking().FirstOrDefaultAsync(g => g.Genre.Name == "Strategy"),
                await this.dbContext.Games.Include(g => g.Genre).AsNoTracking().FirstOrDefaultAsync(g => g.Genre.Name == "Racing"),
                await this.dbContext.Games.Include(g => g.Genre).AsNoTracking().FirstOrDefaultAsync(g => g.Genre.Name == "Sport")
			};
		}
	}
}
