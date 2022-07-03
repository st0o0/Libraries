using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using WPFLibrary.EventSystem.References;
using WPFLibrary.EventSystem.SubscriptionTokens;

namespace WPFLibrary.EventSystem.EventSubscriptions
{
    /// <summary>
	/// Provides a way to retrieve a <see cref="Delegate"/> to execute an action depending
	/// on the value of a second filter predicate that returns true if the action should execute.
	/// </summary>
    public abstract class AsyncEventSubscriptionBase : IEventSubscription
    {
        private readonly IDelegateReference actionReference;

        ///<summary>
        /// Creates a new instance of <see cref="EventSubscription"/>.
        ///</summary>
        ///<param name="actionReference">A reference to a delegate of type <see cref="System.Action"/>.</param>
        ///<exception cref="ArgumentNullException">When <paramref name="actionReference"/> or <see paramref="filterReference"/> are <see langword="null" />.</exception>
        ///<exception cref="ArgumentException">When the target of <paramref name="actionReference"/> is not of type <see cref="System.Action"/>.</exception>
        public AsyncEventSubscriptionBase(IDelegateReference actionReference)
        {
            ArgumentNullException.ThrowIfNull(actionReference, nameof(actionReference));
            if (actionReference.Target is not System.Action)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "InvalidDelegateRerefenceTypeException", typeof(Action).FullName), nameof(actionReference));
            }

            this.actionReference = actionReference;
        }

        /// <summary>
        /// Gets the target <see cref="System.Threading.Tasks"/> that is referenced by the <see cref="IDelegateReference"/>.
        /// </summary>
        /// <value>An <see cref="System.Action"/> or <see langword="null" /> if the referenced target is not alive.</value>
        public Func<CancellationToken, Task> Action => (Func<CancellationToken, Task>)actionReference.Target;

        public Delegate Delegate => actionReference.Target;

        /// <summary>
        /// Gets or sets a <see cref="SubscriptionToken"/> that identifies this <see cref="IEventSubscription"/>.
        /// </summary>
        /// <value>A token that identifies this <see cref="IEventSubscription"/>.</value>
        public SubscriptionToken SubscriptionToken { get; set; }

        /// <summary>
        /// Gets the execution strategy to publish this event.
        /// </summary>
        /// <returns>An <see cref="System.Action"/> with the execution strategy, or <see langword="null" /> if the <see cref="IEventSubscription"/> is no longer valid.</returns>
        /// <remarks>
        /// If <see cref="Action"/>is no longer valid because it was
        /// garbage collected, this method will return <see langword="null" />.
        /// Otherwise it will return a delegate that evaluates the <see cref="Filter"/> and if it
        /// returns <see langword="true" /> will then call <see cref="InvokeAction"/>. The returned
        /// delegate holds a hard reference to the <see cref="Action"/> target
        /// <see cref="Delegate">delegates</see>. As long as the returned delegate is not garbage collected,
        /// the <see cref="Action"/> references delegates won't get collected either.
        /// </remarks>
        public virtual Func<object, CancellationToken, Task> GetExecutionStrategy()
        {
            Func<CancellationToken, Task> func = this.Action;
            if (func != null)
            {
                return async (arg, ct) =>
                {
                    await InvokeDelegate(func, ct);
                    await Task.CompletedTask;
                };
            }
            return null;
        }

        /// <summary>
        /// Invokes the specified <see cref="System.Action{TPayload}"/> synchronously when not overridden.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="ArgumentNullException">An <see cref="ArgumentNullException"/> is thrown if <paramref name="action"/> is null.</exception>
        public virtual Task InvokeDelegate(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));

            return action(cancellationToken);
        }
    }
}