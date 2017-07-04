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

        public event EventHandler<Exception> ExceptionThrew;

        void OnError(Exception error)
        {
            ExceptionThrew?.Invoke(this, error);
        }

        async void SubscribeSchedulerTask()
        {
            try
            {
                var scheduler = await SchedulerTask;
                scheduler.ExceptionThrew += (sender, e) =>
                {
                    OnError(e);
                };
            }
            catch (Exception e)
            {
                OnError(e);
            }
        }

        public void Start()
        {
            SubscribeSchedulerTask();
        }

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
            var schedulerTask = LoadSchedulerAsync(cts.Token);
            return new AppMain(cts, schedulerTask);
        }
    }
}
