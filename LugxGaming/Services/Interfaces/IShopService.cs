using LugxGaming.Data.Models;
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

		/// <summary>
		/// Get all reviews associated to the currently selected game
		/// </summary>
		/// <param name="gameName"></param>
		/// <returns></returns>
		Task<List<ReviewViewModel>?> GetAllReviewsAssociatedToGameAsync(string? gameName);

        /// <summary>
        /// This method writes review to the selected game
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<(bool Success, string ErrorMessage)> WriteReviewAsync(ReviewViewModel model, User? user);
	}
}