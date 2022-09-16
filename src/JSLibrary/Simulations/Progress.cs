using JSLibrary.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Simulations
{
    public static class Progress
    {
        public static async Task Simulation(IProgress<double> progress, int delay = 250, CancellationToken cancellationToken = default)
        {
            await progress.Simulation(delay, cancellationToken);
        }

        public static async Task Simulation(Action<double> action, int delay = 250, CancellationToken cancellationToken = default)
        {
            await new Progress<double>(action).Simulation(delay, cancellationToken);
        }
    }
}