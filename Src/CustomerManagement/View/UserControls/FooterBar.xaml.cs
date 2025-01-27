using System.Reflection;
using System.Windows.Controls;

namespace CustomerManagement.View.UserControls
{    
    public partial class FooterBar : UserControl
    {
        public static string MachineName
        {
            get
            {
                return Environment.MachineName;
            }
        }

        public string? VersionNumber
        {
            get
            {
                return Assembly.GetAssembly(this.GetType())?.GetName().Version?.ToString();
            }
        }

        public FooterBar()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
