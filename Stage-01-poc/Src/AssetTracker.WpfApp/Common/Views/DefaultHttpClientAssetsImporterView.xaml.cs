using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Modules.SteamScraper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
