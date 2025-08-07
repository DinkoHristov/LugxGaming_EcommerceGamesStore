using LugxGaming.BusinessLogic.Models.CreateGame;
using LugxGaming.Data.Data.Models;

namespace LugxGaming.BusinessLogic.Interfaces
{
    public interface ICreateGameInterface
    {
        /// <summary>
        /// Returns all genres
        /// </summary>
        /// <returns></returns>
        Task<List<GenreFormModel>> GetAllGenres();

        /// <summary>
        /// Returns all games
        /// </summary>
        /// <returns></returns>
        Task<List<GameFormModel>> GetAllGames();

        /// <summary>
        /// Returns game by given id
        /// </summary>
        /// <returns></returns>
        Task<Game> GetGameById(int id);

        /// <summary>
        /// Updates game promo price by given id and discount %
        /// </summary>
        /// <returns></returns>
        Task<(bool Success, string GameName, string ErrorMessage)> UpdatePromoPrice(int id, decimal discount);

        /// <summary>
        /// Create new game (only admin can create games)
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="genreId"></param>
        /// <param name="price"></param>
        /// <param name="imageUrl"></param>
        /// <param name="videoUrl"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<(bool Success, string ErrorMessage)> CreateGame(string gameName, int genreId, decimal price,
            string imageUrl, string videoUrl, string description);
    }
}