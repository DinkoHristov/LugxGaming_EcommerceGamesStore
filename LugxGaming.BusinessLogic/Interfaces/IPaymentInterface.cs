using LugxGaming.BusinessLogic.Models.Payment;

namespace LugxGaming.BusinessLogic.Interfaces
{
    public interface IPaymentInterface
    {
        /// <summary>
        /// This method uses the Stripe API to pay with credit or debit card
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<(bool OK, string ErrorMessage, string SessionUrl)> PayWithCard(string amount);

        /// <summary>
        /// Save purchases of the user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cart"></param>
        /// <returns></returns>
        Task<(bool OK, string ErrorMessage)> SavePurchases(string userId, List<CartItem> cart);
    }
}