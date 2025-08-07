using LugxGaming.BusinessLogic.Interfaces;
using LugxGaming.BusinessLogic.Models.CreateGame;
using LugxGaming.Data.Data;
using LugxGaming.Data.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LugxGaming.BusinessLogic.Services
{
    public class CreateGameService : ICreateGameInterface
    {
        private readonly ApplicationDbContext dbContext;

        public CreateGameService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<GenreFormModel>> GetAllGenres()
            => await this.dbContext.Genres
                .AsNoTracking()
                .Select(g => new GenreFormModel
                {
                    Id = g.Id,
                    Name = g.Name
                }).ToListAsync();

        public async Task<List<GameFormModel>> GetAllGames()
            => await this.dbContext.Games
                .AsNoTracking()
                .Select(g => new GameFormModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    Image = g.ImageUrl
                })
                .ToListAsync();

        public async Task<Game> GetGameById(int id)
            => await this.dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);

        public async Task<(bool Success, string GameName, string ErrorMessage)> UpdatePromoPrice(int id, decimal discount)
        {
            var selectedGame = await GetGameById(id);
            if (selectedGame == null)
                return (false, string.Empty, "Game is not found");

            selectedGame.Discount = discount / 100.0m;
            selectedGame.PromoPrice = selectedGame.Price * (1 - selectedGame.Discount);

            await this.dbContext.SaveChangesAsync();

            return (true, selectedGame.Name, string.Empty);
        }

        public async Task<(bool Success, string ErrorMessage)> CreateGame(string gameName, int genreId,
            decimal price, string imageUrl, string videoUrl, string description)
        {
            var game = await this.dbContext.Games.FirstOrDefaultAsync(g => g.Name == gameName);

            if (game != null)
            {
                return (false, "Game already exists!");
            }

            var newGame = new Game
            {
                Name = gameName,
                Genre = await this.dbContext.Genres.FirstOrDefaultAsync(g => g.Id == genreId),
                Price = price,
                ImageUrl = imageUrl,
                VideoUrl = videoUrl,
                Description = description.TrimEnd()
            };

            await this.dbContext.Games.AddAsync(newGame);

            await this.dbContext.SaveChangesAsync();

            return (true, string.Empty);
        }
    }
}