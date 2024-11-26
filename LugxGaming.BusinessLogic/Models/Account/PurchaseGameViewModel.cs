namespace LugxGaming.BusinessLogic.Models.Account
{
    public class PurchaseGameViewModel
    {
        public string GameName { get; set; }

        public string GameGenre { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return this.Quantity * this.Price;
            }
        }
    }
}