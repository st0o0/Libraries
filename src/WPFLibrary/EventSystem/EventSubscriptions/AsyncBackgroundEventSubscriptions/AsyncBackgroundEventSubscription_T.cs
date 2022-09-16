using System;
using System.Threading;
using System.Threading.Tasks;
using WPFLibrary.EventSystem.References;

namespace WPFLibrary.EventSystem.EventSubscriptions
{
    public class AsyncBackgroundEventSubscription<TPayLoad> : AsyncEventSubscriptionBase<TPayLoad>
    {
        /// <summary>
        /// Creates a new instance of <see cref="BackgroundEventSubscription{TPayLoad}"/>.
        /// </summary>
        /// <param name="actionReference">A reference to a delegate of type <see cref="System.Action{TPayLoad}"/>.</param>
        /// <param name="filterReference">A reference to a delegate of type <see cref="Predicate{TPayLoad}"/>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="actionReference"/> or <see paramref="filterReference"/> are <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">When the target of <paramref name="actionReference"/> is not of type <see cref="System.Action{TPayLoad}"/>,
        /// or the target of <paramref name="filterReference"/> is not of type <see cref="Predicate{TPayLoad}"/>.</exception>
        public AsyncBackgroundEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference) : base(actionReference, filterReference)
        {
        }

        /// <summary>
        /// Invokes the specified <see cref="Task"/> in an asynchronous thread by using a <see cref="Task"/>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="argument">The arguments for the action to execute.</param>
        public override Task InvokeDelegate(Func<TPayLoad, CancellationToken, Task> action, TPayLoad argument, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => action(argument, cancellationToken), cancellationToken);
        }
    }
}