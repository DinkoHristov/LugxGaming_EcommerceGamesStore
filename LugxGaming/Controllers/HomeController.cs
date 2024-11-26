using LugxGaming.BusinessLogic.Interfaces;
using LugxGaming.BusinessLogic.Models.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LugxGaming.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeInterface homeService;

        public HomeController(IHomeInterface homeService)
        {
            this.homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var eachCategoryGames = await this.homeService.GetOneGameFromEachCategory();

            ViewBag.TopGames = await this.homeService.GetTopGames();
            ViewBag.TopCategories = await this.homeService.GetTopCategoriesGame();

            return View(eachCategoryGames);
        }

        public IActionResult Page404()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}