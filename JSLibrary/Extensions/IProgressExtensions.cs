using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Extensions
{
    public static class IProgressExtensions
    {
        public static async Task Simulation(this IProgress<double> progress, int delay = 250, CancellationToken cancellationToken = default)
        {
            for (int i = 0; i <= 100; i++)
            {
                await Task.Delay(delay, cancellationToken);
                progress.Report(i);
            }
        }
    }
}