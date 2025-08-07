using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LugxGaming.Data.Data.Models
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
        [Url]
        [Comment("Game video trailer url")]
        public string VideoUrl { get; set; } = null!;

        [Required]
        [Precision(18, 2)]
        [Comment("Game price")]
        public decimal Price { get; set; }

        [Required]
        [Comment("Game description")]
        public string Description { get; set; } = null!;

        [Comment("Game discount percentage")]
        public decimal Discount { get; set; }

        [Precision(18, 2)]
        [Comment("Game promotion price")]
        public decimal PromoPrice { get; set; }

        public virtual ICollection<UsersGames> Users { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}