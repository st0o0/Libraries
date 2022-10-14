using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
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

        protected void UpdateUI(Action action, DispatcherPriority priority = DispatcherPriority.Input, CancellationToken cancellationToken = default)
        {
            Dispatcher?.Invoke(action, priority, cancellationToken);
        }

        protected async Task UpdateUIAsync(Action action, DispatcherPriority priority = DispatcherPriority.Input, CancellationToken cancellationToken = default)
        {
            await Dispatcher?.InvokeAsync(action, priority, cancellationToken);
        }

        protected async Task UpdateUIAsync(Func<Task> func, DispatcherPriority priority = DispatcherPriority.Input, CancellationToken cancellationToken = default)
        {
            await await Dispatcher.InvokeAsync<Task>(func, priority, cancellationToken);
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