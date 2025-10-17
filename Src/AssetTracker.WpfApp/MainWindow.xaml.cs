using AssetTracker.WpfApp.Modules.Main.ViewModels;
using System.Windows;

namespace AssetTracker.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;
    }
}