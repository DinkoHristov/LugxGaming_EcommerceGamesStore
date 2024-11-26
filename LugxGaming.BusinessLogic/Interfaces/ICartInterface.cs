using LugxGaming.BusinessLogic.Models.Cart;

namespace LugxGaming.BusinessLogic.Interfaces
{
    public interface ICartInterface
    {
        /// <summary>
        /// Sets the cart model
        /// </summary>
        /// <returns></returns>
        Task<(CartViewModel? CartViewModel, string EthereumAccount)> SetCartModelAsync();

        /// <summary>
        /// Add item to the shopping cart
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="gameName"></param>
        /// <returns></returns>
        Task<bool> AddItemToCartAsync(int quantity, string gameName);

        /// <summary>
        /// Decrease selected item quantity
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        Task<bool> DecreaseItemQuantityAsync(string gameName);

        /// <summary>
        /// Remove the selected item form the shopping cart
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        Task<bool> RemoveItemFromCartAsync(string gameName);

        /// <summary>
        /// Remove all items from the shopping cart
        /// </summary>
        /// <returns></returns>
        bool RemoveCartItems();
    }
}