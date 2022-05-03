﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using WPFLibrary.EventSystem.References;
using WPFLibrary.EventSystem.SubscriptionTokens;
using WPFLibrary.Helpers;

namespace WPFLibrary.EventSystem.EventSubscriptions
{
    /// <summary>
	/// Provides a way to retrieve a <see cref="Delegate"/> to execute an action depending
	/// on the value of a second filter predicate that returns true if the action should execute.
	/// </summary>
	/// <typeparam name="TPayload">The type to use for the generic <see cref="System.Action{TPayLoad}"/> and <see cref="Predicate{TPayLoad}"/> types.</typeparam>
    public abstract class AsyncEventSubscriptionBase<TPayLoad> : IEventSubscription
    {
        private readonly IDelegateReference actionReference;
        private readonly IDelegateReference filterReference;

        ///<summary>
        /// Creates a new instance of <see cref="EventSubscription{TPayLoad}"/>.
        ///</summary>
        ///<param name="actionReference">A reference to a delegate of type <see cref="System.Action{TPayLoad}"/>.</param>
        ///<param name="filterReference">A reference to a delegate of type <see cref="Predicate{TPayLoad}"/>.</param>
        ///<exception cref="ArgumentNullException">When <paramref name="actionReference"/> or <see paramref="filterReference"/> are <see langword="null" />.</exception>
        ///<exception cref="ArgumentException">When the target of <paramref name="actionReference"/> is not of type <see cref="System.Action{TPayLoad}"/>,
        ///or the target of <paramref name="filterReference"/> is not of type <see cref="Predicate{TPayLoad}"/>.</exception>
        public AsyncEventSubscriptionBase(IDelegateReference actionReference, IDelegateReference filterReference)
        {
            if (actionReference == null)
            {
                throw new ArgumentNullException(nameof(actionReference));
            }
            if (actionReference.Target is not Func<TPayLoad, Task>)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "InvalidDelegateRerefenceTypeException", typeof(Action<TPayLoad>).FullName), nameof(actionReference));
            }

            if (filterReference == null)
            {
                throw new ArgumentNullException(nameof(filterReference));
            }
            if (filterReference.Target is not Predicate<TPayLoad>)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "InvalidDelegateRerefenceTypeException", typeof(Predicate<TPayLoad>).FullName), nameof(filterReference));
            }

            this.actionReference = actionReference;
            this.filterReference = filterReference;
        }

        /// <summary>
        /// Gets the target <see cref="System.Action{TPayLoad}"/> that is referenced by the <see cref="IDelegateReference"/>.
        /// </summary>
        /// <value>An <see cref="System.Action{TPayLoad}"/> or <see langword="null" /> if the referenced target is not alive.</value>
        public Func<TPayLoad, Task> Action => (Func<TPayLoad, Task>)actionReference.Target;

        /// <summary>
        /// Gets the delegate of the action.
        /// </summary>
        /// <value>The delegate.</value>
        public Delegate Delegate => actionReference.Target;

        /// <summary>
        /// Gets the target <see cref="Predicate{TPayLoad}"/> that is referenced by the <see cref="IDelegateReference"/>.
        /// </summary>
        /// <value>An <see cref="Predicate{TPayLoad}"/> or <see langword="null" /> if the referenced target is not alive.</value>
        public Predicate<TPayLoad> Filter => (Predicate<TPayLoad>)filterReference.Target;

        /// <summary>
        /// Gets or sets a <see cref="SubscriptionToken"/> that identifies this <see cref="IEventSubscription"/>.
        /// </summary>
        /// <value>A token that identifies this <see cref="IEventSubscription"/>.</value>
        public SubscriptionToken SubscriptionToken { get; set; }

        /// <summary>
        /// Gets the execution strategy to publish this event.
        /// </summary>
        /// <returns>An <see cref="System.Action{TPayLoad}"/> with the execution strategy, or <see langword="null" /> if the <see cref="IEventSubscription"/> is no longer valid.</returns>
        /// <remarks>
        /// If <see cref="Action"/> or <see cref="Filter"/> are no longer valid because they were
        /// garbage collected, this method will return <see langword="null" />.
        /// Otherwise it will return a delegate that evaluates the <see cref="Filter"/> and if it
        /// returns <see langword="true" /> will then call <see cref="InvokeAction"/>. The returned
        /// delegate holds hard references to the <see cref="Action"/> and <see cref="Filter"/> target
        /// <see cref="Delegate">delegates</see>. As long as the returned delegate is not garbage collected,
        /// the <see cref="Action"/> and <see cref="Filter"/> references delegates won't get collected either.
        /// </remarks>
        public virtual Func<object[], Task> GetExecutionStrategy()
        {
            Func<TPayLoad, Task> action = this.Action;
            Predicate<TPayLoad> filter = this.Filter;

            if (action != null && filter != null)
            {
                return (args) =>
                {
                    TPayLoad argument = default;
                    if (args != null && args.Length > 0 && args[0] != null)
                    {
                        argument = (TPayLoad)args[0];
                    }
                    if (filter(argument))
                    {
                        return InvokeAction(action, argument);
                    }
                    return AsyncHelpers.Return();
                };
            }
            return null;
        }

        /// <summary>
        /// Invokes the specified <see cref="System.Action{TPayLoad}"/> synchronously when not overridden.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="argument">The payload to pass <paramref name="action"/> while invoking it.</param>
        /// <exception cref="ArgumentNullException">An <see cref="ArgumentNullException"/> is thrown if <paramref name="action"/> is null.</exception>
        public virtual Task InvokeAction(Func<TPayLoad, Task> action, TPayLoad argument)
        {
            return action == null ? throw new ArgumentNullException(nameof(action)) : action(argument);
        }
    }
}