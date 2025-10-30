using AssetTracker.AssetsResolver.HumbleBundle.Definitions;
using AssetTracker.Core.Models;
using AssetTracker.Core.Models.Enums;

namespace AssetTracker.AssetsResolver.HumbleBundle
{
    public class AssetItemBuilder
    {
        public static List<Asset> BuildAssetItems(WebPackBundlePageData data)
        {
            IEnumerable<TierItem?> tierItems = data.GetTierItems();
            var items = new Asset[data.BundleData.TierItemData.Count];

            Parallel.For(0, items.Length, i =>
            {
                items[i] = ConvertToAssetItem(tierItems.ElementAt(i));
            });
            return items.Where(x => x != null).ToList();
        }

        static Asset? ConvertToAssetItem(TierItem tierItem)
        {
            var assetType = GetAssetType(tierItem.ItemContentType);
            if (assetType == AssetType.Unknown)
            {
                //itgetsbetter items etc
                return null;
            }
            var defaultTag = AssetTags.GetDefaultTags(assetType);
            //TODO:new feature to specify tags to be applied on import
            var userDefinedDefaultTagsSet = new HashSet<string>();

            userDefinedDefaultTagsSet.Add(defaultTag);

            var assetItem = Asset.Create(
                name: tierItem.HumanName,
                assetType: assetType,
                tags: userDefinedDefaultTagsSet,
                imageUrl: tierItem.ResolvedPaths.FeaturedImage ?? tierItem.ResolvedPaths.FrontPageArtImgixRetina,
                developers: ConvertToDeveloperItems(tierItem.Developers),
                publishers: ConvertToPublisherItems(tierItem.Publishers),
                assetUrl: tierItem.GetAssetUrl(),
                searchKeywords: tierItem.GetNameForSearch()
                );

            return assetItem;
        }

        static Publisher[] ConvertToPublisherItems(IEnumerable<PublisherData> publishers)
        {
            if (publishers == null || !publishers.Any())
            {
                return Array.Empty<Publisher>();
            }
            var items = publishers.Select(d => new Publisher()
            {
                Name = d.Name,
                Url = d.Url

            }).ToArray();
            return items;
        }

        static Developer[] ConvertToDeveloperItems(IEnumerable<DeveloperData> developers)
        {
            if (developers == null || !developers.Any())
            {
                return Array.Empty<Developer>();
            }
            var items = developers.Select(d => new Developer()
            {
                Name = d.Name,
                Url = d.Url

            }).ToArray();
            return items;
        }

        static AssetType GetAssetType(string jsonAssetType)
        {
            return jsonAssetType switch
            {
                ItemContentTypes.Game => AssetType.Game,
                ItemContentTypes.Book => AssetType.Book,
                ItemContentTypes.Software => AssetType.Software,
                _ => AssetType.Unknown
            };
        }
    }
}
