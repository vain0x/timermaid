using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VainZero.Timermaid.Data.Entity;
using VainZero.Timermaid.Scheduling;
using VainZero.Timermaid.UI.Logging;
using VainZero.Timermaid.UI.Notifications;

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

        static async Task<Scheduler> LoadSchedulerAsync(INotifier notifier, ILogger logger, CancellationToken ct)
        {
            using (var context = new AppDbContext())
            {
                var schedules = await context.Schedules.ToArrayAsync(ct).ConfigureAwait(false);
                return Scheduler.Load(schedules, notifier, logger);
            }
        }

        public static AppMain Load(INotifier notifier, ILogger logger)
        {
            var cts = new CancellationTokenSource();
            var schedulerTask =
                LoadSchedulerAsync(notifier, logger, cts.Token)
                .NotifiedBy(notifier);
            return new AppMain(cts, schedulerTask);
        }
    }
}
