﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LugxGaming.Data.Data.Models
{
    public class Genre
    {
        public Genre()
        {
            Games = new HashSet<Game>();
        }

        [Key]
        [Comment("Genre Identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Genre name")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Game> Games { get; set; }
    }
}