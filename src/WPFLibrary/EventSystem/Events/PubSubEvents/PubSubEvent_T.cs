using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WPFLibrary.EventSystem.Events.Bases;
using WPFLibrary.EventSystem.EventSubscriptions;
using WPFLibrary.EventSystem.References;
using WPFLibrary.EventSystem.SubscriptionTokens;

namespace WPFLibrary.EventSystem.Events
{
    public abstract class PubSubEvent<TPayLoad> : EventBase
    {
        /// <summary>
        /// Subscribes a delegate to an event that will be published on the <see cref="ThreadOption.PublisherThread"/>.
        /// <see cref="PubSubEvent{TPayLoad}"/> will maintain a <see cref="WeakReference"/> to the target of the supplied <paramref name="action"/> delegate.
        /// </summary>
        /// <param name="action">The delegate that gets executed when the event is published.</param>
        /// <returns>A <see cref="SubscriptionToken"/> that uniquely identifies the added subscription.</returns>
        /// <remarks>
        /// The PubSubEvent collection is thread-safe.
        /// </remarks>
        public SubscriptionToken Subscribe(Action<TPayLoad> action)
        {
            return Subscribe(action, ThreadOption.PublisherThread);
        }

        /// <summary>
        /// Subscribes a delegate to an event that will be published on the <see cref="ThreadOption.PublisherThread"/>.
        /// <see cref="PubSubEvent{TPayLoad}"/> will maintain a <see cref="WeakReference"/> to the target of the supplied <paramref name="action"/> delegate.
        /// </summary>
        /// <param name="action">The delegate that gets executed when the event is published.</param>
        /// <returns>A <see cref="SubscriptionToken"/> that uniquely identifies the added subscription.</returns>
        /// <remarks>
        /// The PubSubEvent collection is thread-safe.
        /// </remarks>
        public SubscriptionToken Subscribe(Func<TPayLoad, CancellationToken, Task> action)
        {
            return Subscribe(action, ThreadOption.PublisherThread);
        }

        /// <summary>
        /// Subscribes a delegate to an event.
        /// PubSubEvent will maintain a <see cref="WeakReference"/> to the Target of the supplied <paramref name="action"/> delegate.
        /// </summary>
        /// <param name="action">The delegate that gets executed when the event is raised.</param>
        /// <param name="threadOption">Specifies on which thread to receive the delegate callback.</param>
        /// <returns>A <see cref="SubscriptionToken"/> that uniquely identifies the added subscription.</returns>
        /// <remarks>
        /// The PubSubEvent collection is thread-safe.
        /// </remarks>
        public SubscriptionToken Subscribe(Action<TPayLoad> action, ThreadOption threadOption)
        {
            return Subscribe(action, threadOption, false);
        }

        /// <summary>
        /// Subscribes a delegate to an event.
        /// PubSubEvent will maintain a <see cref="WeakReference"/> to the Target of the supplied <paramref name="action"/> delegate.
        /// </summary>
        /// <param name="action">The delegate that gets executed when the event is raised.</param>
        /// <param name="threadOption">Specifies on which thread to receive the delegate callback.</param>
        /// <returns>A <see cref="SubscriptionToken"/> that uniquely identifies the added subscription.</returns>
        /// <remarks>
        /// The PubSubEvent collection is thread-safe.
        /// </remarks>
        public SubscriptionToken Subscribe(Func<TPayLoad, CancellationToken, Task> action, ThreadOption threadOption)
        {
            return Subscribe(action, threadOption, false);
        }

        /// <summary>
        /// Subscribes a delegate to an event that will be published on the <see cref="ThreadOption.PublisherThread"/>.
        /// </summary>
        /// <param name="action">The delegate that gets executed when the event is published.</param>
        /// <param name="keepSubscriberReferenceAlive">When <see langword="true"/>, the <see cref="PubSubEvent{TPayLoad}"/> keeps a reference to the subscriber so it does not get garbage collected.</param>
        /// <returns>A <see cref="SubscriptionToken"/> that uniquely identifies the added subscription.</returns>
        /// <remarks>
        /// If <paramref name="keepSubscriberReferenceAlive"/> is set to <see langword="false" />, <see cref="PubSubEvent{TPayLoad}"/> will maintain a <see cref="WeakReference"/> to the Target of the supplied <paramref name="action"/> delegate.
        /// If not using a WeakReference (<paramref name="keepSubscriberReferenceAlive"/> is <see langword="true" />), the user must explicitly call Unsubscribe for the event when disposing the subscriber in order to avoid memory leaks or unexpected behavior.
        /// <para/>
        /// The PubSubEvent collection is thread-safe.
        /// </remarks>
        public SubscriptionToken Subscribe(Action<TPayLoad> action, bool keepSubscriberReferenceAlive)
        {
            return Subscribe(action, ThreadOption.PublisherThread, keepSubscriberReferenceAlive);
        }

        /// <summary>
        /// Subscribes a delegate to an event that will be published on the <see cref="ThreadOption.PublisherThread"/>.
        /// </summary>
        /// <param name="action">The delegate that gets executed when the event is published.</param>
        /// <param name="keepSubscriberReferenceAlive">When <see langword="true"/>, the <see cref="PubSubEvent{TPayLoad}"/> keeps a reference to the subscriber so it does not get garbage collected.</param>
        /// <returns>A <see cref="SubscriptionToken"/> that uniquely identifies the added subscription.</returns>
        /// <remarks>
        /// If <paramref name="keepSubscriberReferenceAlive"/> is set to <see langword="false" />, <see cref="PubSubEvent{TPayLoad}"/> will maintain a <see cref="WeakReference"/> to the Target of the supplied <paramref name="action"/> delegate.
        /// If not using a WeakReference (<paramref name="keepSubscriberReferenceAlive"/> is <see langword="true" />), the user must explicitly call Unsubscribe for the event when disposing the subscriber in order to avoid memory leaks or unexpected behavior.
        /// <para/>
        /// The PubSubEvent collection is thread-safe.
        /// </remarks>
        public SubscriptionToken Subscribe(Func<TPayLoad, CancellationToken, Task> action, bool keepSubscriberReferenceAlive)
        {
            return Subscribe(action, ThreadOption.PublisherThread, keepSubscriberReferenceAlive);
        }

        /// <summary>
        /// Subscribes a delegate to an event.
        /// </summary>
        /// <param name="action">The delegate that gets executed when the event is published.</param>
        /// <param name="threadOption">Specifies on which thread to receive the delegate callback.</param>
        /// <param name="keepSubscriberReferenceAlive">When <see langword="true"/>, the <see cref="PubSubEvent{TPayLoad}"/> keeps a reference to the subscriber so it does not get garbage collected.</param>
        /// <returns>A <see cref="SubscriptionToken"/> that uniquely identifies the added subscription.</returns>
        /// <remarks>
        /// If <paramref name="keepSubscriberReferenceAlive"/> is set to <see langword="false" />, <see cref="PubSubEvent{TPayLoad}"/> will maintain a <see cref="WeakReference"/> to the Target of the supplied <paramref name="action"/> delegate.
        /// If not using a WeakReference (<paramref name="keepSubscriberReferenceAlive"/> is <see langword="true" />), the user must explicitly call Unsubscribe for the event when disposing the subscriber in order to avoid memory leaks or unexpected behavior.
        /// <para/>
        /// The PubSubEvent collection is thread-safe.
        /// </remarks>
        public SubscriptionToken Subscribe(Action<TPayLoad> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive)
        {
            return Subscribe(action, threadOption, keepSubscriberReferenceAlive, null);
        }

        /// <summary>
        /// Subscribes a delegate to an event.
        /// </summary>
        /// <param name="action">The delegate that gets executed when the event is published.</param>
        /// <param name="threadOption">Specifies on which thread to receive the delegate callback.</param>
        /// <param name="keepSubscriberReferenceAlive">When <see langword="true"/>, the <see cref="PubSubEvent{TPayLoad}"/> keeps a reference to the subscriber so it does not get garbage collected.</param>
        /// <returns>A <see cref="SubscriptionToken"/> that uniquely identifies the added subscription.</returns>
        /// <remarks>
        /// If <paramref name="keepSubscriberReferenceAlive"/> is set to <see langword="false" />, <see cref="PubSubEvent{TPayLoad}"/> will maintain a <see cref="WeakReference"/> to the Target of the supplied <paramref name="action"/> delegate.
        /// If not using a WeakReference (<paramref name="keepSubscriberReferenceAlive"/> is <see langword="true" />), the user must explicitly call Unsubscribe for the event when disposing the subscriber in order to avoid memory leaks or unexpected behavior.
        /// <para/>
        /// The PubSubEvent collection is thread-safe.
        /// </remarks>
        public SubscriptionToken Subscribe(Func<TPayLoad, CancellationToken, Task> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive)
        {
            return Subscribe(action, threadOption, keepSubscriberReferenceAlive, null);
        }

        /// <summary>
        /// Subscribes a delegate to an event.
        /// </summary>
        /// <param name="action">The delegate that gets executed when the event is published.</param>
        /// <param name="threadOption">Specifies on which thread to receive the delegate callback.</param>
        /// <param name="keepSubscriberReferenceAlive">When <see langword="true"/>, the <see cref="PubSubEvent{TPayLoad}"/> keeps a reference to the subscriber so it does not get garbage collected.</param>
        /// <param name="filter">Filter to evaluate if the subscriber should receive the event.</param>
        /// <returns>A <see cref="SubscriptionToken"/> that uniquely identifies the added subscription.</returns>
        /// <remarks>
        /// If <paramref name="keepSubscriberReferenceAlive"/> is set to <see langword="false" />, <see cref="PubSubEvent{TPayLoad}"/> will maintain a <see cref="WeakReference"/> to the Target of the supplied <paramref name="action"/> delegate.
        /// If not using a WeakReference (<paramref name="keepSubscriberReferenceAlive"/> is <see langword="true" />), the user must explicitly call Unsubscribe for the event when disposing the subscriber in order to avoid memory leaks or unexpected behavior.
        ///
        /// The PubSubEvent collection is thread-safe.
        /// </remarks>
        public virtual SubscriptionToken Subscribe(Action<TPayLoad> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<TPayLoad> filter)
        {
            IDelegateReference actionReference = new DelegateReference(action, keepSubscriberReferenceAlive);
            IDelegateReference filterReference;
            if (filter != null)
            {
                filterReference = new DelegateReference(filter, keepSubscriberReferenceAlive);
            }
            else
            {
                filterReference = new DelegateReference(new Predicate<TPayLoad>(x => true), true);
            }

            if (SynchronizationContext == null && threadOption == ThreadOption.UIThread)
            {
                throw new InvalidOperationException("EventAggregatorNotConstructedOnUIThread");
            }

            EventSubscriptionBase<TPayLoad> subscription = threadOption switch
            {
                ThreadOption.PublisherThread => new PublisherEventSubscription<TPayLoad>(actionReference, filterReference),
                ThreadOption.BackgroundThread => new BackgroundEventSubscription<TPayLoad>(actionReference, filterReference),
                ThreadOption.UIThread => new DispatcherEventSubscription<TPayLoad>(actionReference, filterReference, SynchronizationContext),
                _ => new PublisherEventSubscription<TPayLoad>(actionReference, filterReference),
            };
            return InternalSubscribe(subscription);
        }

        /// <summary>
        /// Subscribes a delegate to an event.
        /// </summary>
        /// <param name="func">The delegate that gets executed when the event is published.</param>
        /// <param name="threadOption">Specifies on which thread to receive the delegate callback.</param>
        /// <param name="keepSubscriberReferenceAlive">When <see langword="true"/>, the <see cref="PubSubEvent{TPayLoad}"/> keeps a reference to the subscriber so it does not get garbage collected.</param>
        /// <param name="filter">Filter to evaluate if the subscriber should receive the event.</param>
        /// <returns>A <see cref="SubscriptionToken"/> that uniquely identifies the added subscription.</returns>
        /// <remarks>
        /// If <paramref name="keepSubscriberReferenceAlive"/> is set to <see langword="false" />, <see cref="PubSubEvent{TPayLoad}"/> will maintain a <see cref="WeakReference"/> to the Target of the supplied <paramref name="func"/> delegate.
        /// If not using a WeakReference (<paramref name="keepSubscriberReferenceAlive"/> is <see langword="true" />), the user must explicitly call Unsubscribe for the event when disposing the subscriber in order to avoid memory leaks or unexpected behavior.
        ///
        /// The PubSubEvent collection is thread-safe.
        /// </remarks>
        public virtual SubscriptionToken Subscribe(Func<TPayLoad, CancellationToken, Task> func, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<TPayLoad> filter)
        {
            IDelegateReference funcReference = new DelegateReference(func, keepSubscriberReferenceAlive);
            IDelegateReference filterReference = filter != null
                ? new DelegateReference(filter, keepSubscriberReferenceAlive)
                : new DelegateReference(new Predicate<TPayLoad>(x => true), true);

            if (SynchronizationContext == null && threadOption == ThreadOption.UIThread)
            {
                throw new InvalidOperationException("EventAggregatorNotConstructedOnUIThread");
            }

            AsyncEventSubscriptionBase<TPayLoad> subscription = threadOption switch
            {
                ThreadOption.PublisherThread => new AsyncPublisherEventSubscription<TPayLoad>(funcReference, filterReference),
                ThreadOption.BackgroundThread => new AsyncBackgroundEventSubscription<TPayLoad>(funcReference, filterReference),
                ThreadOption.UIThread => new AsyncDispatcherEventSubscription<TPayLoad>(funcReference, filterReference, SynchronizationContext),
                _ => new AsyncPublisherEventSubscription<TPayLoad>(funcReference, filterReference),
            };
            return InternalSubscribe(subscription);
        }

        /// <summary>
        /// Publishes the <see cref="PubSubEvent{TPayLoad}"/>.
        /// </summary>
        /// <param name="payload">Message to pass to the subscribers.</param>
        public virtual Task PublishAsync(TPayLoad payLoad, CancellationToken cancellationToken = default)
        {
            return InternalPublish(payLoad, cancellationToken);
        }

        /// <summary>
        /// Removes the first subscriber matching <see cref="Action{TPayLoad}"/> from the subscribers' list.
        /// </summary>
        /// <param name="subscriber">The <see cref="Action{TPayload}"/> used when subscribing to the event.</param>
        public virtual void Unsubscribe(Delegate subscriber)
        {
            lock (Subscriptions)
            {
                IEventSubscription eventSubscription = Subscriptions.FirstOrDefault(evt =>
                {
                    return evt.Delegate != null && evt.Delegate.GetType() == subscriber.GetType() &&
                              evt.Delegate.Target == subscriber.Target;
                });
                if (eventSubscription != null)
                {
                    Subscriptions.Remove(eventSubscription);
                }
            }
        }

        /// <summary>
        /// Returns <see langword="true"/> if there is a subscriber matching <see cref="Action{TPayLoad}"/>.
        /// </summary>
        /// <param name="subscriber">The <see cref="Action{TPayLoad}"/> used when subscribing to the event.</param>
        /// <returns><see langword="true"/> if there is an <see cref="Action{TPayLoad}"/> that matches; otherwise <see langword="false"/>.</returns>
        public virtual bool Contains(Action<TPayLoad> subscriber)
        {
            IEventSubscription eventSubscription;
            lock (Subscriptions)
            {
                eventSubscription = Subscriptions.OfType<EventSubscriptionBase<TPayLoad>>().FirstOrDefault(evt => evt.Action == subscriber);
            }
            return eventSubscription != null;
        }

        /// <summary>
        /// Returns <see langword="true"/> if there is a subscriber matching <see cref="Action{TPayLoad}"/>.
        /// </summary>
        /// <param name="subscriber">The <see cref="Action{TPayLoad}"/> used when subscribing to the event.</param>
        /// <returns><see langword="true"/> if there is an <see cref="Action{TPayLoad}"/> that matches; otherwise <see langword="false"/>.</returns>
        public virtual bool Contains(Func<TPayLoad, CancellationToken, Task> subscriber)
        {
            IEventSubscription eventSubscription;
            lock (Subscriptions)
            {
                eventSubscription = Subscriptions.OfType<AsyncEventSubscriptionBase<TPayLoad>>().FirstOrDefault(evt => evt.Action == subscriber);
            }
            return eventSubscription != null;
        }
    }
}