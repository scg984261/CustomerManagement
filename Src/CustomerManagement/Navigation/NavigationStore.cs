using CustomerManagement.ViewModel;

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
                

                if (this.selectedViewModel != null)
                {
                    this.NotifyCurrentViewModelChanged();
                }
            }
        }

        public event Action? SelectedViewModelChanged;

        public NavigationStore()
        {
        }

        public void NotifyCurrentViewModelChanged()
        {
            this.SelectedViewModelChanged?.Invoke();
        }
    }
}
