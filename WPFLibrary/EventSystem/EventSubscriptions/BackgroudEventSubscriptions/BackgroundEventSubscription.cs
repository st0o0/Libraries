using System;
using System.Threading.Tasks;
using WPFLibrary.EventSystem.References;

namespace WPFLibrary.EventSystem.EventSubscriptions
{
    /// <summary>
    /// Extends <see cref="EventSubscription"/> to invoke the <see cref="EventSubscription.Action"/> delegate in a background thread.
    /// </summary>
    public class BackgroundEventSubscription : EventSubscriptionBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="BackgroundEventSubscription"/>.
        /// </summary>
        /// <param name="actionReference">A reference to a delegate of type <see cref="System.Action"/>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="actionReference"/> or <see paramref="filterReference"/> are <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">When the target of <paramref name="actionReference"/> is not of type <see cref="System.Action"/>.</exception>
        public BackgroundEventSubscription(IDelegateReference actionReference) : base(actionReference)
        {
        }

        /// <summary>
        /// Invokes the specified <see cref="System.Action"/> in an asynchronous thread by using a <see cref="Task"/>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public override void InvokeDelegate(Action action)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));

            action.Invoke();
        }
    }
}