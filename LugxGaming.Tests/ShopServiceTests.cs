using LugxGaming.BusinessLogic.Models.Shop;
using LugxGaming.BusinessLogic.Services;
using LugxGaming.Data.Data;
using LugxGaming.Data.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LugxGaming.Tests
{
    public class ShopServiceTests
    {
        private ApplicationDbContext dbContext;
        private ShopService shopService;

        [SetUp]
        public void Setup()
        {
            // Initialize in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            dbContext = new ApplicationDbContext(options);

            // Seed the database with test data
            SeedDatabase();

            // Initialize ShopService
            shopService = new ShopService(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            if (dbContext != null)
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Dispose();
            }
        }

        private async void SeedDatabase()
        {
            var genre1 = new Genre { Id = 1, Name = "Action" };
            var genre2 = new Genre { Id = 2, Name = "Adventure" };

            var games = new List<Game>
            {
                new Game { Id = 1, Name = "Game1", Genre = genre1, Price = 29.99m, ImageUrl = "url1", VideoUrl = "Video/Url1", Description = "Description1" },
                new Game { Id = 2, Name = "Game2", Genre = genre2, Price = 39.99m, ImageUrl = "url2", VideoUrl = "Video/Url2", Description = "Description2" },
                new Game { Id = 3, Name = "Game3", Genre = genre1, Price = 19.99m, ImageUrl = "url3", VideoUrl = "Video/Url3", Description = "Description3" }
            };

            var user = new User { Id = "user1", UserName = "testuser", FirstName = "testFirstName", LastName = "testLastName" };

            dbContext.Genres.AddRange(genre1, genre2);
            dbContext.Games.AddRange(games);
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var game = await dbContext.Games.FindAsync(1);

            var reviews = new List<Review>
            {
                new Review { Id = 1, Game = game, GameId = game.Id, User = user, UserId = user.Id, Comment = "Great game!", Rating = 5, CreatedOn = DateTime.UtcNow },
                new Review { Id = 2, Game = game, GameId = game.Id, User = user, UserId = user.Id, Comment = "Good game.", Rating = 4, CreatedOn = DateTime.UtcNow.AddDays(-1) },
                new Review { Id = 3, Game = game, GameId = game.Id, User = user, UserId = user.Id, Comment = "Not bad.", Rating = 3, CreatedOn = DateTime.UtcNow.AddDays(-2) }
            };

            dbContext.Reviews.AddRange(reviews);
            dbContext.SaveChanges();
        }

        [Test]
        public async Task Test_GetAllGames_ShouldReturnAllGamesSortedByNameAndPrice()
        {
            // Act
            var result = await shopService.GetAllGames();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Game1", result[0].GameName);
            Assert.AreEqual("Game2", result[1].GameName);
            Assert.AreEqual("Game3", result[2].GameName);
            Assert.AreEqual(29.99m, result[0].Price);
            Assert.AreEqual(39.99m, result[1].Price);
            Assert.AreEqual(19.99m, result[2].Price);
        }

        [Test]
        public async Task Test_GetSearchedGames_ShouldReturnGamesOfSpecificGenre()
        {
            // Act
            var result = await shopService.GetSearchedGames("Action");

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(g => g.GameGenre == "Action"));
        }

        [Test]
        public async Task Test_GetGame_ShouldReturnGameDetails()
        {
            // Act
            var result = await shopService.GetGame("Game1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Game1", result.GameName);
            Assert.AreEqual("Action", result.GameGenre);
            Assert.AreEqual(29.99m, result.USDPrice);
            Assert.AreEqual("url1", result.ImageUrl);
            Assert.AreEqual("Description1", result.Description);
        }

        [Test]
        public async Task Test_GetGame_ShouldReturnNullIfGameNotFound()
        {
            // Act
            var result = await shopService.GetGame("NonExistentGame");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task Test_GetFirstGame_ShouldReturnFirstGame()
        {
            // Act
            var result = await shopService.GetFirstGame();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Game1", result.GameName);
            Assert.AreEqual("Action", result.GameGenre);
            Assert.AreEqual(29.99m, result.USDPrice);
            Assert.AreEqual("url1", result.ImageUrl);
            Assert.AreEqual("Description1", result.Description);
        }

        [Test]
        public async Task Test_FillRelatedGames_ShouldReturnTopThreeNonRelatedGames()
        {
            // Act
            var result = await shopService.FillRelatedGames("Action", "Game1");

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsFalse(result.Any(g => g.GameName == "Game1"));
            Assert.AreEqual("Game3", result[0].GameName);
        }

        [Test]
        public async Task Test_FillRelatedGames_ShouldReturnFromAllGamesIfNoRelatedGamesFound()
        {
            // Act
            var result = await shopService.FillRelatedGames("NonExistentGenre", "Game1");

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsFalse(result.Any(g => g.GameName == "Game1"));
        }

        [Test]
        public async Task Test_GetAllReviewsAssociatedToGameAsync_ShouldReturnReviewsForGame()
        {
            // Act
            var result = await shopService.GetAllReviewsAssociatedToGameAsync("Game1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Great game!", result[0].Comment);
            Assert.AreEqual("Good game.", result[1].Comment);
            Assert.AreEqual("Not bad.", result[2].Comment);
        }

        [Test]
        public async Task Test_WriteReviewAsync_ShouldAddReview()
        {
            // Arrange
            var newReview = new ReviewViewModel
            {
                GameName = "Game1",
                UserName = "testuser",
                Comment = "Awesome game!",
                Rating = 5,
                CreatedOn = DateTime.UtcNow.ToString("dd MMM yyyy")
            };

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "testuser");

            // Act
            var result = await shopService.WriteReviewAsync(newReview, user);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsEmpty(result.ErrorMessage);

            var reviews = await dbContext.Reviews.Where(r => r.Game.Name == "Game1").ToListAsync();
            Assert.AreEqual(4, reviews.Count);
            Assert.AreEqual("Awesome game!", reviews.Last().Comment);
        }

        [Test]
        public async Task Test_WriteReviewAsync_ShouldReturnErrorIfGameNotFound()
        {
            // Arrange
            var newReview = new ReviewViewModel
            {
                GameName = "NonExistentGame",
                UserName = "testuser",
                Comment = "This game doesn't exist!",
                Rating = 1,
                CreatedOn = DateTime.UtcNow.ToString("dd MMM yyyy")
            };

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "testuser");

            // Act
            var result = await shopService.WriteReviewAsync(newReview, user);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Game not found", result.ErrorMessage);
        }
    }
}