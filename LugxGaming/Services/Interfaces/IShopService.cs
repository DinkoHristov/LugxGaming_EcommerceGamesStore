using LugxGaming.Models;

namespace LugxGaming.Services.Interfaces
{
	public interface IShopService
	{
		/// <summary>
		/// This method gets all games from the database
		/// </summary>
		/// <returns></returns>
		Task<List<ShopGameModel>> GetAllGames();

		/// <summary>
		/// This method gets all games from the given Genre
		/// </summary>
		/// <param name="gameGenre"></param>
		/// <returns></returns>
		Task<List<ShopGameModel>> GetSearchedGames(string gameGenre);

		/// <summary>
		/// This method gets the game with the given Name
		/// </summary>
		/// <param name="gameName"></param>
		/// <returns></returns>
		Task<GameDetailsModel?> GetGame(string? gameName);

		/// <summary>
		/// This method returns first game from the database
		/// </summary>
		/// <returns></returns>
		Task<GameDetailsModel?> GetFirstGame();

        /// <summary>
        /// This method will load the related games to the current selected game
        /// </summary>
        /// <param name="gameGenre"></param>
        /// <param name="gameName"></param>
        /// <returns></returns>
        Task<List<ShopGameModel>> FillRelatedGames(string gameGenre, string gameName);
	}
}
