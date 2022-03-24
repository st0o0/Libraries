using WPFLibrary.ViewModels.Interfaces;

namespace WPFLibrary.ViewModels
{
    public abstract class ViewModelBase : ViewModel, IViewModel
    {
        protected ViewModelBase() : base()
        {
        }
    }
}