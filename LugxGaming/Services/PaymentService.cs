using LugxGaming.Data;
using LugxGaming.Data.Models;
using LugxGaming.Infrastructure;
using LugxGaming.Models;
using LugxGaming.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace LugxGaming.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly StripeSettings stripe;
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        public string SessionId { get; set; }

        public PaymentService(IOptions<StripeSettings> stripeSettings, ApplicationDbContext dbContext,
                              IHttpContextAccessor httpContextAccessor)
        {
            this.stripe = stripeSettings.Value;
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool OK, string ErrorMessage, string SessionUrl)> PayWithCard(string amount)
        {
            try
            {
                var cart = this.httpContextAccessor.HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

                var currency = "usd";
                var successUrl = "https://localhost:7069/Payment/PaymentSuccess";
                var cancelUrl = "https://localhost:7069/Payment/PaymentFailed";
                StripeConfiguration.ApiKey = this.stripe.SecretKey;

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                    LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = currency,
                            UnitAmountDecimal = Convert.ToDecimal(amount) * 100,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Games",
                                Description = "Games for PC"
                            }
                        },
                        Quantity = 1
                    }
                },
                    Mode = "payment",
                    SuccessUrl = successUrl,
                    CancelUrl = cancelUrl
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);
                SessionId = session.Id;

                this.httpContextAccessor.HttpContext.Session.SetJson("Cart", cart);

                return (true, string.Empty, session.Url);
            }
            catch (Exception ex)
            {
                var cancelUrl = "https://localhost:7069/Payment/PaymentFailed";
                return (false, ex.Message, cancelUrl);
            }
        }

        public async Task<(bool OK, string ErrorMessage)> SavePurchases(string userId, List<CartItem> cart)
        {
            var user = await this.dbContext.Users.Include(u => u.Purchases).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return (false, "User not found");
            }

            foreach (var item in cart)
            {
                var usersGames = new UsersGames
                {
                    UserId = userId,
                    GameId = item.GameId,
                    Quantity = item.Quantity
                };

                if (user.Purchases.Any(g => g.GameId == item.GameId))
                {
                    var game = user.Purchases.FirstOrDefault(g => g.GameId == item.GameId);

                    game.Quantity += item.Quantity;

                    continue;
                }

                user.Purchases.Add(usersGames);
            }

            await this.dbContext.SaveChangesAsync();

            return (true, string.Empty);
        }
    }
}
