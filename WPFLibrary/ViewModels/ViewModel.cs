using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WPFLibrary.ViewModels.Interfaces;

namespace WPFLibrary.ViewModels
{
    public abstract class ViewModel : IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}