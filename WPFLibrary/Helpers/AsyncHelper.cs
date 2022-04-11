using System.Threading.Tasks;

namespace WPFLibrary.Helpers
{
    public static class AsyncHelpers
    {
        /// <summary>
        /// Return typed Task and the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Task<T> Return<T>(T value)
        {
            return Task<T>.FromResult(value);
        }

        /// <summary>
        /// Return Task.
        /// </summary>
        public static Task Return()
        {
            return Task<bool>.FromResult(false);
        }
    }
}