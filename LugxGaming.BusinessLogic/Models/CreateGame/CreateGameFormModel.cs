﻿using System.ComponentModel.DataAnnotations;

namespace LugxGaming.BusinessLogic.Models.CreateGame
{
    public class CreateGameFormModel
    {
        [Required]
        public string GameName { get; set; }

        public int GenreId { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Required]
        [Url]
        public string VideoUrl { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        public IEnumerable<GenreFormModel>? Genres { get; set; }
    }
}