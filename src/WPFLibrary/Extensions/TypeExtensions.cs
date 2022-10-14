using System;

namespace WPFLibrary.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsNullable(this Type type)
        {
            if (!type.IsValueType)
            {
                return true;
            }
            if (Nullable.GetUnderlyingType(type) != null)
            {
                return true;
            }
            return false;
        }
    }
}