using WPFLibrary.EventSystem.References;

namespace WPFLibrary.EventSystem.EventSubscriptions
{
    public class AsyncPublisherEventSubscription<TPayLoad> : AsyncEventSubscriptionBase<TPayLoad>
    {
        public AsyncPublisherEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference) : base(actionReference, filterReference)
        {
        }
    }
}