using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AssetTracker.AssetsImporter.Steam.Definitions
{
    public class SteamAppDetailsResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public SteamAppData Data { get; set; }
    }

    public class SteamAppData
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("steam_appid")]
        public int SteamAppId { get; set; }

        [JsonPropertyName("required_age")]
        public int RequiredAge { get; set; }

        [JsonPropertyName("is_free")]
        public bool IsFree { get; set; }

        [JsonPropertyName("controller_support")]
        public string ControllerSupport { get; set; }

        [JsonPropertyName("detailed_description")]
        public string DetailedDescription { get; set; }

        [JsonPropertyName("about_the_game")]
        public string AboutTheGame { get; set; }

        [JsonPropertyName("short_description")]
        public string ShortDescription { get; set; }

        [JsonPropertyName("supported_languages")]
        public string SupportedLanguages { get; set; }

        [JsonPropertyName("header_image")]
        public string HeaderImage { get; set; }

        [JsonPropertyName("capsule_image")]
        public string CapsuleImage { get; set; }

        [JsonPropertyName("capsule_imagev5")]
        public string CapsuleImageV5 { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("pc_requirements")]
        public Requirements PcRequirements { get; set; }

        [JsonPropertyName("mac_requirements")]
        public Requirements MacRequirements { get; set; }

        [JsonPropertyName("linux_requirements")]
        public Requirements LinuxRequirements { get; set; }

        [JsonPropertyName("developers")]
        public List<string> Developers { get; set; }

        [JsonPropertyName("publishers")]
        public List<string> Publishers { get; set; }

        [JsonPropertyName("price_overview")]
        public PriceOverview PriceOverview { get; set; }

        [JsonPropertyName("packages")]
        public List<int> Packages { get; set; }

        [JsonPropertyName("package_groups")]
        public List<PackageGroup> PackageGroups { get; set; }

        [JsonPropertyName("platforms")]
        public Platforms Platforms { get; set; }

        [JsonPropertyName("metacritic")]
        public Metacritic Metacritic { get; set; }

        [JsonPropertyName("categories")]
        public List<Category> Categories { get; set; }

        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; }

        [JsonPropertyName("screenshots")]
        public List<Screenshot> Screenshots { get; set; }

        [JsonPropertyName("movies")]
        public List<Movie> Movies { get; set; }

        [JsonPropertyName("recommendations")]
        public Recommendations Recommendations { get; set; }

        [JsonPropertyName("achievements")]
        public Achievements Achievements { get; set; }

        [JsonPropertyName("release_date")]
        public ReleaseDate ReleaseDate { get; set; }

        [JsonPropertyName("support_info")]
        public SupportInfo SupportInfo { get; set; }

        [JsonPropertyName("background")]
        public string Background { get; set; }

        [JsonPropertyName("background_raw")]
        public string BackgroundRaw { get; set; }

        [JsonPropertyName("content_descriptors")]
        public ContentDescriptors ContentDescriptors { get; set; }

        [JsonPropertyName("ratings")]
        public Ratings Ratings { get; set; }
    }

    public class Requirements
    {
        [JsonPropertyName("minimum")]
        public string Minimum { get; set; }

        [JsonPropertyName("recommended")]
        public string Recommended { get; set; }
    }

    public class PriceOverview
    {
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("initial")]
        public int Initial { get; set; }

        [JsonPropertyName("final")]
        public int Final { get; set; }

        [JsonPropertyName("discount_percent")]
        public int DiscountPercent { get; set; }

        [JsonPropertyName("initial_formatted")]
        public string InitialFormatted { get; set; }

        [JsonPropertyName("final_formatted")]
        public string FinalFormatted { get; set; }
    }

    public class PackageGroup
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("selection_text")]
        public string SelectionText { get; set; }

        [JsonPropertyName("save_text")]
        public string SaveText { get; set; }

        [JsonPropertyName("display_type")]
        public int DisplayType { get; set; }

        [JsonPropertyName("is_recurring_subscription")]
        public string IsRecurringSubscription { get; set; }

        [JsonPropertyName("subs")]
        public List<Subscription> Subs { get; set; }
    }

    public class Subscription
    {
        [JsonPropertyName("packageid")]
        public int PackageId { get; set; }

        [JsonPropertyName("percent_savings_text")]
        public string PercentSavingsText { get; set; }

        [JsonPropertyName("percent_savings")]
        public int PercentSavings { get; set; }

        [JsonPropertyName("option_text")]
        public string OptionText { get; set; }

        [JsonPropertyName("option_description")]
        public string OptionDescription { get; set; }

        [JsonPropertyName("can_get_free_license")]
        public string CanGetFreeLicense { get; set; }

        [JsonPropertyName("is_free_license")]
        public bool IsFreeLicense { get; set; }

        [JsonPropertyName("price_in_cents_with_discount")]
        public int PriceInCentsWithDiscount { get; set; }
    }

    public class Platforms
    {
        [JsonPropertyName("windows")]
        public bool Windows { get; set; }

        [JsonPropertyName("mac")]
        public bool Mac { get; set; }

        [JsonPropertyName("linux")]
        public bool Linux { get; set; }
    }

    public class Metacritic
    {
        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class Category
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Genre
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Screenshot
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("path_thumbnail")]
        public string PathThumbnail { get; set; }

        [JsonPropertyName("path_full")]
        public string PathFull { get; set; }
    }

    public class Movie
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonPropertyName("webm")]
        public VideoFormats Webm { get; set; }

        [JsonPropertyName("mp4")]
        public VideoFormats Mp4 { get; set; }

        [JsonPropertyName("dash_av1")]
        public string DashAv1 { get; set; }

        [JsonPropertyName("dash_h264")]
        public string DashH264 { get; set; }

        [JsonPropertyName("hls_h264")]
        public string HlsH264 { get; set; }

        [JsonPropertyName("highlight")]
        public bool Highlight { get; set; }
    }

    public class VideoFormats
    {
        [JsonPropertyName("480")]
        public string Format480 { get; set; }

        [JsonPropertyName("max")]
        public string Max { get; set; }
    }

    public class Recommendations
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Achievements
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("highlighted")]
        public List<HighlightedAchievement> Highlighted { get; set; }
    }

    public class HighlightedAchievement
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }
    }

    public class ReleaseDate
    {
        [JsonPropertyName("coming_soon")]
        public bool ComingSoon { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }
    }

    public class SupportInfo
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class ContentDescriptors
    {
        [JsonPropertyName("ids")]
        public List<int> Ids { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }
    }

    public class Ratings
    {
        [JsonPropertyName("oflc")]
        public RatingInfo Oflc { get; set; }

        [JsonPropertyName("kgrb")]
        public RatingInfo Kgrb { get; set; }

        [JsonPropertyName("usk")]
        public RatingInfo Usk { get; set; }

        [JsonPropertyName("dejus")]
        public RatingInfo Dejus { get; set; }

        [JsonPropertyName("steam_germany")]
        public RatingInfo SteamGermany { get; set; }
    }

    public class RatingInfo
    {
        [JsonPropertyName("rating")]
        public string Rating { get; set; }

        [JsonPropertyName("use_age_gate")]
        public string UseAgeGate { get; set; }

        [JsonPropertyName("required_age")]
        public string RequiredAge { get; set; }

        [JsonPropertyName("banned")]
        public string Banned { get; set; }

        [JsonPropertyName("rating_generated")]
        public string RatingGenerated { get; set; }

        [JsonPropertyName("descriptors")]
        public string Descriptors { get; set; }
    }
}
