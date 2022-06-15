using System;

namespace JSLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
    public class NotEmptyAttribute : Attribute
    {
        public NotEmptyAttribute()
        {
        }
    }
}