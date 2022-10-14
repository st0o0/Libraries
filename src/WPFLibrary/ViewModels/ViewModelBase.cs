using Microsoft.Extensions.Logging;
using WPFLibrary.ViewModels.Interfaces;

namespace WPFLibrary.ViewModels
{
    public abstract class ViewModelBase : ViewModel, IViewModel
    {
        protected ViewModelBase(ILogger logger) : this()
        {
            this.Logger = logger;
        }

        protected ViewModelBase() : base()
        {
        }

        protected ILogger Logger { get; init; } = null;
    }
}