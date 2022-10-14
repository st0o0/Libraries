using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WPFLibrary.EventSystem.EventSubscriptions;
using WPFLibrary.EventSystem.SubscriptionTokens;

namespace WPFLibrary.EventSystem.Events.Bases
{
    public abstract class EventBase
    {
        private readonly List<IEventSubscription> subscriptions = new();

        /// <summary>
        /// Allows the SynchronizationContext to be set by the EventAggregator for UI Thread Dispatching
        /// </summary>
        public SynchronizationContext SynchronizationContext { get; set; }

        /// <summary>
        /// Gets the list of current subscriptions.
        /// </summary>
        /// <value>The current subscribers.</value>
        protected ICollection<IEventSubscription> Subscriptions => this.subscriptions;

        /// <summary>
        /// Adds the specified <see cref="IEventSubscription"/> to the subscribers' collection.
        /// </summary>
        /// <param name="eventSubscription">The subscriber.</param>
        /// <returns>The <see cref="SubscriptionToken"/> that uniquely identifies every subscriber.</returns>
        /// <remarks>
        /// Adds the subscription to the internal list and assigns it a new <see cref="SubscriptionToken"/>.
        /// </remarks>
        protected virtual SubscriptionToken InternalSubscribe(IEventSubscription eventSubscription)
        {
            ArgumentNullException.ThrowIfNull(eventSubscription, nameof(eventSubscription));

            eventSubscription.SubscriptionToken = new SubscriptionToken(Unsubscribe);

            lock (Subscriptions)
            {
                Subscriptions.Add(eventSubscription);
            }
            return eventSubscription.SubscriptionToken;
        }

        /// <summary>
        /// Calls all the execution strategies exposed by the list of <see cref="IEventSubscription"/>.
        /// </summary>
        /// <param name="arguments">The arguments that will be passed to the listeners.</param>
        /// <remarks>Before executing the strategies, this class will prune all the subscribers from the
        /// list that return a <see langword="null" /> <see cref="Action{T}"/> when calling the
        /// <see cref="IEventSubscription.GetExecutionStrategy"/> method.</remarks>
        protected Task InternalPublish(object args = null, CancellationToken cancellationToken = default)
        {
            return Task.WhenAll(PruneAndReturnStrategies().Select(func => func?.Invoke(args, cancellationToken)));
        }

        /// <summary>
        /// Removes the subscriber matching the <see cref="SubscriptionToken"/>.
        /// </summary>
        /// <param name="token">The <see cref="SubscriptionToken"/> returned by <see cref="EventBase"/> while subscribing to the event.</param>
        public virtual void Unsubscribe(SubscriptionToken token)
        {
            lock (Subscriptions)
            {
                IEventSubscription subscription = Subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);
                if (subscription != null)
                {
                    Subscriptions.Remove(subscription);
                }
            }
        }

        /// <summary>
        /// Returns <see langword="true"/> if there is a subscriber matching <see cref="SubscriptionToken"/>.
        /// </summary>
        /// <param name="token">The <see cref="SubscriptionToken"/> returned by <see cref="EventBase"/> while subscribing to the event.</param>
        /// <returns><see langword="true"/> if there is a <see cref="SubscriptionToken"/> that matches; otherwise <see langword="false"/>.</returns>
        public virtual bool Contains(SubscriptionToken token)
        {
            lock (Subscriptions)
            {
                IEventSubscription subscription = Subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);
                return subscription != null;
            }
        }

        private List<Func<object, CancellationToken, Task>> PruneAndReturnStrategies()
        {
            List<Func<object, CancellationToken, Task>> result = new();

            lock (Subscriptions)
            {
                for (int i = Subscriptions.Count - 1; i >= 0; i--)
                {
                    Func<object, CancellationToken, Task> listItem = subscriptions[i].GetExecutionStrategy();

                    if (listItem == null)
                    {
                        // Prune from main list. Log?
                        subscriptions.RemoveAt(i);
                    }
                    else
                    {
                        result.Add(listItem);
                    }
                }
            }

            return result;
        }
    }
}