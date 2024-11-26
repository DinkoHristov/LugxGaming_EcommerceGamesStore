using LugxGaming.BusinessLogic.Models.CreateGame;

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
        /// Create new game (only admin can create games)
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="genreId"></param>
        /// <param name="price"></param>
        /// <param name="imageUrl"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<(bool Success, string ErrorMessage)> CreateGame(string gameName, int genreId, decimal price,
                                                             string imageUrl, string description);
    }
}