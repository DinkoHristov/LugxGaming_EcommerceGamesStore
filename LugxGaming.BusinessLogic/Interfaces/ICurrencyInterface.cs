namespace LugxGaming.BusinessLogic.Interfaces
{
    public interface ICurrencyInterface
    {
        /// <summary>
        /// This method gets the ETH price in USD
        /// </summary>
        /// <returns></returns>
        public Task<decimal> GetEthPriceInUsdAsync();
    }
}