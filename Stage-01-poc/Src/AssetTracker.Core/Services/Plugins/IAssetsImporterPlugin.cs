using System.Diagnostics;

namespace AssetTracker.Core.Services.Plugins
{
    public interface IAssetsImporterPlugin : IPlugin
    {
        /// <summary>
        /// Used when importing will have to use browser and cookies to retrieve asset info f.e browse marketplace web site with paginations like SyntyStore
        /// </summary>
        public bool UseDefaultBrowserLayout { get; }
        /// <summary>
        /// Used when importing will have to use httpClient and some parameters to retrieve asset info f.e API call like SteamAPI
        /// keys that will be used in Query params in <see cref="ImportApiUrl"/> and description as value in the dictionary
        /// </summary>
        public bool UseDefaultHttpClientLayout { get; }
        public Dictionary<string,string> UseHttpClientCallParams { get; }
        string DisplayName { get; }
        string Description { get; }
        string IconUrl { get; }
        string ImportDescription { get; }
        /// <summary>
        /// Login page for <see cref="UseDefaultBrowserLayout" or API call <see cref="ImportApiUrl"/>/>
        /// </summary>
        string ImportSourceUrl { get; }
        /// <summary>
        /// could be formatted with keys from <see cref="UseHttpClientCallParams"/>
        /// </summary>
        string ImportApiUrl { get; }
        /// <summary>
        /// GET / POST
        /// </summary>
        string ImportApiCallMethod { get; }
    }
}
