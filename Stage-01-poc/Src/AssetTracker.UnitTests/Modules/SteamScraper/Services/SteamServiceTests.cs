using AssetTracker.Application.Services;
using AssetTracker.UnitTests.Modules.SteamScraper.Helpers;
using AssetTracker.WpfApp.Modules.SteamScraper.Services;
using FluentAssertions;
using Moq;
using System.Net;

namespace AssetTracker.UnitTests.Modules.SteamScraper.Services
{
    public class SteamServiceTests
    {
        private readonly Mock<IMyHttpClient> _httpClient;

        public SteamServiceTests()
        {
            _httpClient = new Mock<IMyHttpClient>();
        }

        ISteamService BuildSut()
        {
            return new SteamService(_httpClient.Object);
        }


        [Fact]
        public async Task GetSteamGamesAsync_ValidSteamId_ReturnsGames()
        {
            // Arrange
            var steamApiKey = "123412345678901234567";
            var steamId = "12345678901234567";
            var expectedGames = TestDataHelper.CreateTestGames();
            var apiResponse = TestDataHelper.CreateSteamApiResponse();
            string requestUrl = TestDataHelper.GetSteamGamesRequestUrl(steamApiKey, steamId);

            _httpClient.Setup(x => x.GetAsync(requestUrl, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(apiResponse)
                });

            var sut = BuildSut();

            // Act
            var result = await sut.GetSteamGamesAsync(steamApiKey, steamId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(expectedGames.Count);
            for (int i = 0; i < expectedGames.Count; ++i)
            {
                result[i].Name.Should().Be(expectedGames[i].Name);
                result[i].AppId.Should().Be(expectedGames[i].AppId);
            }
        }
    }
}
