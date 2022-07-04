using System;
using System.Threading;
using WPFLibrary.EventSystem.References;

namespace WPFLibrary.EventSystem.EventSubscriptions
{
    ///<summary>
    /// Extends <see cref="EventSubscription{TPayLoad}"/> to invoke the <see cref="EventSubscription{TPayLoad}.Action"/> delegate
    /// in a specific <see cref="SynchronizationContext"/>.
    ///</summary>
    /// <typeparam name="TPayLoad">The type to use for the generic <see cref="System.Action{TPayLoad}"/> and <see cref="Predicate{TPayLoad}"/> types.</typeparam>
    public class DispatcherEventSubscription<TPayLoad> : EventSubscriptionBase<TPayLoad>
    {
        private readonly SynchronizationContext syncContext;

        ///<summary>
        /// Creates a new instance of <see cref="BackgroundEventSubscription{TPayLoad}"/>.
        ///</summary>
        ///<param name="actionReference">A reference to a delegate of type <see cref="System.Action{TPayLoad}"/>.</param>
        ///<param name="filterReference">A reference to a delegate of type <see cref="Predicate{TPayLoad}"/>.</param>
        ///<param name="context">The synchronization context to use for UI thread dispatching.</param>
        ///<exception cref="ArgumentNullException">When <paramref name="actionReference"/> or <see paramref="filterReference"/> are <see langword="null" />.</exception>
        ///<exception cref="ArgumentException">When the target of <paramref name="actionReference"/> is not of type <see cref="System.Action{TPayLoad}"/>,
        ///or the target of <paramref name="filterReference"/> is not of type <see cref="Predicate{TPayLoad}"/>.</exception>
        public DispatcherEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference, SynchronizationContext context) : base(actionReference, filterReference)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            syncContext = context;
        }

        /// <summary>
        /// Invokes the specified <see cref="System.Action{TPayLoad}"/> asynchronously in the specified <see cref="SynchronizationContext"/>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="argument">The payload to pass <paramref name="action"/> while invoking it.</param>
        public override void InvokeDelegate(Action<TPayLoad> action, TPayLoad argument)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));
            syncContext.Post((o) => action((TPayLoad)o), argument);
        }
    }
}