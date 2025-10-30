using AssetTracker.AssetsResolver.HumbleBundle.Tests.TestData;
using AssetTracker.Core.Models;
using FluentAssertions;
using System.Reflection;

namespace AssetTracker.AssetsResolver.HumbleBundle.Tests.Helpers
{
    internal static class TestDataHelpers
    {
        public static string LoadTestHtml(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"AssetTracker.AssetsResolver.HumbleBundle.Tests.TestData.{fileName}";

            string? fullResourceNameMatchingFile = assembly.GetManifestResourceNames().FirstOrDefault(x=>x.EndsWith(fileName));
            if(string.IsNullOrEmpty(fullResourceNameMatchingFile))
                throw new FileNotFoundException($"Embedded resource ending with '{fileName}' not found. Available resources: {string.Join(", ", assembly.GetManifestResourceNames())}");
           
            using var stream = assembly.GetManifestResourceStream(fullResourceNameMatchingFile);
            if (stream == null)
                throw new FileNotFoundException($"Embedded resource '{resourceName}' not found. Available resources: {string.Join(", ", assembly.GetManifestResourceNames())}");

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static void AssertAssetItemsEqual(List<ExpectedAssetItem> expected, List<Asset> actual)
        {
            actual.Should().HaveCount(expected.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                var expectedItem = expected[i];
                var actualItem = actual[i];

                actualItem.Name.Should().Be(expectedItem.Name);
                actualItem.AssetType.Should().Be(expectedItem.Type);
                //actualItem.AssetUrl.Should().Be(expectedItem.Url);
                //actualItem.Tags.Should().BeEquivalentTo(expectedItem.Tags);
                //actualItem.Publishers.Should().BeEquivalentTo(expectedItem.Publishers);
                //actualItem.Developers.Should().BeEquivalentTo(expectedItem.Developers);
            }
        }

    }
}
