using System.Windows;
using CustomerManagement.ViewModel;
using log4net;
using log4net.Config;

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

        private void MainWindow_Loaded(object sender, RoutedEventArgs args)
        {
            this.viewModel.Load();
        }
    }
}