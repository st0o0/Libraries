using System;
using System.Threading;
using System.Threading.Tasks;
using WPFLibrary.EventSystem.References;

namespace WPFLibrary.EventSystem.EventSubscriptions
{
    ///<summary>
    /// Extends <see cref="EventSubscription"/> to invoke the <see cref="EventSubscription.Action"/> delegate
    /// in a specific <see cref="SynchronizationContext"/>.
    ///</summary>
    public class AsyncDispatcherEventSubscription : AsyncEventSubscriptionBase
    {
        private readonly SynchronizationContext syncContext;

        ///<summary>
        /// Creates a new instance of <see cref="BackgroundEventSubscription"/>.
        ///</summary>
        ///<param name="actionReference">A reference to a delegate of type <see cref="System.Action{TPayload}"/>.</param>
        ///<param name="context">The synchronization context to use for UI thread dispatching.</param>
        ///<exception cref="ArgumentNullException">When <paramref name="actionReference"/> or <see paramref="filterReference"/> are <see langword="null" />.</exception>
        ///<exception cref="ArgumentException">When the target of <paramref name="actionReference"/> is not of type <see cref="System.Action{TPayload}"/>.</exception>
        public AsyncDispatcherEventSubscription(IDelegateReference actionReference, SynchronizationContext context) : base(actionReference)
        {
            syncContext = context;
        }

        /// <summary>
        /// Invokes the specified <see cref="Func{Task}"/> asynchronously in the specified <see cref="SynchronizationContext"/>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public override Task InvokeDelegate(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));

            TaskCompletionSource<bool> tcs = new();
            syncContext.Post(async (args) =>
            {
                await action(cancellationToken);
                tcs.SetResult(true);
            }, null);
            return tcs.Task;
        }
    }
}