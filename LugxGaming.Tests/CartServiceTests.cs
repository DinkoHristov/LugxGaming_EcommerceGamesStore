using LugxGaming.BusinessLogic.Interfaces;
using LugxGaming.BusinessLogic.Models.Payment;
using LugxGaming.BusinessLogic.Services;
using LugxGaming.Data.Data;
using LugxGaming.Data.Data.Models;
using LugxGaming.Tests.TestsHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LugxGaming.Tests
{
    public class CartServiceTests
    {
        private ApplicationDbContext dbContext;
        private Mock<IHttpContextAccessor> httpContextAccessorMock;
        private Mock<ICurrencyInterface> currencyServiceMock;
        private CartService cartService;
        private TestsHelpers.TestSession session;

        [SetUp]
        public void Setup()
        {
            // Initialize in-memory database
            dbContext = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("TEST")
                    .Options
            );

            // Mock IHttpContextAccessor
            httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            session = new TestSession();
            var httpContext = new DefaultHttpContext { Session = session };
            httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            // Mock ICurrencyService to return a fixed ETH price for testing
            currencyServiceMock = new Mock<ICurrencyInterface>();
            currencyServiceMock.Setup(s => s.GetEthPriceInUsdAsync()).ReturnsAsync(4000m); // Sample ETH price

            // Initialize CartService
            cartService = new CartService(dbContext, httpContextAccessorMock.Object, currencyServiceMock.Object);

            // Seed the database with a test game
            dbContext.Games.Add(new Game { Id = 1, Name = "Test Game", Price = 10, Description = "Test Description", ImageUrl = "Image/Url" });
            dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Database.EnsureDeleted();

                this.dbContext.Dispose();
            }
        }

        [Test]
        public async Task Test_SetCartModelAsync_ShouldCalculateCartTotals()
        {
            // Arrange
            TestsHelpers.SessionExtensions.SetString(session, "MetaMaskAccount", "0x123");
            var cartItems = new List<CartItem>
            {
                new CartItem { GameName = "Game 1", USDPrice = 60m, Quantity = 2 },
                new CartItem { GameName = "Game 2", USDPrice = 40m, Quantity = 1 }
            };
            TestsHelpers.SessionExtensions.SetJson(session, "Cart", cartItems);

            // Act
            var (cartViewModel, ethereumAccount) = await cartService.SetCartModelAsync();

            // Assert
            Assert.IsNotNull(cartViewModel);
            Assert.AreEqual("0x123", ethereumAccount);
            Assert.AreEqual(160m, cartViewModel.USDGrandTotal);
            Assert.AreEqual(0.04m, cartViewModel.ETHGrandTotal); // 160 USD / 4000 ETH price
        }

        [Test]
        public async Task Test_AddItemToCartAsync_ShouldAddNewItem()
        {
            // Arrange
            string gameName = "Test Game";
            int quantity = 2;

            // Act
            var result = await cartService.AddItemToCartAsync(quantity, gameName);

            // Assert
            Assert.IsTrue(result);
            var cart = TestsHelpers.SessionExtensions.GetJson<List<CartItem>>(session, "Cart");
            Assert.AreEqual(1, cart.Count);
            Assert.AreEqual(gameName, cart[0].GameName);
            Assert.AreEqual(quantity, cart[0].Quantity);
            Assert.AreEqual(1, cart[0].GameId); // Ensure the GameId matches the game in the database
        }

        [Test]
        public async Task Test_AddItemToCartAsync_ShouldIncreaseQuantityIfItemExists()
        {
            // Arrange
            string gameName = "Test Game";
            int initialQuantity = 1;
            int additionalQuantity = 2;

            // Pre-populate the session cart with an item matching the test game
            TestsHelpers.SessionExtensions.SetJson(session, "Cart", new List<CartItem>
            {
                new CartItem { GameId = 1, GameName = gameName, Quantity = initialQuantity }
            });

            // Act
            var result = await cartService.AddItemToCartAsync(additionalQuantity, gameName);

            // Assert
            Assert.IsTrue(result);
            var cart = TestsHelpers.SessionExtensions.GetJson<List<CartItem>>(session, "Cart");
            Assert.AreEqual(1, cart.Count);
            Assert.AreEqual(initialQuantity + additionalQuantity, cart[0].Quantity); // Expected: 1 + 2 = 3
        }

        [Test]
        public async Task Test_DecreaseItemQuantityAsync_ShouldDecreaseQuantity()
        {
            // Arrange
            string gameName = "Test Game";
            int initialQuantity = 2;

            // Pre-populate the session cart with an item
            TestsHelpers.SessionExtensions.SetJson(session, "Cart", new List<CartItem>
            {
                new CartItem { GameId = 1, GameName = gameName, Quantity = initialQuantity }
            });

            // Act
            var result = await cartService.DecreaseItemQuantityAsync(gameName);

            // Assert
            Assert.IsTrue(result);
            var cart = TestsHelpers.SessionExtensions.GetJson<List<CartItem>>(session, "Cart");
            Assert.AreEqual(1, cart.Count);
            Assert.AreEqual(initialQuantity - 1, cart[0].Quantity); // Expected: 2 - 1 = 1
        }

        [Test]
        public async Task Test_DecreaseItemQuantityAsync_ShouldRemoveItemIfQuantityReachesZero()
        {
            // Arrange
            string gameName = "Test Game";
            int initialQuantity = 1;

            // Pre-populate the session cart with an item
            TestsHelpers.SessionExtensions.SetJson(session, "Cart", new List<CartItem>
            {
                new CartItem { GameId = 1, GameName = gameName, Quantity = initialQuantity }
            });

            // Act
            var result = await cartService.DecreaseItemQuantityAsync(gameName);

            // Assert
            Assert.IsTrue(result);
            var cart = TestsHelpers.SessionExtensions.GetJson<List<CartItem>>(session, "Cart");
            Assert.AreEqual(0, cart.Count); // Item should be removed
        }

        [Test]
        public async Task Test_RemoveItemFromCartAsync_ShouldRemoveItem()
        {
            // Arrange
            string gameName = "Test Game";

            // Pre-populate the session cart with an item
            TestsHelpers.SessionExtensions.SetJson(session, "Cart", new List<CartItem>
            {
                new CartItem { GameId = 1, GameName = gameName, Quantity = 1 }
            });

            // Act
            var result = await cartService.RemoveItemFromCartAsync(gameName);

            // Assert
            Assert.IsTrue(result);
            var cart = TestsHelpers.SessionExtensions.GetJson<List<CartItem>>(session, "Cart");
            Assert.AreEqual(0, cart.Count); // Item should be removed
        }

        [Test]
        public void Test_RemoveCartItems_ShouldClearAllItemsInCart()
        {
            // Arrange
            TestsHelpers.SessionExtensions.SetJson(session, "Cart", new List<CartItem>
            {
                new CartItem { GameId = 1, GameName = "Test Game", Quantity = 2 },
                new CartItem { GameId = 2, GameName = "Another Game", Quantity = 1 }
            });

            // Act
            var result = cartService.RemoveCartItems();

            // Assert
            Assert.IsTrue(result);
            var cart = TestsHelpers.SessionExtensions.GetJson<List<CartItem>>(session, "Cart");
            Assert.IsEmpty(cart); // The cart should be empty after removal
        }
    }
}