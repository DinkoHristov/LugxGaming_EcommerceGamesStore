using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LugxGaming.Data.Models
{
    public class Review
    {
        [Key]
        [Comment("Review Identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Game Identifier")]
        public int GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; } = null!;

        [Required]
        [Comment("User Identifier")]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        [Comment("Review rating")]
        public int Rating { get; set; }

        [Required]
        [Comment("Review comment")]
        public string Comment { get; set; } = null!;

        [Required]
        [Comment("Review created date")]
        public DateTime CreatedOn { get; set; }
    }
}