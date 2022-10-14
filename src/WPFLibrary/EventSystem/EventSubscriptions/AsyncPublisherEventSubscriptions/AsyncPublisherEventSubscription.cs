using WPFLibrary.EventSystem.References;

namespace WPFLibrary.EventSystem.EventSubscriptions
{
    public class AsyncPublisherEventSubscription : AsyncEventSubscriptionBase
    {
        public AsyncPublisherEventSubscription(IDelegateReference actionReference) : base(actionReference)
        {
        }
    }
}