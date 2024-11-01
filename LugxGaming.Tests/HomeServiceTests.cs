using LugxGaming.Data;
using LugxGaming.Data.Models;
using LugxGaming.Services;
using Microsoft.EntityFrameworkCore;

namespace LugxGaming.Tests
{
    public class HomeServiceTests
    {
        private ApplicationDbContext dbContext;
        private HomeService homeService;

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

            // Initialize HomeService
            homeService = new HomeService(dbContext);
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

        private void SeedDatabase()
        {
            dbContext.Genres.AddRange(
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Adventure" },
                new Genre { Id = 3, Name = "Strategy" },
                new Genre { Id = 4, Name = "Racing" },
                new Genre { Id = 5, Name = "Sport" }
            );
            dbContext.Games.AddRange(
                new Game { Id = 1, Name = "Game 1", Price = 10, Description = "Description 1", ImageUrl = "Image/Url1", GenreId = 1 },
                new Game { Id = 2, Name = "Game 2", Price = 20, Description = "Description 2", ImageUrl = "Image/Url2", GenreId = 2 },
                new Game { Id = 3, Name = "Game 3", Price = 30, Description = "Description 3", ImageUrl = "Image/Url3", GenreId = 3 },
                new Game { Id = 4, Name = "Game 4", Price = 40, Description = "Description 4", ImageUrl = "Image/Url4", GenreId = 4 },
                new Game { Id = 5, Name = "Game 5", Price = 50, Description = "Description 5", ImageUrl = "Image/Url5", GenreId = 5 },
                new Game { Id = 6, Name = "Game 6", Price = 60, Description = "Description 6", ImageUrl = "Image/Url6", GenreId = 1 },
                new Game { Id = 7, Name = "Game 7", Price = 70, Description = "Description 7", ImageUrl = "Image/Url7", GenreId = 2 }
            );
            dbContext.SaveChanges();
        }

        [Test]
        public async Task Test_GetTopGames_ShouldReturnTop6Games()
        {
            // Act
            var result = await homeService.GetTopGames();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count);
        }

        [Test]
        public async Task Test_GetTopGames_ShouldReturnCorrectData()
        {
            // Act
            var result = await homeService.GetTopGames();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count);

            // Check if the first game matches the expected data
            var firstGame = result.First();
            Assert.AreEqual("Game 1", firstGame.GameName);
            Assert.AreEqual("Image/Url1", firstGame.Image);
            Assert.AreEqual("Action", firstGame.GenreName);
            Assert.AreEqual(10, firstGame.Price);
        }

        [Test]
        public async Task Test_GetOneGameFromEachCategory_ShouldReturnCorrectData()
        {
            var result = await homeService.GetOneGameFromEachCategory();

            Assert.NotNull(result);
            Assert.AreEqual(5, result.Count);
        }

        [Test]
        public async Task Test_GetTopCategoriesGame_ShouldReturnCorrectData()
        {
            var result = await homeService.GetTopCategoriesGame();

            Assert.NotNull(result);
            Assert.AreEqual(5, result.Count);
        }
    }
}
