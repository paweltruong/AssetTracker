using AssetTracker.Core.Services.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public class AssetsDataViewModel : ViewModelBase, IAssetsDataViewModel
    {
        IPlugin _plugin;
        public AssetsDataViewModel(IPlugin plugin)
        {
            _plugin = plugin;
        }
    }
}
