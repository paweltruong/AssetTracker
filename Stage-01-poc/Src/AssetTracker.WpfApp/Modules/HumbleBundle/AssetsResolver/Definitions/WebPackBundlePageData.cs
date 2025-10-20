using System.Text.Json.Serialization;

namespace AssetTracker.WpfApp.Modules.HumbleBundle.AssetsResolver.Definitions
{
    public class WebPackBundlePageData
    {
        public const string DeveloperSyntyStudios = "Synty Studios";


        [JsonPropertyName("bundleData")]
        public BundleData BundleData { get; set; }

        public IEnumerable<TierItem> GetTierItems()
        {
            if (BundleData == null
                || BundleData.TierItemData == null)
            {
                return Enumerable.Empty<TierItem>();
            }
            return BundleData.TierItemData.Values;
        }
    }

    /// <summary>
    /// string values for ItemContentType field in TierItem
    /// </summary>
    public static class ItemContentTypes
    {
        public const string Game = "game";
        public const string Software = "software";
        public const string Book = "book";
    }

    public class BundleData //bundle_data
    {
        [JsonPropertyName("basic_data")]
        public BasicData BasicData { get; set; }

        [JsonPropertyName("tier_item_data")]
        public Dictionary<string, TierItem> TierItemData { get; set; }
    }



    public class BasicData
    {
        /// <summary>
        /// Example: game
        /// </summary>
        [JsonPropertyName("media_type")]
        public string MediaType { get; set; }
        /// <summary>
        /// Html format with bundle description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        /// <summary>
        /// Example: ["steam"]
        /// </summary>
        [JsonPropertyName("required_account_links")]
        public List<string> RequiredAccountLinks { get; set; }
        /// <summary>
        /// Example: 2025-11-04T02:00:00
        /// </summary>
        [JsonPropertyName("end_time|datetime")]
        public DateTime EndTime { get; set; }
        [JsonPropertyName("msrp|money")]
        public Money MSRP { get; set; }
        /// <summary>
        /// Example: IGN Fan Fest 2025: Fall Edition
        /// </summary>
        [JsonPropertyName("human_name")]
        public string HumanName { get; set; }
    }

    public class Money
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        /// <summary>
        /// Example: EUR
        /// </summary>
        [JsonPropertyName("currency")]
        public string CurrencyCode { get; set; }
    }

    public class TierItem
    {
        /// <summary>
        /// Example: polygon_sci-ficitypack
        /// </summary>
        [JsonPropertyName("machine_name")]
        public string MachineName { get; set; }
        /// <summary>
        /// Example: E-bGLtv6zHY
        /// Can be used in YouTube links to identify the video like https://www.youtube.com/watch?v=E-bGLtv6zHY
        /// </summary>
        [JsonPropertyName("youtube_link")]
        public string YoutTubeLink { get; set; }

        /// <summary>
        /// Example: software
        /// </summary>
        [JsonPropertyName("item_content_type")]
        public string ItemContentType { get; set; }
        [JsonPropertyName("availability_icons")]
        public AvailabilityIcons AvailabilityIcons { get; set; }
        /// <summary>
        /// Html text with callout
        /// Example: <div class=\"accolades\">MSRP: $69.99</div>
        /// </summary>
        [JsonPropertyName("callout")]
        public string Callout { get; set; }

        [JsonPropertyName("publishers")]
        public List<PublisherData> Publishers { get; set; }
        /// <summary>
        ///  Manufacturer's Suggested Retail Price
        /// Example: 69.99
        /// </summary>
        [JsonPropertyName("msrp_price")]
        public string? MSRPPrice { get; set; }
        /// <summary>
        /// Html format This might contain ifram for soundcloud playlist
        /// </summary>
        [JsonPropertyName("description_text")]
        public string DescriptionText { get; set; }
        [JsonPropertyName("front_page_art")]
        public FrontPageArtData FrontPageArt { get; set; }
        [JsonPropertyName("developers")]
        public List<DeveloperData> Developers { get; set; }
        [JsonPropertyName("is_clickable")]
        public bool IsClickable { get; set; }
        [JsonPropertyName("resolved_paths")]
        public ResolvedPaths ResolvedPaths { get; set; }
        /// <summary>
        /// Example: POLYGON - Sci-Fi City Pack
        /// </summary>
        [JsonPropertyName("human_name")]
        public string HumanName { get; set; }

        public string GetAssetUrl()
        {
            if (Developers.Any(x =>
                x.Name.Equals(WebPackBundlePageData.DeveloperSyntyStudios,
                StringComparison.OrdinalIgnoreCase)))
                return Developers.First(x =>
                x.Name.Equals(WebPackBundlePageData.DeveloperSyntyStudios,
                StringComparison.OrdinalIgnoreCase)).Url;
            //TODO: get steam store link for game if it could be redeemed on steam (look for availability_icons)
            return "TODO";
        }
    }

    public class AvailabilityIcons
    {
        [JsonPropertyName("platform_icons")]
        public List<string> PlatformIcons { get; set; } = new();
        /// <summary>
        /// Example: {"hb-windows":"Windows","hb-linux":"Linux","hb-steam":"Steam","hb-osx":"Mac"}
        /// </summary>
        [JsonPropertyName("human_names")]
        public Dictionary<string, string> HumanNames { get; set; } = new();

        /// <summary>
        /// Example: "hb-steam": {"available": ["hb-windows", "hb-mac"],"unavailable": ["hb-linux"]}
        /// </summary>
        [JsonPropertyName("delivery_to_platform")]
        public Dictionary<string, PlatformAvailability> DeliveryToPlatform { get; set; } = new();

        [JsonPropertyName("preamble")]
        public Preamble Preamble { get; set; } = new();

        [JsonPropertyName("delivery_icons")]
        public List<string> DeliveryIcons { get; set; } = new();
        /// <summary>
        /// Example: "hb-windows": {"available": ["hb-steam"],"unavailable": []}
        /// </summary>

        [JsonPropertyName("platform_to_delivery")]
        public Dictionary<string, PlatformDelivery> PlatformToDelivery { get; set; } = new();
    }

    public class PlatformAvailability
    {
        [JsonPropertyName("available")]
        public List<string> Available { get; set; } = new();

        [JsonPropertyName("unavailable")]
        public List<string> Unavailable { get; set; } = new();
    }

    public class Preamble
    {
        [JsonPropertyName("available")]
        public string Available { get; set; } = string.Empty;

        [JsonPropertyName("unavailable")]
        public string Unavailable { get; set; } = string.Empty;

        [JsonPropertyName("hb-steam")]
        public string HbSteam { get; set; } = string.Empty;
    }

    public class PlatformDelivery
    {
        [JsonPropertyName("available")]
        public List<string> Available { get; set; } = new();

        [JsonPropertyName("unavailable")]
        public List<string> Unavailable { get; set; } = new();
    }

    public class FrontPageArtData
    {
        /// <summary>
        /// Example: images/displayitems/master_images/polygon_sci-ficitypack.png
        /// https://www.humblebundle.com/image/800x400/https://files.humblebundle.com/thumbnail/production/packaging/0e6f3f4a8f3c4f2fb6f3f4a8f3c4f2fb.png
        /// </summary>
        [JsonPropertyName("image_path")]
        public string ImagePath { get; set; }
        /// <summary>
        /// Example: POLYGON - Sci-Fi City Pack
        /// </summary>
        [JsonPropertyName("image_text")]
        public string ImageText { get; set; }
    }

    public class PublisherData
    {
        /// <summary>
        /// Example: THQ Nordic
        /// </summary>
        [JsonPropertyName("publisher-name")]
        public string Name { get; set; }
        /// <summary>
        /// Example: https://thqnordic.com/
        /// </summary>
        [JsonPropertyName("publisher-url")]
        public string Url { get; set; }
    }
    public class DeveloperData
    {
        /// <summary>
        /// Example: Synty Studios
        /// </summary>
        [JsonPropertyName("developer-name")]
        public string Name { get; set; }
        /// <summary>
        /// Example: https://syntystore.com/products/polygon-sci-fi-city
        /// </summary>
        [JsonPropertyName("developer-url")]
        public string Url { get; set; }
    }

    public class ResolvedPaths
    {
        [JsonPropertyName("front_page_art_charity_imgix")]
        public string FrontPageArtCharityImgix { get; set; }

        [JsonPropertyName("featured_image")]
        public string FeaturedImage { get; set; }

        /// <summary>
        /// Miniature image for bundle item
        /// Example: https://hb.imgix.net/e4c3ff6ecd8c1172e297715c1a3d3ae989782d0e.png?auto=compress,format&dpr=2&fit=clip&h=218&w=150&s=a1c4e4ffc335f21a9c9bbf52f40976ed
        /// </summary>
        [JsonPropertyName("front_page_art_imgix_retina")]
        public string FrontPageArtImgixRetina { get; set; }

        [JsonPropertyName("front_page_art_charity_imgix_retina")]
        public string FrontPageArtCharityImgixRetina { get; set; }

        [JsonPropertyName("front_page_art_imgix")]
        public string FrontPageArtImgix { get; set; }

        [JsonPropertyName("preview_image")]
        public string PreviewImage { get; set; }
    }
}
