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
        private static int multiplicator = 2;

        public static void SetMultiplicator(int value) => multiplicator = value;

        public static int MaxDegreeOfParallelism => Convert.ToInt32(Math.Floor((Environment.ProcessorCount * 0.75) * multiplicator));

        public static async Task TaskManyAsync<InputType>(IEnumerable<InputType> items, Action<InputType> action, CancellationToken cancellationToken = default)
        {
            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                BoundedCapacity = items.Count(),
                EnsureOrdered = true
            };

            ActionBlock<InputType> ab = new(action, edflbo);

            foreach (InputType input in items)
            {
                await ab.SendAsync(input, cancellationToken);
            }

            ab.Complete();

            await ab.Completion;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, OutputType>(IEnumerable<InputType> items, Func<InputType, OutputType> func, CancellationToken cancellationToken = default)
        {
            SemaphoreSlim semaphoreSlim = new(1, 1);

            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                BoundedCapacity = items.Count(),
                EnsureOrdered = true
            };

            TransformBlock<InputType, OutputType> tb = new(func, edflbo);
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
            }, new ExecutionDataflowBlockOptions() { BoundedCapacity = items.Count(), CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2 });

            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in items)
            {
                await tb.SendAsync(input, cancellationToken);
            }
            tb.Complete();

            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, OutputType>(IEnumerable<InputType> items, Func<InputType, IEnumerable<OutputType>> func, CancellationToken cancellationToken = default)
        {
            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                BoundedCapacity = items.Count(),
                EnsureOrdered = true
            };

            TransformManyBlock<InputType, OutputType> tb = new(func, edflbo);
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
            }, new ExecutionDataflowBlockOptions() { BoundedCapacity = items.Count(), CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2 });

            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in items)
            {
                await tb.SendAsync(input, cancellationToken);
            }
            tb.Complete();

            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, OutputType>(IEnumerable<InputType> items, Func<InputType, Task<OutputType>> func, CancellationToken cancellationToken = default)
        {
            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                BoundedCapacity = items.Count(),
                EnsureOrdered = true
            };

            TransformBlock<InputType, OutputType> tb = new(func, edflbo);
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
            }, new ExecutionDataflowBlockOptions() { BoundedCapacity = items.Count(), CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2 });

            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in items)
            {
                await tb.SendAsync(input, cancellationToken);
            }
            tb.Complete();

            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, OutputType>(IEnumerable<InputType> items, Func<InputType, Task<IEnumerable<OutputType>>> func, CancellationToken cancellationToken = default)
        {
            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                BoundedCapacity = items.Count(),
                EnsureOrdered = true
            };

            TransformManyBlock<InputType, OutputType> tb = new(func, edflbo);
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
            }, new ExecutionDataflowBlockOptions() { BoundedCapacity = items.Count(), CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2 });

            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in items)
            {
                await tb.SendAsync(input, cancellationToken);
            }

            tb.Complete();

            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, MiddleType> func0, Func<MiddleType, OutputType> func1, CancellationToken cancellationToken = default)
        {
            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                BoundedCapacity = items.Count(),
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            TransformBlock<MiddleType, OutputType> tb = new(func1, edflbo);
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
            }, new ExecutionDataflowBlockOptions() { BoundedCapacity = items.Count(), CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2 });

            tmb.LinkTo(tb, new DataflowLinkOptions() { PropagateCompletion = true });
            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in items)
            {
                await tmb.SendAsync(input, cancellationToken);
            }
            tmb.Complete();

            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, MiddleType> func0, Func<MiddleType, IEnumerable<OutputType>> func1, CancellationToken cancellationToken = default)
        {
            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                BoundedCapacity = items.Count(),
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            TransformManyBlock<MiddleType, OutputType> tb = new(func1, edflbo);
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
            }, new ExecutionDataflowBlockOptions() { BoundedCapacity = items.Count(), CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2 });

            tmb.LinkTo(tb, new DataflowLinkOptions() { PropagateCompletion = true });
            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in items)
            {
                await tmb.SendAsync(input, cancellationToken);
            }
            tmb.Complete();

            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, IEnumerable<MiddleType>> func0, Func<MiddleType, OutputType> func1, CancellationToken cancellationToken = default)
        {
            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                BoundedCapacity = items.Count(),
                EnsureOrdered = true
            };

            TransformManyBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            TransformBlock<MiddleType, OutputType> tb = new(func1, edflbo);
            ActionBlock<OutputType> ab = new(x =>
            {
                outputs.Add(x);
            }, new ExecutionDataflowBlockOptions() { BoundedCapacity = items.Count(), CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2 });

            tmb.LinkTo(tb, new DataflowLinkOptions() { PropagateCompletion = true });
            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in items)
            {
                await tmb.SendAsync(input, cancellationToken);
            }
            tmb.Complete();

            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, IEnumerable<MiddleType>> func0, Func<MiddleType, IEnumerable<OutputType>> func1, CancellationToken cancellationToken = default)
        {
            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                BoundedCapacity = items.Count(),
                EnsureOrdered = true
            };

            TransformManyBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            TransformManyBlock<MiddleType, OutputType> tb = new(func1, edflbo);
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
            }, new ExecutionDataflowBlockOptions() { BoundedCapacity = items.Count(), CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2, SingleProducerConstrained = true, EnsureOrdered = true });

            tmb.LinkTo(tb, new DataflowLinkOptions() { PropagateCompletion = true });
            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in items)
            {
                await tmb.SendAsync(input, cancellationToken);
            }
            tmb.Complete();

            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, Task<MiddleType>> func0, Func<MiddleType, Task<OutputType>> func1, CancellationToken cancellationToken = default)
        {
            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                BoundedCapacity = items.Count(),
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            TransformBlock<MiddleType, OutputType> tb = new(func1, edflbo);
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
            }, new ExecutionDataflowBlockOptions() { BoundedCapacity = items.Count(), CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2 });

            tmb.LinkTo(tb, new DataflowLinkOptions() { PropagateCompletion = true });
            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in items)
            {
                await tmb.SendAsync(input, cancellationToken);
            }
            tmb.Complete();

            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, Task<MiddleType>> func0, Func<MiddleType, Task<IEnumerable<OutputType>>> func1, CancellationToken cancellationToken = default)
        {
            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                BoundedCapacity = items.Count(),
                EnsureOrdered = true
            };

            TransformBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            TransformManyBlock<MiddleType, OutputType> tb = new(func1, edflbo);
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
            }, new ExecutionDataflowBlockOptions() { BoundedCapacity = items.Count(), CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2 });

            tmb.LinkTo(tb, new DataflowLinkOptions() { PropagateCompletion = true });
            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in items)
            {
                await tmb.SendAsync(input, cancellationToken);
            }
            tmb.Complete();

            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, Task<IEnumerable<MiddleType>>> func0, Func<MiddleType, Task<OutputType>> func1, CancellationToken cancellationToken = default)
        {
            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                BoundedCapacity = items.Count(),
                EnsureOrdered = true
            };

            TransformManyBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            TransformBlock<MiddleType, OutputType> tb = new(func1, edflbo);
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
            }, new ExecutionDataflowBlockOptions() { BoundedCapacity = items.Count(), CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2 });

            tmb.LinkTo(tb, new DataflowLinkOptions() { PropagateCompletion = true });
            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in items)
            {
                await tmb.SendAsync(input, cancellationToken);
            }
            tmb.Complete();

            await ab.Completion;
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> items, Func<InputType, Task<IEnumerable<MiddleType>>> func0, Func<MiddleType, Task<IEnumerable<OutputType>>> func1, CancellationToken cancellationToken = default)
        {
            SemaphoreSlim semaphoreSlim = new(1, 1);
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken,
                BoundedCapacity = items.Count(),
                EnsureOrdered = true
            };

            TransformManyBlock<InputType, MiddleType> tmb = new(func0, edflbo);
            TransformManyBlock<MiddleType, OutputType> tb = new(func1, edflbo);
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
            }, new ExecutionDataflowBlockOptions() { BoundedCapacity = items.Count(), CancellationToken = cancellationToken, MaxDegreeOfParallelism = 2 });

            tmb.LinkTo(tb, new DataflowLinkOptions() { PropagateCompletion = true });
            tb.LinkTo(ab, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (InputType input in items)
            {
                await tmb.SendAsync(input, cancellationToken);
            }
            tmb.Complete();

            await ab.Completion;
            return outputs;
        }
    }
}