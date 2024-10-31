using LugxGaming.Services;
using LugxGaming.Tests.TestsHelpers;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Net;

namespace LugxGaming.Tests
{
    public class CurrencyServiceTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<IMemoryCache> _memoryCacheMock;
        private CurrencyService _currencyService;
        private MockHttpMessageHandler _mockHttpMessageHandler;

        [SetUp]
        public void SetUp()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _mockHttpMessageHandler = new MockHttpMessageHandler();
            var client = new HttpClient(_mockHttpMessageHandler);

            _httpClientFactoryMock
                .Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(client);

            _currencyService = new CurrencyService(_httpClientFactoryMock.Object, _memoryCacheMock.Object);
        }

        [TearDown]
        public void Dispose()
        {
            _mockHttpMessageHandler.Dispose();
        }

        [Test]
        public async Task Test_GetEthPriceInUsdAsync_CacheHit_ReturnsCachedPrice()
        {
            // Arrange
            var cachedPrice = 2000m;
            object cachedPriceObject = cachedPrice;

            _memoryCacheMock
                .Setup(mc => mc.TryGetValue("ETHPrice", out cachedPriceObject))
                .Returns(true);

            // Act
            var result = await _currencyService.GetEthPriceInUsdAsync();

            // Assert
            Assert.AreEqual(cachedPrice, result);
            _memoryCacheMock.Verify(mc => mc.TryGetValue("ETHPrice", out cachedPriceObject), Times.Once);
        }

        [Test]
        public async Task Test_GetEthPriceInUsdAsync_CacheMiss_FetchesFromApiAndCachesResult()
        {
            // Arrange
            object cachedPriceObject = null;

            _memoryCacheMock
                .Setup(mc => mc.TryGetValue("ETHPrice", out cachedPriceObject))
                .Returns(false);

            var apiPrice = 3000m;
            _mockHttpMessageHandler.ResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"ethereum\": {\"usd\": 3000}}")
            };

            var client = new HttpClient(_mockHttpMessageHandler);
            _httpClientFactoryMock
                .Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(client);

            var cacheEntryMock = Mock.Of<ICacheEntry>();
            _memoryCacheMock
                .Setup(mc => mc.CreateEntry(It.IsAny<object>()))
                .Returns(cacheEntryMock);

            // Act
            var result = await _currencyService.GetEthPriceInUsdAsync();

            // Assert
            Assert.AreEqual(apiPrice, result);
            _httpClientFactoryMock.Verify(factory => factory.CreateClient(It.IsAny<string>()), Times.Once);
            Mock.Get(_memoryCacheMock.Object).Verify(mc => mc.CreateEntry(It.IsAny<object>()), Times.Once);

            // Verify that the cache entry is set correctly
            Mock.Get(cacheEntryMock).VerifySet(ce => ce.Value = apiPrice);
            Mock.Get(cacheEntryMock).VerifySet(ce => ce.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60));
        }

        [Test]
        public void Test_GetEthPriceInUsdAsync_ApiCallFails_ThrowsHttpRequestException()
        {
            // Arrange
            object cachedPriceObject = null;

            _memoryCacheMock
                .Setup(mc => mc.TryGetValue("ETHPrice", out cachedPriceObject))
                .Returns(false);

            _mockHttpMessageHandler.ResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            var client = new HttpClient(_mockHttpMessageHandler);
            _httpClientFactoryMock
                .Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(client);

            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(async () => await _currencyService.GetEthPriceInUsdAsync());
        }
    }
}
