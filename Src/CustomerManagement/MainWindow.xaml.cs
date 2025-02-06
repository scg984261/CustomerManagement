using System.Diagnostics.CodeAnalysis;
using System.Windows;
using log4net;
using log4net.Config;

namespace CustomerManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindow));

        public MainWindow()
        {
            XmlConfigurator.Configure();
            InitializeComponent();
            log.Info("Start of application. Main window successfully initialised.");
        }
    }
}