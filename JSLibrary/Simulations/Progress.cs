using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSLibrary.Simulations
{
    public static class Progress
    {
        public static async Task Simulation(IProgress<double> progress, int delay = 250, CancellationToken cancellationToken = default)
        {
            for(int i = 0; i <= 100; i++)
            {
                await Task.Delay(delay, cancellationToken);
                progress.Report(i);
            }
        }

        public static async Task Simulation(Action<double> action, int delay = 250, CancellationToken cancellationToken = default)
        {
            for(int i = 0; i <= 100; i++)
            {
                await Task.Delay(delay, cancellationToken);
                action.Invoke(i);
            }
        }
    }
}
