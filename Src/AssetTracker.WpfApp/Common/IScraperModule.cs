using AssetTracker.WpfApp.Common.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracker.WpfApp.Common
{
    public interface IScraperModule
    {
        void ConfigureModule(IServiceCollection services);
        IScraperServiceView GetView(IServiceProvider serviceProvider);
    }
}
