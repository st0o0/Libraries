using System;
using WPFLibrary.EventSystem.References;

namespace WPFLibrary.EventSystem.EventSubscriptions
{
    /// <summary>
    /// Extends <see cref="EventSubscription{TPayLoad}"/> to invoke the <see cref="EventSubscription{TPayLoad}.Action"/> delegate in a background thread.
    /// </summary>
    /// <typeparam name="TPayload">The type to use for the generic <see cref="System.Action{TPayLoad}"/> and <see cref="Predicate{TPayLoad}"/> types.</typeparam>
    public class BackgroundEventSubscription<TPayLoad> : EventSubscriptionBase<TPayLoad>
    {
        /// <summary>
        /// Creates a new instance of <see cref="BackgroundEventSubscription{TPayLoad}"/>.
        /// </summary>
        /// <param name="actionReference">A reference to a delegate of type <see cref="System.Action{TPayLoad}"/>.</param>
        /// <param name="filterReference">A reference to a delegate of type <see cref="Predicate{TPayLoad}"/>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="actionReference"/> or <see paramref="filterReference"/> are <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">When the target of <paramref name="actionReference"/> is not of type <see cref="System.Action{TPayLoad}"/>,
        /// or the target of <paramref name="filterReference"/> is not of type <see cref="Predicate{TPayLoad}"/>.</exception>
        public BackgroundEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference) : base(actionReference, filterReference)
        {
        }

        /// <summary>
        /// Invokes the specified <see cref="System.Action{TPayLoad}"/> in an asynchronous thread by using a <see cref="System.Threading.ThreadPool"/>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="argument">The payload to pass <paramref name="action"/> while invoking it.</param>
        public override void InvokeDelegate(Action<TPayLoad> action, TPayLoad argument)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));

            action.Invoke(argument);
        }
    }
}