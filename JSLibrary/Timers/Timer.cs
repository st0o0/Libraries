using System;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Timers
{
    public class Timer<InputType> : MarshalByRefObject, IDisposable, IAsyncDisposable where InputType : class
    {
        private readonly Timer timer;

        public Timer(DateTime specificTime, TimerCallback<InputType> callback, InputType input, CancellationToken cancellationToken = default) : this(specificTime, async (x, ct) => await Task.Run(() => callback(x), ct), input, cancellationToken)
        {
        }

        public Timer(TimeSpan period, TimeSpan dueTime, TimerCallback<InputType> callback, InputType input, CancellationToken cancellationToken = default) : this(period, dueTime, async (x, ct) => await Task.Run(() => callback(x), ct), input, cancellationToken)
        {
        }

        public Timer(DateTime specificTime, TimerCallbackAsync<InputType> callback, InputType input, CancellationToken cancellationToken = default) : this(specificTime.ToUniversalTime().Subtract(DateTime.UtcNow) > TimeSpan.Zero ? specificTime.ToUniversalTime().Subtract(DateTime.UtcNow) : specificTime.AddDays(1).ToUniversalTime().Subtract(DateTime.UtcNow), TimeSpan.FromDays(1), callback, input, cancellationToken)
        {
        }

        public Timer(TimeSpan period, TimeSpan dueTime, TimerCallbackAsync<InputType> callback, InputType input, CancellationToken cancellationToken = default)
        {
            this.timer = new Timer(async x => await callback(x as InputType, cancellationToken), input, dueTime, period);
        }

        public bool Change(int dueTime, int period) => this.timer.Change(dueTime, period);

        public bool Change(long dueTime, long period) => this.timer.Change(dueTime, period);

        public bool Change(TimeSpan dueTime, TimeSpan period) => this.timer.Change(dueTime, period);

        public bool Change(DateTime specificTime) => this.timer.Change(specificTime.ToUniversalTime().Subtract(DateTime.UtcNow) > TimeSpan.Zero ? specificTime.ToUniversalTime().Subtract(DateTime.UtcNow) : specificTime.AddDays(1).ToUniversalTime().Subtract(DateTime.UtcNow), TimeSpan.FromDays(1));

        public void Dispose()
        {
            timer.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await timer.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        public delegate Task TimerCallbackAsync<ModelType>(ModelType model, CancellationToken cancellationToken) where ModelType : class;

        public delegate void TimerCallback<ModelType>(ModelType model) where ModelType : class;
    }
}