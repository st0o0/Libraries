using System;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Timers
{
    public class Timer<InputType> : MarshalByRefObject, IAsyncDisposable, IDisposable where InputType : class
    {
        private readonly Timer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer{InputType}"/> class.
        /// </summary>
        /// <param name="specificTime">The specific time in GMT.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="input">The input.</param>
        public Timer(DateTime specificTime, TimerCallback<InputType> callback, InputType input) : this(specificTime, TimeSpan.FromDays(1), callback, input)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer{InputType}"/> class.
        /// </summary>
        /// <param name="specificTime">The specific time in GMT.</param>
        /// <param name="period">The period.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="input">The input.</param>
        public Timer(DateTime specificTime, TimeSpan period, TimerCallback<InputType> callback, InputType input) : this(specificTime.Subtract(DateTime.UtcNow) > TimeSpan.Zero ? specificTime.Subtract(DateTime.UtcNow) : specificTime.AddDays(1).Subtract(DateTime.UtcNow), period, callback, input)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer{InputType}"/> class.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="input">The input.</param>
        public Timer(TimeSpan dueTime, TimeSpan period, TimerCallback<InputType> callback, InputType input)
        {
            this.timer = new Timer(x => callback(x as InputType), input, dueTime, period);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer{InputType}"/> class.
        /// </summary>
        /// <param name="specificTime">The specific time in GMT.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="input">The input.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public Timer(DateTime specificTime, TimerCallbackAsync<InputType> callback, InputType input, CancellationToken cancellationToken = default) : this(specificTime, TimeSpan.FromDays(1), callback, input, cancellationToken)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer{InputType}"/> class.
        /// </summary>
        /// <param name="specificTime">The specific time in GMT.</param>
        /// <param name="period">The period.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="input">The input.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public Timer(DateTime specificTime, TimeSpan period, TimerCallbackAsync<InputType> callback, InputType input, CancellationToken cancellationToken = default) : this(specificTime.Subtract(DateTime.UtcNow) > TimeSpan.Zero ? specificTime.Subtract(DateTime.UtcNow) : specificTime.AddDays(1).Subtract(DateTime.UtcNow), period, callback, input, cancellationToken)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer{InputType}"/> class.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="input">The input.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public Timer(TimeSpan dueTime, TimeSpan period, TimerCallbackAsync<InputType> callback, InputType input, CancellationToken cancellationToken = default)
        {
            this.timer = new Timer(async (x) => await callback(x as InputType, cancellationToken), input, dueTime, period);
        }

        /// <summary>
        /// Changes the specified due time.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <returns></returns>
        public bool Change(int dueTime, int period) => this.timer.Change(dueTime, period);

        /// <summary>
        /// Changes the specified due time.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <returns></returns>
        public bool Change(long dueTime, long period) => this.timer.Change(dueTime, period);

        /// <summary>
        /// Changes the specified due time.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <returns></returns>
        public bool Change(TimeSpan dueTime, TimeSpan period) => this.timer.Change(dueTime, period);

        /// <summary>
        /// Changes the specified specific time.
        /// </summary>
        /// <param name="specificTime">The specific time in GMT.</param>
        /// <returns></returns>
        public bool Change(DateTime specificTime) => this.timer.Change(specificTime.Subtract(DateTime.UtcNow) > TimeSpan.Zero ? specificTime.Subtract(DateTime.UtcNow) : specificTime.AddDays(1).Subtract(DateTime.UtcNow), TimeSpan.FromDays(1));

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.timer.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            await this.timer.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="ModelType">The type of the odel type.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public delegate Task TimerCallbackAsync<ModelType>(ModelType model, CancellationToken cancellationToken) where ModelType : class;

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="ModelType">The type of the odel type.</typeparam>
        /// <param name="model">The model.</param>
        public delegate void TimerCallback<ModelType>(ModelType model) where ModelType : class;
    }
}