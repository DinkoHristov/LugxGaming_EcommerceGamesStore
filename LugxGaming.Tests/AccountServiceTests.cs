using LugxGaming.Data.Data;
using LugxGaming.Data.Data.Enums;
using LugxGaming.Data.Data.Models;
using LugxGaming.BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LugxGaming.Tests
{
    public class AccountServiceTests
    {
        private ApplicationDbContext dbContext;

        [SetUp]
        public void Setup()
        {
            this.dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TEST").Options);
        }

        [TearDown]
        public void TearDown()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Users.RemoveRange(dbContext.Users);
                this.dbContext.Games.RemoveRange(dbContext.Games);
                this.dbContext.Genres.RemoveRange(dbContext.Genres);
                this.dbContext.SaveChanges();

                this.dbContext.Dispose();
            }
        }

        [Test]
        public async Task Test_GetAllUsers()
        {
            // Arrange
            var user1 = new User { Id = "1", Email = "test1@test.com", FirstName = "John", LastName = "Doe" };
            var user2 = new User { Id = "2", Email = "test2@test.com", FirstName = "Jane", LastName = "Doe" };

            await this.dbContext.Users.AddAsync(user1);
            await this.dbContext.Users.AddAsync(user2);
            await dbContext.SaveChangesAsync();

            var accountService = new AccountService(dbContext, null, null, null, null);
            var result = await accountService.GetAllUsersAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task Test_GetAllPurchasesFromUserAsync()
        {
            // Arrange
            var genre = new Genre() { Id = 1, Name = "Action"};

            await this.dbContext.Genres.AddAsync(genre);
            await dbContext.SaveChangesAsync();

            var game1 = new Game() { Id = 1, Name = "Fallout 4", GenreId = 1, ImageUrl = "https://cdn.mos.cms.futurecdn.net/ch99eH3EBogFypt3mKGwsk-1200-80.jpg.webp", VideoUrl = "https://youtu.be/Ldb4yfUZXF4", Price = 19.99m, Description = "Fallout 4 may be bleak at times, what with the whole nuclear devastation and that, but it ultimately presents us with lands to explore that are rich in detail, populated by unforgettable characters - robot detectives! Ghoulified radio lovers! -, and ripe for the chance to build your own community on. While you wander through the scorched remains of a society devastated by an invisible enemy (in the form of radiation)." };
            var game2 = new Game() { Id = 2, Name = "A Way Out", GenreId = 1, ImageUrl = "https://cdn.mos.cms.futurecdn.net/75oM9gEUohayDJDPFiGzKn-1200-80.jpg.webp", VideoUrl = "https://youtu.be/Ldb4yfUZXF4", Price = 39.99m, Description = "It’s an inventive set-up, in which players must work with one another smartly if they are to make any kind of progress - at some point, one player will have to create a distraction so that the other can pick up an object that will aid them in their escape plan. By the end of the game, the two players helping the protagonists Leo and Vincent to go on the run will become as close as the characters they’re controlling and may even shed a tear or two as the story nears its conclusion." };

            await this.dbContext.Games.AddAsync(game1);
            await this.dbContext.Games.AddAsync(game2);
            await dbContext.SaveChangesAsync();

            var user = new User
            {
                Id = "1",
                Email = "test1@test.com",
                FirstName = "John",
                LastName = "Doe",
                Purchases = new List<UsersGames>()
                {
                    new UsersGames() {GameId = 1, UserId = "1"},
                    new UsersGames() {GameId = 2, UserId = "1"},
                }
            };

            await this.dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var accountService = new AccountService(dbContext, null, null, null, null);
            var result = await accountService.GetAllPurchasesFromUserAsync("1");

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task Test_SendEmailAsync()
        {
            var accountService = new AccountService(dbContext, null, null, null, null);
            var result = await accountService.SendEmailAsync("ivan@abv.bg", "test message", "https/confirmlink");

            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Test_EditUserAsync()
        {
            var user = new User { Id = "1", Email = "test1@test.com", FirstName = "John", LastName = "Doe" };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var accountService = new AccountService(dbContext, null, null, null, null);
            var result = await accountService.EditUserAsync("1", "edited@test.com", "ivan", "ivanov");

            Assert.AreEqual(true, result);
            Assert.AreEqual("edited@test.com", this.dbContext.Users.First().Email);
            Assert.AreEqual("ivan", this.dbContext.Users.First().FirstName);
            Assert.AreEqual("ivanov", this.dbContext.Users.First().LastName);

        }

        [Test]
        public async Task Test_DeleteUserAsync()
        {
            var user1 = new User { Id = "1", Email = "test1@test.com", FirstName = "John", LastName = "Doe" };
            var user2 = new User { Id = "2", Email = "test2@test.com", FirstName = "David", LastName = "Jackson" };

            await this.dbContext.Users.AddAsync(user1);
            await this.dbContext.Users.AddAsync(user2);
            await this.dbContext.SaveChangesAsync();

            var accountService = new AccountService(dbContext, null, null, null, null);
            var result = await accountService.DeleteUserAsync("1");

            Assert.AreEqual(true, result);
            Assert.AreEqual(1, this.dbContext.Users.Count());
        }

        [Test]
        public async Task Test_ResetEmailAsync()
        {
            var user = new User { Id = "1", Email = "test1@test.com", FirstName = "John", LastName = "Doe" };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            mockUserManager.Setup(m => m.FindByEmailAsync("test1@test.com"))
                  .ReturnsAsync((User)null); // Simulate email not found

            mockUserManager.Setup(m => m.FindByIdAsync("1"))
                           .ReturnsAsync(user); // Return our test user

            mockUserManager.Setup(m => m.GenerateChangeEmailTokenAsync(user, "newemail@test.com"))
                           .ReturnsAsync("valid-token"); // Return a fake token

            mockUserManager.Setup(m => m.ChangeEmailAsync(user, "newemail@test.com", "valid-token"))
                           .ReturnsAsync(IdentityResult.Success); // Simulate a successful email change


            var accountService = new AccountService(dbContext, mockUserManager.Object, null, null, null);
            var result = await accountService.ResetEmailAsync("1", "newemail@test.com");

            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task Test_SignInAsync_EmailNotFound()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                                                                   Mock.Of<IHttpContextAccessor>(),
                                                                   Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                                                                   null, null, null, null);

            // Set up UserManager to return null for FindByEmailAsync to simulate "Email not found"
            mockUserManager.Setup(m => m.FindByEmailAsync("unknown@test.com"))
                           .ReturnsAsync((User)null);

            var accountService = new AccountService(dbContext, mockUserManager.Object, mockSignInManager.Object, null, null);

            // Act
            var result = await accountService.SignInAsync("unknown@test.com", "password");

            // Assert
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("EmailNotFound", result.ErrorKey);
            Assert.AreEqual("Email is incorrect!", result.ErrorMessage);
        }

        [Test]
        public async Task Test_SignInAsync_EmailNotConfirmed()
        {
            // Arrange
            var user = new User { Id = "1", Email = "test1@test.com" };

            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                                                                   Mock.Of<IHttpContextAccessor>(),
                                                                   Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                                                                   null, null, null, null);

            // Set up mocks
            mockUserManager.Setup(m => m.FindByEmailAsync("test1@test.com"))
                           .ReturnsAsync(user);
            mockUserManager.Setup(m => m.IsEmailConfirmedAsync(user))
                           .ReturnsAsync(false); // Simulate unconfirmed email

            var accountService = new AccountService(dbContext, mockUserManager.Object, mockSignInManager.Object, null, null);

            // Act
            var result = await accountService.SignInAsync("test1@test.com", "password");

            // Assert
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("EmailNotConfirmed", result.ErrorKey);
            Assert.AreEqual("Email is not confirmed!", result.ErrorMessage);
        }

        [Test]
        public async Task Test_SignInAsync_IncorrectPassword()
        {
            // Arrange
            var user = new User { Id = "1", Email = "test1@test.com" };

            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                                                                   Mock.Of<IHttpContextAccessor>(),
                                                                   Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                                                                   null, null, null, null);

            // Set up mocks
            mockUserManager.Setup(m => m.FindByEmailAsync("test1@test.com"))
                           .ReturnsAsync(user);
            mockUserManager.Setup(m => m.IsEmailConfirmedAsync(user))
                           .ReturnsAsync(true); // Simulate confirmed email
            mockUserManager.Setup(m => m.CheckPasswordAsync(user, "wrongpassword"))
                           .ReturnsAsync(false); // Simulate incorrect password

            var accountService = new AccountService(dbContext, mockUserManager.Object, mockSignInManager.Object, null, null);

            // Act
            var result = await accountService.SignInAsync("test1@test.com", "wrongpassword");

            // Assert
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("IncorrectPassword", result.ErrorKey);
            Assert.AreEqual("Password is incorrect!", result.ErrorMessage);
        }

        [Test]
        public async Task Test_SignInAsync_Success()
        {
            // Arrange
            var user = new User { Id = "1", Email = "test1@test.com" };

            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object,
                                                                   Mock.Of<IHttpContextAccessor>(),
                                                                   Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                                                                   null, null, null, null);

            // Set up mocks
            mockUserManager.Setup(m => m.FindByEmailAsync("test1@test.com"))
                           .ReturnsAsync(user);
            mockUserManager.Setup(m => m.IsEmailConfirmedAsync(user))
                           .ReturnsAsync(true);
            mockUserManager.Setup(m => m.CheckPasswordAsync(user, "correctpassword"))
                           .ReturnsAsync(true); // Simulate correct password
            mockSignInManager.Setup(m => m.SignInAsync(user, true, null))
                             .Returns(Task.CompletedTask); // Simulate successful sign-in

            var accountService = new AccountService(dbContext, mockUserManager.Object, mockSignInManager.Object, null, null);

            // Act
            var result = await accountService.SignInAsync("test1@test.com", "correctpassword");

            // Assert
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(string.Empty, result.ErrorKey);
            Assert.AreEqual(string.Empty, result.ErrorMessage);
        }

        [Test]
        public async Task Test_SignUpAsync_EmailAlreadyExists_ReturnsError()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            // Configure UserManager to return a user for FindByEmailAsync, simulating that email already exists
            mockUserManager.Setup(m => m.FindByEmailAsync("existing@test.com"))
                           .ReturnsAsync(new User { Email = "existing@test.com" });

            var accountService = new AccountService(dbContext, mockUserManager.Object, null, mockUrlHelperFactory.Object, mockHttpContextAccessor.Object);

            // Act
            var result = await accountService.SignUpAsync("existing@test.com", "John", "Doe", "password123");

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("ExistingUser", result.ErrorKey);
            Assert.AreEqual("Email is already taken!", result.ErrorMessage);
        }

        [Test]
        public async Task Test_SignUpAsync_CreateUserFails_ReturnsError()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            // Set up FindByEmailAsync to return null (no existing user with this email)
            mockUserManager.Setup(m => m.FindByEmailAsync("newuser@test.com")).ReturnsAsync((User)null);

            // Simulate CreateAsync failure with specific error messages
            mockUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(), "password123"))
                           .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password is too weak" }));

            var accountService = new AccountService(dbContext, mockUserManager.Object, null, mockUrlHelperFactory.Object, mockHttpContextAccessor.Object);

            // Act
            var result = await accountService.SignUpAsync("newuser@test.com", "John", "Doe", "password123");

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("FailedCreatingUser", result.ErrorKey);
            Assert.AreEqual("Password is too weak", result.ErrorMessage);
        }

        [Test]
        public async Task Test_SignUpAsync_SuccessfulSignUp_SendsConfirmationEmail()
        {
            // Arrange
            var user = new User { Email = "newuser@test.com" };

            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockUrlHelper = new Mock<IUrlHelper>();

            // Set up UserManager mocks
            mockUserManager.Setup(m => m.FindByEmailAsync("newuser@test.com")).ReturnsAsync((User)null);
            mockUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(), "password123")).ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<User>())).ReturnsAsync("mockToken");
            mockUserManager.Setup(m => m.AddToRoleAsync(It.IsAny<User>(), Roles.User.ToString())).ReturnsAsync(IdentityResult.Success);

            // Set up URL generation
            mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns("http://testlink/confirm");
            mockUrlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(mockUrlHelper.Object);
            mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(new DefaultHttpContext());

            var accountService = new AccountService(dbContext, mockUserManager.Object, null, mockUrlHelperFactory.Object, mockHttpContextAccessor.Object);

            // Act
            var result = await accountService.SignUpAsync("newuser@test.com", "John", "Doe", "password123");

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(string.Empty, result.ErrorKey);
            Assert.AreEqual(string.Empty, result.ErrorMessage);

            // Verify email confirmation and role assignment
            mockUserManager.Verify(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()), Times.Once);
            mockUserManager.Verify(m => m.AddToRoleAsync(It.IsAny<User>(), Roles.User.ToString()), Times.Once);
        }

        [Test]
        public async Task Test_ForgotPasswordAsync_UserDoesNotExist_ReturnsError()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            mockUserManager.Setup(m => m.FindByEmailAsync("nonexistent@test.com")).ReturnsAsync((User)null);

            var accountService = new AccountService(dbContext, mockUserManager.Object, null, mockUrlHelperFactory.Object, mockHttpContextAccessor.Object);

            // Act
            var result = await accountService.ForgotPasswordAsync("nonexistent@test.com");

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("UserDoesntExist", result.ErrorKey);
            Assert.AreEqual("User with this email doesn't exist!", result.ErrorMessage);
        }

        [Test]
        public async Task Test_ForgotPasswordAsync_UserExists_SendsResetEmail()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockUrlHelper = new Mock<IUrlHelper>();

            var user = new User { Email = "existing@test.com" };
            mockUserManager.Setup(m => m.FindByEmailAsync("existing@test.com")).ReturnsAsync(user);
            mockUserManager.Setup(m => m.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("mockToken");

            mockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns("http://testlink/reset");
            mockUrlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(mockUrlHelper.Object);
            mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(new DefaultHttpContext());

            var accountService = new AccountService(dbContext, mockUserManager.Object, null, mockUrlHelperFactory.Object, mockHttpContextAccessor.Object);

            // Act
            var result = await accountService.ForgotPasswordAsync("existing@test.com");

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(string.Empty, result.ErrorKey);
            mockUserManager.Verify(m => m.GeneratePasswordResetTokenAsync(user), Times.Once);
        }

        [Test]
        public async Task Test_SetPasswordResetModelAsync_UserSignedIn_ReturnsResetModelWithUserEmailAndToken()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var user = new User { Id = "1", Email = "user@test.com" };
            mockUserManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            mockUserManager.Setup(m => m.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("newToken");

            var accountService = new AccountService(dbContext, mockUserManager.Object, null, null, null);

            // Act
            var result = await accountService.SetPasswordResetModelAsync(true, "1", "user@test.com", "initialToken");

            // Assert
            Assert.AreEqual("user@test.com", result.Email);
            Assert.AreEqual("newToken", result.Token);
        }

        [Test]
        public async Task Test_SetPasswordResetModelAsync_UserNotSignedIn_ReturnsModelWithInitialEmailAndToken()
        {
            // Arrange
            var accountService = new AccountService(dbContext, null, null, null, null);

            // Act
            var result = await accountService.SetPasswordResetModelAsync(false, "1", "initial@test.com", "initialToken");

            // Assert
            Assert.AreEqual("initial@test.com", result.Email);
            Assert.AreEqual("initialToken", result.Token);
        }

        [Test]
        public async Task Test_ResetPasswordAsync_ResetPasswordFails_ReturnsError()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var user = new User { Email = "existing@test.com" };
            mockUserManager.Setup(m => m.FindByEmailAsync("existing@test.com")).ReturnsAsync(user);
            mockUserManager.Setup(m => m.ResetPasswordAsync(user, "token", "newPassword")).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid token" }));

            var accountService = new AccountService(dbContext, mockUserManager.Object, null, null, null);

            // Act
            var result = await accountService.ResetPasswordAsync("existing@test.com", "token", "newPassword");

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("FailedCreatingUser", result.ErrorKey);
            Assert.AreEqual("Invalid token", result.ErrorMessage);
        }

        [Test]
        public async Task Test_ResetPasswordAsync_SuccessfulReset_ReturnsSuccess()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var user = new User { Email = "existing@test.com" };
            mockUserManager.Setup(m => m.FindByEmailAsync("existing@test.com")).ReturnsAsync(user);
            mockUserManager.Setup(m => m.ResetPasswordAsync(user, "token", "newPassword")).ReturnsAsync(IdentityResult.Success);

            var accountService = new AccountService(dbContext, mockUserManager.Object, null, null, null);

            // Act
            var result = await accountService.ResetPasswordAsync("existing@test.com", "token", "newPassword");

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(string.Empty, result.ErrorKey);
            Assert.AreEqual(string.Empty, result.ErrorMessage);
        }
    }
}