using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using CustomerManagement.View.Windows;
using CustomerManagement.ViewModel;
using log4net;
using log4net.Config;
using Microsoft.Win32;

namespace CustomerManagement
{
    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));
        private readonly MainViewModel viewModel;

        public MainWindow(MainViewModel mainViewModel)
        {
            XmlConfigurator.Configure();
            InitializeComponent();
            this.viewModel = mainViewModel;
            this.DataContext = this.viewModel;
            this.Loaded += this.MainWindow_Loaded;
            log.Info("Start of application. Main window successfully initialised.");
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs args)
        {
            await this.viewModel.LoadAsync();
        }
    }
}