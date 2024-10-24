using System.ComponentModel.DataAnnotations;

namespace LugxGaming.Models
{
    public class ShopGameModel
    {
        [Required]
        public string GameName { get; set; }

        [Required]
        public string GameGenre { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
