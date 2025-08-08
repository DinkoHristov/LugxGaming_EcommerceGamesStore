﻿using System.ComponentModel.DataAnnotations;

namespace LugxGaming.BusinessLogic.Models.Home
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

        public decimal PromoPrice { get; set; }
    }
}