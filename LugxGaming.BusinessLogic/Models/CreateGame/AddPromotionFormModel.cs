using System.ComponentModel.DataAnnotations;

namespace LugxGaming.BusinessLogic.Models.CreateGame
{
    public class AddPromotionFormModel
    {
        [Required]
        public int GameId { get; set; }

        [Required]
        public decimal DiscountAmount { get; set; }

        public IEnumerable<GameFormModel>? Games { get; set; }
    }
}
