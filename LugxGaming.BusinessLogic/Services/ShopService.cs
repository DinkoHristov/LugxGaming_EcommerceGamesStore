using LugxGaming.BusinessLogic.Interfaces;
using LugxGaming.BusinessLogic.Models.Shop;
using LugxGaming.Data.Data;
using LugxGaming.Data.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LugxGaming.BusinessLogic.Services
{
    public class ShopService : IShopInterface
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
                    VideoUrl = g.VideoUrl,
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
                VideoUrl = g.VideoUrl,
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

        public async Task<List<ReviewViewModel>?> GetAllReviewsAssociatedToGameAsync(string? gameName)
        {
            return await this.dbContext.Reviews
                .Where(r => r.Game.Name == gameName)
                .Select(r => new ReviewViewModel
                {
                    UserName = r.User.UserName,
                    GameName = gameName,
                    Comment = r.Comment,
                    Rating = r.Rating,
                    CreatedOn = r.CreatedOn.ToString("dd MMM yyyy")
                })
                .OrderByDescending(r => r.Rating)
                .ToListAsync();
        }

        public async Task<(bool Success, string ErrorMessage)> WriteReviewAsync(ReviewViewModel model, User? user)
        {
            try
            {
                var game = await this.dbContext.Games.FirstOrDefaultAsync(g => g.Name == model.GameName);

                if (game is null)
                {
                    return (false, "Game not found");
                }

                var review = new Review()
                {
                    GameId = game.Id,
                    Game = game,
                    User = user,
                    UserId = user.Id,
                    Comment = model.Comment,
                    CreatedOn = DateTime.Parse(model.CreatedOn),
                    Rating = model.Rating
                };

                await this.dbContext.Reviews.AddAsync(review);
                await this.dbContext.SaveChangesAsync();

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}