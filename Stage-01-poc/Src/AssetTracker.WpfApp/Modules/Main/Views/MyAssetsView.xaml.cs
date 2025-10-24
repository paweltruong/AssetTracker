using System.Windows.Controls;

namespace AssetTracker.WpfApp.Modules.Main.Views
{
    /// <summary>
    /// Interaction logic for MyAssetsView.xaml. Binding with DataContext is done in <see cref="AssetTracker.WpfApp.Modules.Main.ViewModels.MainWindowViewModel.ExecuteNavigateCommand"/>
    /// </summary>
    public partial class MyAssetsView : UserControl
    {
        public MyAssetsView()
        {
            InitializeComponent();
        }
    }
}
