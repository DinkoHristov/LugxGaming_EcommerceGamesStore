using LugxGaming.Data;
using LugxGaming.Data.Models;
using LugxGaming.Models;
using LugxGaming.Services;
using LugxGaming.Tests.TestsHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace LugxGaming.Tests
{
    public class PaymentServiceTests
    {
        private ApplicationDbContext dbContext;
        private Mock<IHttpContextAccessor> httpContextAccessorMock;
        private Mock<IOptions<StripeSettings>> stripeSettingsMock;
        private PaymentService paymentService;
        private TestSession session;

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

            // Mock IHttpContextAccessor
            httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            session = new TestSession();
            var httpContext = new DefaultHttpContext { Session = session };
            httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            // Mock StripeSettings
            stripeSettingsMock = new Mock<IOptions<StripeSettings>>();
            stripeSettingsMock.Setup(s => s.Value).Returns(new StripeSettings { SecretKey = "test_api_key" });

            // Initialize PaymentService
            paymentService = new PaymentService(stripeSettingsMock.Object, dbContext, httpContextAccessorMock.Object);
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
            var user = new User { Id = "user1", UserName = "testuser", FirstName = "testFirstName", LastName = "testLastName" };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        [Test]
        public async Task Test_PayWithCard_ShouldReturnFailedWithSessionUrl()
        {
            // Arrange
            session.SetJson("Cart", new List<CartItem> { new CartItem { GameId = 1, GameName = "Test Game", Quantity = 1, USDPrice = 100 } });
            var amount = "100.00";

            // Act
            var (OK, ErrorMessage, SessionUrl) = await paymentService.PayWithCard(amount);

            // Assert
            Assert.IsFalse(OK);
            Assert.IsNotEmpty(ErrorMessage);
            Assert.IsNotNull(SessionUrl);
        }

        [Test]
        public async Task Test_PayWithCard_ShouldReturnFailureWithErrorMessage()
        {
            // Arrange
            session.SetJson("Cart", new List<CartItem> { new CartItem { GameId = 1, GameName = "Test Game", Quantity = 1, USDPrice = 100 } });
            var amount = "invalid_amount";

            // Act
            var (OK, ErrorMessage, SessionUrl) = await paymentService.PayWithCard(amount);

            // Assert
            Assert.IsFalse(OK);
            Assert.IsNotEmpty(ErrorMessage);
            Assert.AreEqual("https://localhost:7069/Payment/PaymentFailed", SessionUrl);
        }

        [Test]
        public async Task Test_SavePurchases_ShouldSavePurchasesSuccessfully()
        {
            // Arrange
            var userId = "user1";
            var cart = new List<CartItem> { new CartItem { GameId = 1, GameName = "Test Game", Quantity = 1, USDPrice = 100 } };

            // Act
            var (OK, ErrorMessage) = await paymentService.SavePurchases(userId, cart);

            // Assert
            Assert.IsTrue(OK);
            Assert.IsEmpty(ErrorMessage);
            var user = await dbContext.Users.Include(u => u.Purchases).FirstOrDefaultAsync(u => u.Id == userId);
            Assert.IsNotNull(user);
            Assert.AreEqual(1, user.Purchases.Count);
            Assert.AreEqual(1, user.Purchases.First().GameId);
            Assert.AreEqual(1, user.Purchases.First().Quantity);
        }

        [Test]
        public async Task Test_SavePurchases_ShouldReturnErrorIfUserNotFound()
        {
            // Arrange
            var userId = "non_existent_user";
            var cart = new List<CartItem> { new CartItem { GameId = 1, GameName = "Test Game", Quantity = 1, USDPrice = 100 } };

            // Act
            var (OK, ErrorMessage) = await paymentService.SavePurchases(userId, cart);

            // Assert
            Assert.IsFalse(OK);
            Assert.AreEqual("User not found", ErrorMessage);
        }
    }
}
