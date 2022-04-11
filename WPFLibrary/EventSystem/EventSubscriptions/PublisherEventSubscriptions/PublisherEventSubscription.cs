using WPFLibrary.EventSystem.References;

namespace WPFLibrary.EventSystem.EventSubscriptions
{
    public class PublisherEventSubscription : EventSubscriptionBase
    {
        public PublisherEventSubscription(IDelegateReference actionReference) : base(actionReference)
        {
        }
    }
}