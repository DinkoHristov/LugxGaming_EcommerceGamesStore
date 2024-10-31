using LugxGaming.Data;
using LugxGaming.Data.Models;
using LugxGaming.Services;
using LugxGaming.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LugxGaming.Tests
{
    public class Tests
    {
        private ApplicationDbContext dbContext;
        private Mock<UserManager<User>> userManager;
        private Mock<SignInManager<User>> signInManager;
        private Mock<IAccountService> accountService;

        [SetUp]
        public void Setup()
        {
            this.dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TEST").Options);

            this.userManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>());

            this.accountService = new Mock<IAccountService>();

            this.signInManager = new Mock<SignInManager<User>>();
        }

        [TearDown]
        public void TearDown()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
            }
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}