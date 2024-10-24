namespace LugxGaming.Models
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }

        public decimal USDGrandTotal { get; set; }

        public decimal ETHGrandTotal { get; set; }
    }
}
