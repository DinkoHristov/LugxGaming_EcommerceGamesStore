namespace LugxGaming.Services.Interfaces
{
    public interface ICurrencyService
    {
        /// <summary>
        /// This method gets the ETH price in USD
        /// </summary>
        /// <returns></returns>
        public Task<decimal> GetEthPriceInUsdAsync();
    }
}
