using AssetTracker.WpfApp.Common.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace AssetTracker.WpfApp.Common.Views
{
    /// <summary>
    /// Interaction logic for DefaultHttpClientAssetsImporterView.xaml
    /// </summary>
    public partial class DefaultHttpClientAssetsImporterView : UserControl, IScraperServiceMainView
    {
        public DefaultHttpClientAssetsImporterView()
        {
            InitializeComponent();
        }

        private void ParamPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && passwordBox.Tag is string key)
            {
                var viewModel = DataContext as DefaultHttpClientAssetsImporterViewModel;
                viewModel?.OnParameterPasswordChanged(key, passwordBox.Password);
            }
        }
    }
}
