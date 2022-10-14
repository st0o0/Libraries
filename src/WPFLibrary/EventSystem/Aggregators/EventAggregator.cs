using System;
using System.Collections.Generic;
using System.Threading;
using WPFLibrary.EventSystem.Aggregators.Interfaces;
using WPFLibrary.EventSystem.Events.Bases;

namespace WPFLibrary.EventSystem.Aggregators
{
    /// <summary>
    /// Implements <see cref="IEventAggregator"/>.
    /// </summary>
    public sealed class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, EventBase> events = new();

        private readonly SynchronizationContext syncContext = SynchronizationContext.Current;

        /// <summary>
        /// Gets the single instance of the event managed by this EventAggregator. Multiple calls to this method with the same <typeparamref name="TEventType"/> returns the same event instance.
        /// </summary>
        /// <typeparam name="TEventType">The type of event to get. This must inherit from <see cref="EventBase"/>.</typeparam>
        /// <returns>A singleton instance of an event object of type <typeparamref name="TEventType"/>.</returns>
        public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            lock (events)
            {
                if (events.TryGetValue(typeof(TEventType), out EventBase existingEvent))
                {
                    return (TEventType)existingEvent;
                }
                else
                {
                    TEventType newEvent = new()
                    {
                        SynchronizationContext = syncContext
                    };
                    events.Add(typeof(TEventType), newEvent);
                    return newEvent;
                }
            }
        }
    }
}