using AssetTracker.WpfApp.Common.ViewModels;
using System.Windows.Controls;

namespace AssetTracker.WpfApp.Common.Views
{
    /// <summary>
    /// Interaction logic for AssetsDataView.xaml
    /// </summary>
    public partial class AssetsDataView : UserControl, IScraperServiceMainView
    {
        public AssetsDataView()
        {
            InitializeComponent();
        }
    }
}
