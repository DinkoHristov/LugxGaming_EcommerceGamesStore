using LugxGaming.Models;

namespace LugxGaming.Services.Interfaces
{
    public interface IHomeService
	{
        /// <summary>
        /// This method gets one game from each category
        /// </summary>
        /// <returns></returns>
        Task<List<HomeGameModel>> GetOneGameFromEachCategory();

        /// <summary>
        /// This method gets top 6 games
        /// </summary>
        /// <returns></returns>
        Task<List<HomeGameModel>?> GetTopGames();

		/// <summary>
		/// This method gets top categories and one game from each
		/// </summary>
		/// <returns></returns>
		Task<List<HomeGameModel>?> GetTopCategoriesGame();
	}
}
