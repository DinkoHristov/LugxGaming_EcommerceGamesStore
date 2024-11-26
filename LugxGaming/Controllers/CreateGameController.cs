using LugxGaming.BusinessLogic.Interfaces;
using LugxGaming.BusinessLogic.Models.CreateGame;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LugxGaming.Controllers
{
    public class CreateGameController : Controller
    {
        private readonly ICreateGameInterface createGameService;

        public CreateGameController(ICreateGameInterface createGameService)
        {
            this.createGameService = createGameService;
        }

        public async Task<IActionResult> Create()
        {
            var genres = new CreateGameFormModel() { Genres = await this.createGameService.GetAllGenres() };

            return View(genres);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateGameFormModel model) 
        {
            model.Genres = await this.createGameService.GetAllGenres();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, errorMessage) = await this.createGameService.CreateGame(model.GameName, model.GenreId,
                model.Price, model.ImageUrl, model.Description);

            if (!success)
            {
                ViewBag.Exists = true;

                return View(model);
            }

            ViewBag.Created = true;

            ModelState.Clear();

            return View(model);
        }
    }
}