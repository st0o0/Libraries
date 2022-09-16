using System;
using System.Diagnostics.CodeAnalysis;

namespace WPFLibrary.EventSystem.SubscriptionTokens
{
    public class SubscriptionToken : IEquatable<SubscriptionToken>, IDisposable
    {
        private readonly Guid token;

        private Action<SubscriptionToken> unsubscriptionAction;

        /// <summary>
        /// Initializes a new instance of <see cref="SubscriptionToken"/>.
        /// </summary>
        public SubscriptionToken(Action<SubscriptionToken> unsubscribeAction)
        {
            this.unsubscriptionAction = unsubscribeAction;
            this.token = Guid.NewGuid();
        }

        public bool Equals([AllowNull] SubscriptionToken other)
        {
            if (other == null) { return false; };
            return Equals(token, other.token);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) { return true; };
            return Equals(obj as SubscriptionToken);
        }

        public override int GetHashCode()
        {
            return token.GetHashCode();
        }

        public virtual void Dispose()
        {
            if (this.unsubscriptionAction != null)
            {
                this.unsubscriptionAction(this);
                this.unsubscriptionAction = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}