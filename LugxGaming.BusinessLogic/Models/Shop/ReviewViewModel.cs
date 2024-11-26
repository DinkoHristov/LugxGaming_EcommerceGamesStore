using System.ComponentModel.DataAnnotations;

namespace LugxGaming.BusinessLogic.Models.Shop
{
    public class ReviewViewModel
    {
        [Required]
        public string GameName { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        [Range(0, 5)]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; } = null!;

        [Required]
        public string CreatedOn { get; set; } = null!;
    }
}