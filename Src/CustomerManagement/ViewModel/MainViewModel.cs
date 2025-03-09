using CustomerManagement.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ProductsViewModel ProductsViewModel { get; }
        public CustomersViewModel CustomersViewModel { get; }
        public DelegateCommand SelectViewModelCommand { get; }
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
                this.NotifyPropertyChanged();
            }
        }

        public MainViewModel(CustomersViewModel customersViewModel, ProductsViewModel productsViewModel)
        {
            this.CustomersViewModel = customersViewModel;
            this.ProductsViewModel = productsViewModel;
            this.SelectedViewModel = customersViewModel;
            this.SelectViewModelCommand = new DelegateCommand(SelectViewModel);
        }

        public override async Task LoadAsync()
        {
            if (SelectedViewModel != null)
            {
                await this.SelectedViewModel.LoadAsync();
            }
        }

        public async void SelectViewModel(object? parameter)
        {
            this.SelectedViewModel = parameter as ViewModelBase;
            await this.LoadAsync();
        }
    }
}
