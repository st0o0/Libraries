using System;
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

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}