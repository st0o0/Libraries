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
        public static async Task TaskManyAsync<InputType>(IEnumerable<InputType> inputs, Action<InputType> action, CancellationToken cancellationToken = default, int degreeParalllel = 5)
        {
            ActionBlock<InputType> actionBlock = new(action, new() { MaxDegreeOfParallelism = degreeParalllel, CancellationToken = cancellationToken });

            inputs.ToList().ForEach(async x => await actionBlock.SendAsync(x, cancellationToken));

            actionBlock.Complete();

            await actionBlock.Completion;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, OutputType>(IEnumerable<InputType> inputs, Func<InputType, OutputType> func, CancellationToken cancellationToken = default, int degreeParallel = 5)
        {
            TransformBlock<InputType, OutputType> transformBlock = new(func, new() { MaxDegreeOfParallelism = 5, CancellationToken = cancellationToken });

            inputs.ToList().ForEach(async x => await transformBlock.SendAsync(x, cancellationToken));

            transformBlock.Complete();

            await transformBlock.Completion;

            ICollection<OutputType> outputs = new List<OutputType>();

            if (transformBlock.InputCount == transformBlock.OutputCount)
            {
                for (int i = 0; i < transformBlock.OutputCount; i++)
                {
                    outputs.Add(await transformBlock.ReceiveAsync(cancellationToken));
                }
            }
            else
            {
                throw new Exception();
            }
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, OutputType>(IEnumerable<InputType> inputs, Func<InputType, Task<OutputType>> func, CancellationToken cancellationToken = default, int degreeParallel = 5)
        {
            TransformBlock<InputType, OutputType> transformBlock = new(func, new() { MaxDegreeOfParallelism = 5, CancellationToken = cancellationToken });

            inputs.ToList().ForEach(async x => await transformBlock.SendAsync(x, cancellationToken));

            transformBlock.Complete();

            await transformBlock.Completion;

            ICollection<OutputType> outputs = new List<OutputType>();

            if (transformBlock.InputCount == transformBlock.OutputCount)
            {
                for (int i = 0; i < transformBlock.OutputCount; i++)
                {
                    outputs.Add(await transformBlock.ReceiveAsync(cancellationToken));
                }
            }
            else
            {
                throw new Exception();
            }
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> inputs, Func<InputType, IEnumerable<MiddleType>> func, Func<MiddleType, OutputType> func1, CancellationToken cancellationToken = default, int degreeParallel = 5)
        {
            ExecutionDataflowBlockOptions edflbo = new() { MaxDegreeOfParallelism = degreeParallel, CancellationToken = cancellationToken };

            TransformManyBlock<InputType, MiddleType> transformManyBlock = new(func, edflbo);

            TransformBlock<MiddleType, OutputType> transformBlock = new(func1, edflbo);

            transformManyBlock.LinkTo(transformBlock, new() { PropagateCompletion = true });

            inputs.ToList().ForEach(async x => await transformManyBlock.SendAsync(x, cancellationToken));

            transformManyBlock.Complete();

            await transformBlock.Completion;

            ICollection<OutputType> outputs = new List<OutputType>();

            if (transformBlock.InputCount == transformBlock.OutputCount)
            {
                for (int i = 0; i < transformBlock.OutputCount; i++)
                {
                    outputs.Add(await transformBlock.ReceiveAsync(cancellationToken));
                }
            }
            else
            {
                throw new Exception();
            }
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> inputs, Func<InputType, Task<IEnumerable<MiddleType>>> func, Func<MiddleType, OutputType> func1, CancellationToken cancellationToken = default, int degreeParallel = 5)
        {
            ExecutionDataflowBlockOptions edflbo = new() { MaxDegreeOfParallelism = degreeParallel, CancellationToken = cancellationToken };

            TransformManyBlock<InputType, MiddleType> transformManyBlock = new(func, edflbo);

            TransformBlock<MiddleType, OutputType> transformBlock = new(func1, edflbo);

            transformManyBlock.LinkTo(transformBlock, new() { PropagateCompletion = true });

            inputs.ToList().ForEach(async x => await transformManyBlock.SendAsync(x, cancellationToken));

            transformManyBlock.Complete();

            await transformBlock.Completion;

            ICollection<OutputType> outputs = new List<OutputType>();

            if (transformBlock.InputCount == transformBlock.OutputCount)
            {
                for (int i = 0; i < transformBlock.OutputCount; i++)
                {
                    outputs.Add(await transformBlock.ReceiveAsync(cancellationToken));
                }
            }
            else
            {
                throw new Exception();
            }
            return outputs;
        }
    }
}