using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using VainZero.Collections.ObjectModel;
using VainZero.Timermaid.Data.Entity;
using VainZero.Timermaid.Scheduling;
using VainZero.Timermaid.UI.Logging;
using VainZero.Timermaid.UI.Notifications;

namespace VainZero.Timermaid.ScheduleLists
{
    public sealed class ScheduleListPage
        : IDisposable
    {
        public Scheduler Scheduler { get; }

        public BindableCollection<Schedule> Schedules => Scheduler.Schedules;

        public DelegateCommand<object> DisableCommand { get; }
        public DelegateCommand<object> HideCommand { get; }

        Action DisposeCore { get; }
        public void Dispose()
        {
            DisposeCore();
        }

        ScheduleListPage(Scheduler scheduler, Action dispose)
        {
            Scheduler = scheduler;
            DisposeCore = dispose;

            DisableCommand =
                new DelegateCommand<object>(
                    parameter =>
                    {
                        var schedule = (Schedule)parameter;
                        schedule.Status =
                            schedule.Status == ScheduleStatus.Enabled
                                ? ScheduleStatus.Disabled :
                            schedule.Status == ScheduleStatus.Disabled
                                ? ScheduleStatus.Enabled :
                            schedule.Status;
                    },
                    parameter => parameter is Schedule
                );

            HideCommand =
                new DelegateCommand<object>(
                    parameter =>
                    {
                        var schedule = (Schedule)parameter;
                        schedule.Status = ScheduleStatus.Hidden;
                    },
                    parameter => parameter is Schedule
                );
        }

        static TimeSpan AutoSaveDelay => TimeSpan.FromMilliseconds(500);

        public static ScheduleListPage Load(Scheduler scheduler, INotifier notifier, ILogger logger)
        {
            var context = new AppDbContext();
            var autoSaving =
                scheduler.Schedules
                .EnableAutoSave(
                    context,
                    AutoSaveDelay,
                    () => logger.Add("Saved schedules."),
                    notifier.NotifyError
                );

            var dispose =
                new Action(() =>
                {
                    autoSaving.Dispose();
                    context.Dispose();
                });
            return new ScheduleListPage(scheduler, dispose);
        }
    }
}
