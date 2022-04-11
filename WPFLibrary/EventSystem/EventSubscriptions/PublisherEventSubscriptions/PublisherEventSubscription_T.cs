using WPFLibrary.EventSystem.References;

namespace WPFLibrary.EventSystem.EventSubscriptions
{
    public class PublisherEventSubscription<TPayLoad> : EventSubscriptionBase<TPayLoad>
    {
        public PublisherEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference) : base(actionReference, filterReference)
        {
        }
    }
}