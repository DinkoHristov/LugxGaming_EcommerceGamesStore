using LugxGaming.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace LugxGaming.Models
{
    public class GameDetailsModel
    {
        [Required]
        public string GameName { get; set; }

        [Required]
        public string GameGenre { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public decimal USDPrice { get; set; }


		[Required]
		public decimal ETHPrice { get; set; }

		[Required]
        public string Description { get; set; }

        public int Quantity { get; set; }

        public List<ShopGameModel> RelatedGames { get; set; }

        public List<ReviewViewModel> Reviews { get; set; }
    }
}