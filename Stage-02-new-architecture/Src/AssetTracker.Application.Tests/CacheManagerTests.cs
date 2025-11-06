using AssetTracker.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace AssetTracker.Application.Tests
{
    public class CacheManagerTests
    {
        private readonly Mock<ILogger<CacheManager>> _logger;

        public CacheManagerTests()
        {
            _logger = new Mock<ILogger<CacheManager>>();
        }

        ICacheManager BuildSut()
        {
            return new CacheManager(_logger.Object);
        }


        [Fact]
        public void LoadAllMarketplaceDataAsync_WhenHasGames_ReturnsGames()
        {
            // Arrange          

            // Act

            // Assert
          
        }
    }
}
