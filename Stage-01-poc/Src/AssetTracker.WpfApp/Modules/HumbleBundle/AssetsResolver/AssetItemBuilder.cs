using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Models.Enums;
using AssetTracker.WpfApp.Modules.HumbleBundle.AssetsResolver.Definitions;

namespace AssetTracker.WpfApp.Modules.HumbleBundle.AssetsResolver
{
    public class AssetItemBuilder
    {

        public static List<AssetItem> BuildAssetItems(WebPackBundlePageData data)
        {
            IEnumerable<TierItem?> tierItems = data.GetTierItems();
            var items = new AssetItem[data.BundleData.TierItemData.Count];

            Parallel.For(0, items.Length, i =>
            {
                items[i] = ConvertToAssetItem(tierItems.ElementAt(i));
            });
            return items.Where(x=>x != null).ToList();
        }

        static AssetItem? ConvertToAssetItem(TierItem tierItem)
        {
            var assetType = GetAssetType(tierItem.ItemContentType);
            if(assetType == AssetType.Unknown)
            {
                //itgetsbetter items etc
                return null;
            }
            var defaultTag = Tags.GetDefaultTags(assetType);
            //TODO:new feature to specify tags to be applied on import
            var userDefinedDefaultTagsSet = new HashSet<string>();

            userDefinedDefaultTagsSet.Add(defaultTag);

            var assetItem = new AssetItem()
            {
                Name = tierItem.HumanName,
                AssetType = assetType,
                Tags = userDefinedDefaultTagsSet,
                ImageUrl = tierItem.ResolvedPaths.FrontPageArtImgixRetina,
                Developers = ConvertToDeveloperItems(tierItem.Developers),
                Publishers = ConvertToPublisherItems(tierItem.Publishers),
                AssetUrl = tierItem.GetAssetUrl()

            }; ;
            return assetItem;
        }

        static PublisherItem[] ConvertToPublisherItems(IEnumerable<PublisherData> publishers)
        {
            if (publishers == null || !publishers.Any())
            {
                return Array.Empty<PublisherItem>();
            }
            var items = publishers.Select(d => new PublisherItem()
            {
                Name = d.Name,
                Url = d.Url

            }).ToArray();
            return items;
        }

        static DeveloperItem[] ConvertToDeveloperItems(IEnumerable<DeveloperData> developers)
        {
            if (developers == null || !developers.Any())
            {
                return Array.Empty<DeveloperItem>();
            }
            var items = developers.Select(d => new DeveloperItem()
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
