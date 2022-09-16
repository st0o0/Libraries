using System;
using System.Threading;
using System.Threading.Tasks;
using WPFLibrary.EventSystem.References;

namespace WPFLibrary.EventSystem.EventSubscriptions
{
    public class AsyncDispatcherEventSubscription<TPayLoad> : AsyncEventSubscriptionBase<TPayLoad>
    {
        private readonly SynchronizationContext syncContext;

        ///<summary>
        /// Creates a new instance of <see cref="BackgroundEventSubscription{TPayload}"/>.
        ///</summary>
        ///<param name="actionReference">A reference to a delegate of type <see cref="System.Action{TPayLoad}"/>.</param>
        ///<param name="filterReference">A reference to a delegate of type <see cref="Predicate{TPayLoad}"/>.</param>
        ///<param name="context">The synchronization context to use for UI thread dispatching.</param>
        ///<exception cref="ArgumentNullException">When <paramref name="actionReference"/> or <see paramref="filterReference"/> are <see langword="null" />.</exception>
        ///<exception cref="ArgumentException">When the target of <paramref name="actionReference"/> is not of type <see cref="System.Action{TPayLoad}"/>,
        ///or the target of <paramref name="filterReference"/> is not of type <see cref="Predicate{TPayLoad}"/>.</exception>
        public AsyncDispatcherEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference, SynchronizationContext context) : base(actionReference, filterReference)
        {
            syncContext = context;
        }

        /// <summary>
        /// Invokes the specified <see cref="Func{TPayLoad, Task}"/> asynchronously in the specified <see cref="SynchronizationContext"/>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="argument">The payload to pass <paramref name="action"/> while invoking it.</param>
        public override Task InvokeDelegate(Func<TPayLoad, CancellationToken, Task> action, TPayLoad argument, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));

            TaskCompletionSource<bool> tcs = new();
            syncContext.Post(async (args) =>
            {
                await action.Invoke((TPayLoad)args, cancellationToken);
                tcs.SetResult(true);
            }, argument);
            return tcs.Task;
        }
    }
}