using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.ViewModel
{
    public class ValidationViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> errorsByPropertyName = new Dictionary<string, List<string>>();

        public bool HasErrors
        {
            get
            {
                return this.errorsByPropertyName.Any();
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName != null && this.errorsByPropertyName.ContainsKey(propertyName))
            {
                return this.errorsByPropertyName[propertyName];
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs args)
        {
            ErrorsChanged?.Invoke(this, args);
        }

        protected void AddError(string error, [CallerMemberName] string? propertyName = null)
        {
            if (propertyName is null)
            {
                return;
            }

            if (!this.errorsByPropertyName.ContainsKey(propertyName))
            {
                this.errorsByPropertyName[propertyName] = new List<string>();
            }
            if (!this.errorsByPropertyName[propertyName].Contains(error))
            {
                this.errorsByPropertyName[propertyName].Add(error);
                this.OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
                this.NotifyPropertyChanged(nameof(HasErrors));
            }
        }

        protected void ClearErrors([CallerMemberName] string? propertyName = null)
        {
            if (propertyName is null)
            {
                return;
            }

            if (this.errorsByPropertyName.ContainsKey(propertyName))
            {
                this.errorsByPropertyName.Remove(propertyName);
                this.OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
                this.NotifyPropertyChanged(nameof(HasErrors));
            }
        }
    }
}
