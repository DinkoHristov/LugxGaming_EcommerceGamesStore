using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LugxGaming.Data.Data.Models
{
    public class UsersGames
    {
        [Required]
        [Comment("User Identifier")]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        [Comment("Game Identifier")]
        public int GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; } = null!;

        [Required]
        [Comment("Bought games quantity")]
        public int Quantity { get; set; }
    }
}