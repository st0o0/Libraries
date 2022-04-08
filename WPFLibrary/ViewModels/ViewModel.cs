using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WPFLibrary.ComponentModel;
using WPFLibrary.ViewModels.Interfaces;

namespace WPFLibrary.ViewModels
{
    public abstract class ViewModel : ObservableObject, IViewModel
    {
        protected ViewModel()
        {
            Dispatcher = Application.Current.Dispatcher;
        }

        public Dispatcher Dispatcher { get; private set; }

        protected void UpdateUI(Action action, DispatcherPriority priority = DispatcherPriority.Input)
        {
            Dispatcher?.Invoke(action, priority);
        }

        protected async Task UpdateUIAsync(Action action, DispatcherPriority priority = DispatcherPriority.Input)
        {
            await Dispatcher?.InvokeAsync(action, priority);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanging"/> event.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}