using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerManagement.ViewModel;
using CustomerManagement.Data;

namespace CustomerManagement.Navigation
{
    public class NavigationStore : ViewModelBase
    {
        private ViewModelBase? selectedViewModel;
        public ViewModelBase? SelectedViewModel
        {
            get
            {
                return this.selectedViewModel;
            }
            set
            {
                this.selectedViewModel = value;
                this.NotifyCurrentViewModelChanged();
            }
        }

        public event Action SelectedViewModelChanged;

        public NavigationStore()
        {
        }

        public void NotifyCurrentViewModelChanged()
        {
            this.SelectedViewModelChanged?.Invoke();
        }
    }
}
