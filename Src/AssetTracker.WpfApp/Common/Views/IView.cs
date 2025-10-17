using AssetTracker.WpfApp.Common.ViewModels;

namespace AssetTracker.WpfApp.Common.Views
{
    public interface IView<TViewModel> where TViewModel : IViewModel
    {
        TViewModel? ViewModel { get; }
    }
}
