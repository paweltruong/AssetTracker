namespace AssetTracker.Core.Services.Plugins
{
    public interface IAssetsImporterPlugin : IPlugin
    {
        public bool UseDefaultBrowserLayout { get; }
        string DisplayName { get; }
        string Description { get; }
        string IconUrl { get; }
        string ImportDescription { get; }
        string ImportSourceUrl { get; }
    }
}
