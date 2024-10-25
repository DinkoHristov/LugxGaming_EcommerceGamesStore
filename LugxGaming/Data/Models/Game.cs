using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LugxGaming.Data.Models
{
    public class Game
    {
        public Game()
        {
            Users = new HashSet<UsersGames>();
            Reviews = new HashSet<Review>();
        }

        [Key]
        [Comment("Game Identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Game name")]
        public string Name { get; set; } = null!;

        [Required]
        [Comment("Genre Identifier")]
        public int GenreId { get; set; }

        [ForeignKey(nameof(GenreId))]
        public virtual Genre Genre { get; set; } = null!;

        [Required]
        [Url]
        [Comment("Game image url")]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Precision(18, 5)]
        [Comment("Game price")]
        public decimal Price { get; set; }

        [Required]
        [Comment("Game description")]
        public string Description { get; set; } = null!;

        public virtual ICollection<UsersGames> Users { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}