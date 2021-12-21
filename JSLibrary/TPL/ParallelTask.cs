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
        public static async Task TaskManyAsync<InputType>(IEnumerable<InputType> inputs, Action<InputType> action, int degreeParallel = 5, CancellationToken cancellationToken = default)
        {
            ActionBlock<InputType> actionBlock = new(action, new() { MaxDegreeOfParallelism = degreeParallel, CancellationToken = cancellationToken });

            inputs.ToList().ForEach(x => actionBlock.SendAsync(x, cancellationToken));

            actionBlock.Complete();

            await actionBlock.Completion;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, OutputType>(IEnumerable<InputType> inputs, Func<InputType, OutputType> func, int degreeParallel = 5, CancellationToken cancellationToken = default)
        {
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new()
            {
                MaxDegreeOfParallelism = degreeParallel,
                CancellationToken = cancellationToken
            };

            TransformBlock<InputType, OutputType> transformBlock = new(func, edflbo);

            ActionBlock<OutputType> actionBlock = new(input =>
            {
                outputs.Add(input);
            }, edflbo);

            transformBlock.LinkTo(actionBlock, new() { PropagateCompletion = true, });

            inputs.ToList().ForEach(x => transformBlock.SendAsync(x, cancellationToken));

            transformBlock.Complete();

            await actionBlock.Completion;

            if (inputs.Count() != outputs.Count)
            {
                throw new Exception();
            }
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, OutputType>(IEnumerable<InputType> inputs, Func<InputType, Task<OutputType>> func, int degreeParallel = 5, CancellationToken cancellationToken = default)
        {
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new() { MaxDegreeOfParallelism = degreeParallel, CancellationToken = cancellationToken };

            TransformBlock<InputType, OutputType> transformBlock = new(func, edflbo);

            ActionBlock<OutputType> actionBlock = new(input =>
            {
                outputs.Add(input);
            }, edflbo);

            transformBlock.LinkTo(actionBlock, new() { PropagateCompletion = true });

            inputs.ToList().ForEach(x => transformBlock.SendAsync(x, cancellationToken));

            transformBlock.Complete();

            await actionBlock.Completion;

            if (inputs.Count() != outputs.Count)
            {
                throw new Exception();
            }
            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> inputs, Func<InputType, IEnumerable<MiddleType>> func, Func<MiddleType, OutputType> func1, int degreeParallel = 5, CancellationToken cancellationToken = default)
        {
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new() { MaxDegreeOfParallelism = degreeParallel, CancellationToken = cancellationToken };

            TransformManyBlock<InputType, MiddleType> transformManyBlock = new(func, edflbo);

            TransformBlock<MiddleType, OutputType> transformBlock = new(func1, edflbo);

            ActionBlock<OutputType> actionBlock = new(input =>
            {
                outputs.Add(input);
            }, edflbo);

            transformManyBlock.LinkTo(transformBlock, new() { PropagateCompletion = true });

            transformBlock.LinkTo(actionBlock, new() { PropagateCompletion = true });

            inputs.ToList().ForEach(x => transformManyBlock.SendAsync(x, cancellationToken));

            transformManyBlock.Complete();

            await actionBlock.Completion;

            if (inputs.Count() != outputs.Count)
            {
                throw new Exception();
            }

            return outputs;
        }

        public static async Task<IEnumerable<OutputType>> TaskManyAsync<InputType, MiddleType, OutputType>(IEnumerable<InputType> inputs, Func<InputType, Task<IEnumerable<MiddleType>>> func, Func<MiddleType, OutputType> func1, int degreeParallel = 5, CancellationToken cancellationToken = default)
        {
            List<OutputType> outputs = new();

            ExecutionDataflowBlockOptions edflbo = new() { MaxDegreeOfParallelism = degreeParallel, CancellationToken = cancellationToken };

            TransformManyBlock<InputType, MiddleType> transformManyBlock = new(func, edflbo);

            TransformBlock<MiddleType, OutputType> transformBlock = new(func1, edflbo);

            ActionBlock<OutputType> actionBlock = new(input =>
            {
                outputs.Add(input);
            }, edflbo);

            transformManyBlock.LinkTo(transformBlock, new() { PropagateCompletion = true });

            transformBlock.LinkTo(actionBlock, new() { PropagateCompletion = true });

            inputs.ToList().ForEach(x => transformManyBlock.SendAsync(x, cancellationToken));

            transformManyBlock.Complete();

            await actionBlock.Completion;

            if (inputs.Count() != outputs.Count)
            {
                throw new Exception();
            }

            return outputs;
        }
    }
}