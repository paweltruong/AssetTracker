using System.ComponentModel;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public class ViewModelBase : IViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
