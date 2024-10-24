using System.ComponentModel.DataAnnotations;

namespace LugxGaming.Models
{
	public class HomeGameModel
	{
        [Required]
        public string Image { get; set; }

        [Required]
        public string GameName { get; set; }

        [Required]
        public string GenreName { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
