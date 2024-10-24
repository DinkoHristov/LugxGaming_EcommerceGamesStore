using LugxGaming.Data;
using LugxGaming.Data.Models;
using LugxGaming.Infrastructure;
using LugxGaming.Models;
using LugxGaming.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LugxGaming.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICurrencyService currencyService;

        public CartService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor,
                           ICurrencyService currencyService)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.currencyService = currencyService;

        }

        public async Task<(CartViewModel? CartViewModel, string EthereumAccount)> SetCartModelAsync()
        {
            var ethereumAccount = this.httpContextAccessor.HttpContext.Session.GetString("MetaMaskAccount");

            var cart = this.httpContextAccessor.HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var ethPriceInUsd = await this.currencyService.GetEthPriceInUsdAsync();

            foreach (var item in cart)
            {
                item.ETHPrice = item.USDPrice / ethPriceInUsd;
            }

            var cartModel = new CartViewModel
            {
                CartItems = cart,
                USDGrandTotal = cart.Sum(c => c.Quantity * c.USDPrice),
                ETHGrandTotal = cart.Sum(c => c.Quantity * c.ETHPrice)
            };

            return (cartModel, ethereumAccount);
        }

        public async Task<bool> AddItemToCartAsync(int quantity, string gameName)
        {
            try
            {
                var (cart, cartItem, game) = await this.GetCartItem(gameName);

                if (cartItem == null)
                {
                    cart.Add(new CartItem(game, quantity));
                }
                else
                {
                    if (quantity == 0)
                    {
                        quantity = 1;
                    }

                    cartItem.Quantity += quantity;
                }

                this.httpContextAccessor.HttpContext.Session.SetJson("Cart", cart);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DecreaseItemQuantityAsync(string gameName)
        {
            try
            {
                var (cart, cartItem, game) = await GetCartItem(gameName);

                if (cartItem != null)
                {
                    if (cartItem.Quantity > 0)
                    {
                        cartItem.Quantity -= 1;
                    }

                    if (cartItem.Quantity == 0)
                    {
                        cart.Remove(cartItem);
                    }
                }

                this.httpContextAccessor.HttpContext.Session.SetJson("Cart", cart);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemoveItemFromCartAsync(string gameName)
        {
            try
            {
                var (cart, cartItem, game) = await GetCartItem(gameName);

                if (cartItem != null)
                {
                    cart.Remove(cartItem);
                }

                this.httpContextAccessor.HttpContext.Session.SetJson("Cart", cart);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveCartItems()
        {
            try
            {
                var cart = this.httpContextAccessor.HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

                if (cart != null)
                {
                    cart = new List<CartItem>();
                }

                this.httpContextAccessor.HttpContext.Session.SetJson("Cart", cart);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<(List<CartItem>?, CartItem, Game?)> GetCartItem(string gameName)
        {
            var game = await this.dbContext.Games.FirstOrDefaultAsync(g => g.Name == gameName);

            var gameId = game.Id;

            var cart = this.httpContextAccessor.HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var cartItem = cart.Where(c => c.GameId == gameId).FirstOrDefault();

            return (cart, cartItem, game);
        }
    }
}
