using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace JSLibrary.TPL
{
    public static class ParallelTask
    {
        private static int maxDegreeOfParallelism = Convert.ToInt32(Math.Floor((Environment.ProcessorCount * 0.75) * 2));

        public static void SetMultiplicator(int value) => maxDegreeOfParallelism = Convert.ToInt32(Math.Floor((Environment.ProcessorCount * 0.75) * value));

        public static int MaxDegreeOfParallelism => maxDegreeOfParallelism;

        public static async Task TaskManyAsync<InputType>(IEnumerable<InputType> items, Action<InputType> func0, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            ActionBlock<InputType> ab = new(func0, edflbo);

            foreach (InputType item in new List<InputType>(items))
            {
                await ab.SendAsync(item, cancellationToken);
            }

            ab.Complete();
            await ab.Completion;
        }

        public static async Task TaskManyAsync<InputType>(IEnumerable<InputType> items, Func<InputType, Task> func0, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            ActionBlock<InputType> ab = new(func0, edflbo);

            foreach (InputType item in new List<InputType>(items))
            {
                await ab.SendAsync(item, cancellationToken);
            }

            ab.Complete();
            await ab.Completion;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, OutputType>(IEnumerable<InputType> items, Func<InputType, OutputType> func0, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformBlock<InputType, OutputType> tb = new(func0, edflbo);
            ActionBlock<OutputType> ab = new(async item =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(item);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }, edflbo);

            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tb.SendAsync(item, cancellationToken);
            }

            tb.Complete();
            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, OutputType>(IEnumerable<InputType> items, Func<InputType, IEnumerable<OutputType>> func0, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformManyBlock<InputType, OutputType> tmb = new(func0, edflbo);
            ActionBlock<OutputType> ab = new(async item =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(item);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }, edflbo);

            tmb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tmb.SendAsync(item, cancellationToken);
            }

            tmb.Complete();
            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, OutputType>(IEnumerable<InputType> items, Func<InputType, Task<OutputType>> func0, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformBlock<InputType, OutputType> tb = new(func0, edflbo);
            ActionBlock<OutputType> ab = new(async x =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(x);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }, new ExecutionDataflowBlockOptions() { CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2 });

            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tb.SendAsync(item, cancellationToken);
            }

            tb.Complete();
            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, OutputType>(IEnumerable<InputType> items, Func<InputType, Task<IEnumerable<OutputType>>> func0, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformManyBlock<InputType, OutputType> tb = new(func0, edflbo);
            ActionBlock<OutputType> ab = new(async item =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(item);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }, edflbo);

            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tb.SendAsync(item, cancellationToken);
            }

            tb.Complete();
            await ab.Completion;
            return outputs;
        }

        public static async Task TaskManyAsync<InputType, MiddleType>(IEnumerable<InputType> items, Func<InputType, MiddleType> func0, Action<MiddleType> action, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tb = new(func0, edflbo);
            ActionBlock<MiddleType> ab = new(action, edflbo);

            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in new List<InputType>(items))
            {
                await tb.SendAsync(input, cancellationToken);
            }
            tb.Complete();
            await ab.Completion;
        }

        public static async Task TaskManyAsync<InputType, MiddleType>(IEnumerable<InputType> items, Func<InputType, Task<MiddleType>> func0, Action<MiddleType> action, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tb = new(func0, edflbo);
            ActionBlock<MiddleType> ab = new(action, edflbo);

            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in new List<InputType>(items))
            {
                await tb.SendAsync(input, cancellationToken);
            }
            tb.Complete();
            await ab.Completion;
        }

        public static async Task TaskManyAsync<InputType, MiddleType>(IEnumerable<InputType> items, Func<InputType, MiddleType> func0, Func<MiddleType, Task> func1, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));
            ArgumentNullException.ThrowIfNull(func1, nameof(func1));

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            ActionBlock<MiddleType> ab = new(func1, edflbo);

            tmb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in new List<InputType>(items))
            {
                await tmb.SendAsync(input, cancellationToken);
            }
            tmb.Complete();
            await ab.Completion;
        }

        public static async Task TaskManyAsync<InputType, MiddleType>(IEnumerable<InputType> items, Func<InputType, Task<MiddleType>> func0, Func<MiddleType, Task> func1, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));
            ArgumentNullException.ThrowIfNull(func1, nameof(func1));

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            ActionBlock<MiddleType> ab = new(func1, edflbo);

            tmb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in new List<InputType>(items))
            {
                await tmb.SendAsync(input, cancellationToken);
            }
            tmb.Complete();
            await ab.Completion;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, MiddleType> func0, Func<MiddleType, OutputType> func1, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));
            ArgumentNullException.ThrowIfNull(func1, nameof(func1));

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tb = new(func0, edflbo);
            TransformBlock<MiddleType, OutputType> tb1 = new(func1, edflbo);
            ActionBlock<OutputType> ab = new(async item =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(item);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }, edflbo);

            tb.LinkTo(tb1, new DataflowLinkOptions() { PropagateCompletion = true });
            tb1.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tb.SendAsync(item, cancellationToken);
            }

            tb.Complete();
            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, MiddleType> func0, Func<MiddleType, OutputType> func1, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));
            ArgumentNullException.ThrowIfNull(func1, nameof(func1));

            int itemCount = items.Count();

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tb = new(func0, edflbo);
            TransformBlock<MiddleType, OutputType> tb1 = new(func1, edflbo);
            ActionBlock<OutputType> ab = new(async item =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(item);
                }
                finally
                {
                    progress.Report((itemCount / outputs.Count) * 100.0);
                    semaphoreSlim.Release();
                }
            }, edflbo);

            tb.LinkTo(tb1, new DataflowLinkOptions() { PropagateCompletion = true });
            tb1.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tb.SendAsync(item, cancellationToken);
            }

            tb.Complete();
            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, MiddleType> func0, Func<MiddleType, IEnumerable<OutputType>> func1, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));
            ArgumentNullException.ThrowIfNull(func1, nameof(func1));

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tb = new(func0, edflbo);
            TransformManyBlock<MiddleType, OutputType> tmb = new(func1, edflbo);
            ActionBlock<OutputType> ab = new(async item =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(item);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }, edflbo);

            tb.LinkTo(tmb, new DataflowLinkOptions() { PropagateCompletion = true });
            tmb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tb.SendAsync(item, cancellationToken);
            }

            tb.Complete();
            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, IEnumerable<MiddleType>> func0, Func<MiddleType, OutputType> func1, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));
            ArgumentNullException.ThrowIfNull(func1, nameof(func1));

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformManyBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            TransformBlock<MiddleType, OutputType> tb = new(func1, edflbo);
            ActionBlock<OutputType> ab = new(async item =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(item);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }, edflbo);

            tmb.LinkTo(tb, new DataflowLinkOptions() { PropagateCompletion = true });
            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tmb.SendAsync(item, cancellationToken);
            }

            tmb.Complete();
            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, IEnumerable<MiddleType>> func0, Func<MiddleType, IEnumerable<OutputType>> func1, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));
            ArgumentNullException.ThrowIfNull(func1, nameof(func1));

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformManyBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            TransformManyBlock<MiddleType, OutputType> tmb1 = new(func1, edflbo);
            ActionBlock<OutputType> ab = new(async item =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(item);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }, edflbo);

            tmb.LinkTo(tmb1, new DataflowLinkOptions() { PropagateCompletion = true });
            tmb1.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tmb.SendAsync(item, cancellationToken);
            }

            tmb.Complete();
            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, Task<MiddleType>> func0, Func<MiddleType, Task<OutputType>> func1, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));
            ArgumentNullException.ThrowIfNull(func1, nameof(func1));

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tb = new(func0, edflbo);
            TransformBlock<MiddleType, OutputType> tb1 = new(func1, edflbo);
            ActionBlock<OutputType> ab = new(async item =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(item);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }, edflbo);

            tb.LinkTo(tb1, new DataflowLinkOptions() { PropagateCompletion = true });
            tb1.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tb.SendAsync(item, cancellationToken);
            }

            tb.Complete();
            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, Task<MiddleType>> func0, Func<MiddleType, Task<OutputType>> func1, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));
            ArgumentNullException.ThrowIfNull(func1, nameof(func1));

            int itemCount = items.Count();

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tb = new(func0, edflbo);
            TransformBlock<MiddleType, OutputType> tb1 = new(func1, edflbo);
            ActionBlock<OutputType> ab = new(async item =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(item);
                }
                finally
                {
                    progress.Report((itemCount / outputs.Count) * 100.0);
                    semaphoreSlim.Release();
                }
            }, edflbo);

            tb.LinkTo(tb1, new DataflowLinkOptions() { PropagateCompletion = true });
            tb1.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tb.SendAsync(item, cancellationToken);
            }

            tb.Complete();
            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, Task<MiddleType>> func0, Func<MiddleType, Task<IEnumerable<OutputType>>> func1, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));
            ArgumentNullException.ThrowIfNull(func1, nameof(func1));

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tb = new(func0, edflbo);
            TransformManyBlock<MiddleType, OutputType> tmb = new(func1, edflbo);
            ActionBlock<OutputType> ab = new(async item =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(item);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }, edflbo);

            tb.LinkTo(tmb, new DataflowLinkOptions() { PropagateCompletion = true });
            tmb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tb.SendAsync(item, cancellationToken);
            }

            tb.Complete();
            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, Task<IEnumerable<MiddleType>>> func0, Func<MiddleType, Task<OutputType>> func1, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));
            ArgumentNullException.ThrowIfNull(func1, nameof(func1));

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformManyBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            TransformBlock<MiddleType, OutputType> tb = new(func1, edflbo);
            ActionBlock<OutputType> ab = new(async item =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(item);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }, edflbo);

            tmb.LinkTo(tb, new DataflowLinkOptions() { PropagateCompletion = true });
            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tmb.SendAsync(item, cancellationToken);
            }

            tmb.Complete();
            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, Task<IEnumerable<MiddleType>>> func0, Func<MiddleType, Task<IEnumerable<OutputType>>> func1, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));
            ArgumentNullException.ThrowIfNull(func0, nameof(func0));
            ArgumentNullException.ThrowIfNull(func1, nameof(func1));

            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                EnsureOrdered = true
            };

            TransformManyBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            TransformManyBlock<MiddleType, OutputType> tmb2 = new(func1, edflbo);
            ActionBlock<OutputType> ab = new(async item =>
            {
                try
                {
                    await semaphoreSlim.WaitAsync();
                    outputs.Add(item);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }, edflbo);

            tmb.LinkTo(tmb2, new DataflowLinkOptions() { PropagateCompletion = true });
            tmb2.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType item in new List<InputType>(items))
            {
                await tmb.SendAsync(item, cancellationToken);
            }

            tmb.Complete();
            await ab.Completion;
            return outputs;
        }
    }
}