using AssetTracker.WpfApp.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracker.WpfApp.Common.Views
{
    public interface IScraperServiceView
    {
        IScraperServiceListItemView? ListItemView { get; }
        IScraperServiceMainView? MainView { get; }
    }

    public interface IScraperServiceView<TViewModel> : IScraperServiceView, IView<TViewModel> where TViewModel : IScraperServiceViewModel
    {
    }
}
