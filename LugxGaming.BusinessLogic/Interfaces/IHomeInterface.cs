using LugxGaming.BusinessLogic.Models.Home;

namespace LugxGaming.BusinessLogic.Interfaces
{
    public interface IHomeInterface
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