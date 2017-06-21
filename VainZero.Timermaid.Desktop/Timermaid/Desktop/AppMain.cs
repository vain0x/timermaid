using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VainZero.Timermaid.Data.Entity;
using VainZero.Timermaid.Scheduling;

namespace VainZero.Timermaid.Desktop
{
    public sealed class AppMain
        : IDisposable
    {
        CancellationTokenSource CancellationTokenSource { get; } =
            new CancellationTokenSource();

        CancellationToken CancellationToken =>
            CancellationTokenSource.Token;

        public Task<Scheduler> SchedulerTask { get; }

        public void Dispose()
        {
            CancellationTokenSource.Cancel();

            SchedulerTask.ContinueWith(task =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    task.Result.Dispose();
                }
            }).Wait();
        }

        public AppMain(CancellationTokenSource cts, Task<Scheduler> schedulerTask)
        {
            CancellationTokenSource = cts;
            SchedulerTask = schedulerTask;
        }

        static async Task<Scheduler> LoadSchedulerAsync(CancellationToken cancellationToken)
        {
            using (var context = new AppDbContext())
            {
                var schedules = await context.Schedules.ToArrayAsync(cancellationToken);
                return Scheduler.Load(schedules);
            }
        }

        public static AppMain Load()
        {
            var cts = new CancellationTokenSource();
            return new AppMain(cts, LoadSchedulerAsync(cts.Token));
        }
    }
}
