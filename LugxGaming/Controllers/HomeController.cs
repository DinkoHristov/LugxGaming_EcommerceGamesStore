﻿using LugxGaming.Data;
using LugxGaming.Models;
using LugxGaming.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LugxGaming.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext dbContext;
        private readonly IHomeService homeService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IHomeService homeService)
        {
            _logger = logger;
            this.dbContext = context;
            this.homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var eachCategoryGames = await this.homeService.GetOneGameFromEachCategory();

            ViewBag.TopGames = await this.homeService.GetTopGames();
            ViewBag.TopCategories = await this.homeService.GetTopCategoriesGame();

            return View(eachCategoryGames);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
