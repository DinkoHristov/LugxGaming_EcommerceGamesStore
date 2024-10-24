using LugxGaming.Data.Models;

namespace LugxGaming.Models
{
    public class CartItem
    {
        public CartItem()
        {

        }

        public CartItem(Game game, int quantity)
        {
            this.GameId = game.Id;
            this.GameName = game.Name;
            this.USDPrice = game.Price;
            this.Quantity = quantity;
            this.Image = game.ImageUrl;
        }

        public int GameId { get; set; }

        public string GameName { get; set; }

        public int Quantity { get; set; }

        public decimal USDPrice { get; set; }

        public decimal ETHPrice { get; set; }

        public decimal USDTotal 
        {
            get
            {
                return this.Quantity * this.USDPrice;
            }
        }

		public decimal ETHTotal
		{
			get
			{
				return this.Quantity * this.ETHPrice;
			}
		}

		public string Image { get; set; }
    }
}
