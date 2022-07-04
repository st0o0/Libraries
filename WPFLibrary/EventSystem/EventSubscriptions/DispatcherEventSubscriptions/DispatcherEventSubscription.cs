using System;
using System.Threading;
using WPFLibrary.EventSystem.References;

namespace WPFLibrary.EventSystem.EventSubscriptions
{
    ///<summary>
    /// Extends <see cref="EventSubscription"/> to invoke the <see cref="EventSubscription.Action"/> delegate
    /// in a specific <see cref="SynchronizationContext"/>.
    ///</summary>
    public class DispatcherEventSubscription : EventSubscriptionBase
    {
        private readonly SynchronizationContext syncContext;

        ///<summary>
        /// Creates a new instance of <see cref="BackgroundEventSubscription"/>.
        ///</summary>
        ///<param name="actionReference">A reference to a delegate of type <see cref="System.Action{TPayload}"/>.</param>
        ///<param name="context">The synchronization context to use for UI thread dispatching.</param>
        ///<exception cref="ArgumentNullException">When <paramref name="actionReference"/> or <see paramref="filterReference"/> are <see langword="null" />.</exception>
        ///<exception cref="ArgumentException">When the target of <paramref name="actionReference"/> is not of type <see cref="System.Action{TPayload}"/>.</exception>
        public DispatcherEventSubscription(IDelegateReference actionReference, SynchronizationContext context) : base(actionReference)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            syncContext = context;
        }

        /// <summary>
        /// Invokes the specified <see cref="System.Action{TPayload}"/> asynchronously in the specified <see cref="SynchronizationContext"/>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public override void InvokeDelegate(Action action)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));

            syncContext.Post((o) => action.Invoke(), null);
        }
    }
}