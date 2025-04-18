using CustomerManagement.Navigation;
using CDB.Model;
using CustomerManagement.ViewModel.CustomerViewModels;

namespace CustomerManagement.Test.Navigation
{
    public class NavigationStoreTest
    {
        [Test]
        public void TestNavigationStore()
        {
            CustomerItemViewModel testViewModel = new CustomerItemViewModel(new Customer());
            NavigationStore testNavigationStore = new NavigationStore();

            testNavigationStore.SelectedViewModel = testViewModel;
            Assert.That(testNavigationStore.SelectedViewModel, Is.Not.Null);
            CustomerItemViewModel? selectedViewModel = (CustomerItemViewModel) testNavigationStore.SelectedViewModel;
            Assert.That(testNavigationStore.SelectedViewModel is CustomerItemViewModel);
        }
    }
}
