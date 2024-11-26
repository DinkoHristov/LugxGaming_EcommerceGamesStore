using LugxGaming.BusinessLogic.Models.Payment;

namespace LugxGaming.BusinessLogic.Models.Cart
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }

        public decimal USDGrandTotal { get; set; }

        public decimal ETHGrandTotal { get; set; }
    }
}