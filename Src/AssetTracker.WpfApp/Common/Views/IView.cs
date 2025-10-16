using AssetTracker.WpfApp.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracker.WpfApp.Common.Views
{
    public interface IView<TViewModel> where TViewModel : IViewModel
    {
        TViewModel? ViewModel { get; }
    }
}
