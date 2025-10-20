using AssetTracker.Application.Services;
using AssetTracker.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.Application
{
    public class ApplicationIocConfig
    {
        public static void RegisterServices(IServiceCollection services)
        {
             services.AddSingleton<IAssetDatabase, FilesAssetDatabase>();
        }
    }
}
