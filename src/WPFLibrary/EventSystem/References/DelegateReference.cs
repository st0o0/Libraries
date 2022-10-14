using System;
using System.Reflection;

namespace WPFLibrary.EventSystem.References
{
    public class DelegateReference : IDelegateReference
    {
        private readonly Delegate @delegate;
        private readonly WeakReference weakReference;
        private readonly MethodInfo method;
        private readonly Type delegateType;

        /// <summary>
        /// Initializes a new instance of <see cref="DelegateReference"/>.
        /// </summary>
        /// <param name="delegate">The original <see cref="Delegate"/> to create a reference for.</param>
        /// <param name="keepReferenceAlive">If <see langword="false" /> the class will create a weak reference to the delegate, allowing it to be garbage collected. Otherwise it will keep a strong reference to the target.</param>
        /// <exception cref="ArgumentNullException">If the passed <paramref name="delegate"/> is not assignable to <see cref="Delegate"/>.</exception>
        public DelegateReference(Delegate @delegate, bool keepReferenceAlive)
        {
            if (@delegate == null)
                throw new ArgumentNullException(nameof(@delegate));

            if (keepReferenceAlive)
            {
                this.@delegate = @delegate;
            }
            else
            {
                this.weakReference = new WeakReference(@delegate.Target);
                this.method = @delegate.GetMethodInfo();
                this.delegateType = @delegate.GetType();
            }
        }

        /// <summary>
        /// Gets the <see cref="Delegate" /> (the target) referenced by the current <see cref="DelegateReference"/> object.
        /// </summary>
        /// <value><see langword="null"/> if the object referenced by the current <see cref="DelegateReference"/> object has been garbage collected; otherwise, a reference to the <see cref="Delegate"/> referenced by the current <see cref="DelegateReference"/> object.</value>
        public Delegate Target
        {
            get
            {
                if (this.@delegate != null)
                {
                    return this.@delegate;
                }
                else
                {
                    return TryGetDelegate();
                }
            }
        }

        /// <summary>
        /// Checks if the <see cref="Delegate" /> (the target) referenced by the current <see cref="DelegateReference"/> object are equal to another <see cref="Delegate" />.
        /// This is equivalent with comparing <see cref="Target"/> with <paramref name="delegate"/>, only more efficient.
        /// </summary>
        /// <param name="delegate">The other delegate to compare with.</param>
        /// <returns>True if the target referenced by the current object are equal to <paramref name="delegate"/>.</returns>
        public bool TargetEquals(Delegate @delegate)
        {
            if (this.@delegate != null)
            {
                return this.@delegate == @delegate;
            }
            if (@delegate == null)
            {
                return !this.method.IsStatic && !this.weakReference.IsAlive;
            }
            return this.weakReference.Target == @delegate.Target && Equals(this.method, @delegate.GetMethodInfo());
        }

        private Delegate TryGetDelegate()
        {
            if (this.method.IsStatic)
            {
                return this.method.CreateDelegate(this.delegateType, null);
            }
            object target = this.weakReference.Target;
            if (target != null)
            {
                return this.method.CreateDelegate(this.delegateType, target);
            }
            return null;
        }
    }
}