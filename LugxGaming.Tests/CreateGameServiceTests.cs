using LugxGaming.BusinessLogic.Services;
using LugxGaming.Data.Data;
using LugxGaming.Data.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LugxGaming.Tests
{
    public class CreateGameServiceTests
    {
        private ApplicationDbContext _dbContext;
        private CreateGameService _createGameService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "LugxGamingTestDb")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _createGameService = new CreateGameService(_dbContext);

            SeedDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        private void SeedDatabase()
        {
            var genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Adventure" }
            };

            _dbContext.Genres.AddRange(genres);
            _dbContext.SaveChanges();
        }

        [Test]
        public async Task Test_GetAllGenres_ReturnsAllGenres()
        {
            // Act
            var genres = await _createGameService.GetAllGenres();

            // Assert
            Assert.AreEqual(2, genres.Count);
            Assert.IsTrue(genres.Any(g => g.Name == "Action"));
            Assert.IsTrue(genres.Any(g => g.Name == "Adventure"));
        }

        [Test]
        public async Task Test_CreateGame_GameAlreadyExists_ReturnsFalseWithErrorMessage()
        {
            // Arrange
            var existingGame = new Game
            {
                Id = 1,
                Name = "Existing Game",
                Genre = await _dbContext.Genres.FirstAsync(g => g.Id == 1),
                Price = 59.99m,
                ImageUrl = "http://example.com/image.jpg",
                VideoUrl = "https://youtu.be/S7MnkE_qxj8",
                Description = "A popular existing game."
            };

            await _dbContext.Games.AddAsync(existingGame);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _createGameService.CreateGame("Existing Game", 1, 59.99m, "http://example.com/image.jpg", "https://youtu.be/S7MnkE_qxj8", "A popular existing game.");

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Game already exists!", result.ErrorMessage);
        }

        [Test]
        public async Task Test_CreateGame_NewGame_ReturnsTrue()
        {
            // Act
            var result = await _createGameService.CreateGame("New Game", 1, 49.99m, "http://example.com/newgame.jpg", "https://youtu.be/S7MnkE_qxj8", "An exciting new game.");

            // Assert
            Assert.IsTrue(result.Success);
            Assert.IsEmpty(result.ErrorMessage);

            var game = await _dbContext.Games.FirstOrDefaultAsync(g => g.Name == "New Game");
            Assert.IsNotNull(game);
            Assert.AreEqual("New Game", game.Name);
            Assert.AreEqual(1, game.Genre.Id);
            Assert.AreEqual(49.99m, game.Price);
            Assert.AreEqual("http://example.com/newgame.jpg", game.ImageUrl);
            Assert.AreEqual("https://youtu.be/S7MnkE_qxj8", game.VideoUrl);
            Assert.AreEqual("An exciting new game.", game.Description);
        }
    }
}