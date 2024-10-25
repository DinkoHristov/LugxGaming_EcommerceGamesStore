using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LugxGaming.Data.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Purchases = new HashSet<UsersGames>();
            Reviews = new HashSet<Review>();
        }

        [Required]
        [Comment("User first name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Comment("User last name")]
        public string LastName { get; set; } = null!;

        public virtual ICollection<UsersGames> Purchases { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}