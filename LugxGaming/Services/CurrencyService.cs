using LugxGaming.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace LugxGaming.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IMemoryCache memoryCache;

        public CurrencyService(IHttpClientFactory clientFactory, IMemoryCache memoryCache)
        {
            this.clientFactory = clientFactory;
            this.memoryCache = memoryCache;
        }

        public async Task<decimal> GetEthPriceInUsdAsync()
        {
            // Check if the ETH price is already cached
            if (!memoryCache.TryGetValue("ETHPrice", out decimal ethPrice))
            {
                // Not in cache, so fetch from API
                ethPrice = await FetchEthPriceFromApi();

                // Set cache options
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(60)); 

                // Save data in cache
                memoryCache.Set("ETHPrice", ethPrice, cacheOptions);
            }

            return ethPrice;
        }

        private async Task<decimal> FetchEthPriceFromApi()
        {
            try
            {
                var client = clientFactory.CreateClient();
                var response = await client.GetStringAsync("https://api.coingecko.com/api/v3/simple/price?ids=ethereum&vs_currencies=usd");
                var ethPrice = JsonConvert.DeserializeObject<dynamic>(response).ethereum.usd;
                return (decimal)ethPrice;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Failed to fetch ETH price", ex);
            }
        }
    }
}
