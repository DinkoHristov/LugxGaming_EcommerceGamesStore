using LugxGaming.Data;
using LugxGaming.Data.Models;
using LugxGaming.Models;
using LugxGaming.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LugxGaming.Services
{
    public class CreateGameService : ICreateGameService
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

        public async Task<(bool Success, string ErrorMessage)> CreateGame(string gameName, int genreId,
            decimal price, string imageUrl,string description)
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
                Description = description.TrimEnd()
            };

            await this.dbContext.Games.AddAsync(newGame);

            await this.dbContext.SaveChangesAsync();

            return (true, string.Empty);
        }
    }
}
