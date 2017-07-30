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

namespace VainZero.Timermaid.ScheduleLists
{
    public sealed class ScheduleListPage
        : IDisposable
    {
        public Scheduler Scheduler { get; }

        public ScheduleViewList Schedules { get; }

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
            Schedules = new ScheduleViewList(scheduler, scheduler.Schedules.Select(s => new ScheduleView(s)), scheduler.Schedules);

            DisableCommand =
                new DelegateCommand<object>(
                    parameter =>
                    {
                        var schedule = (ScheduleView)parameter;
                        schedule.Status = ScheduleViewStatusModule.Switch(schedule.Status);
                    },
                    parameter => parameter is ScheduleView
                );

            HideCommand =
                new DelegateCommand<object>(
                    parameter =>
                    {
                        var schedule = (ScheduleView)parameter;
                        schedule.Status = ScheduleViewStatus.Hidden;
                    },
                    parameter => parameter is ScheduleView
                );
        }

        static TimeSpan AutoSaveDelay => TimeSpan.FromMilliseconds(500);

        public static ScheduleListPage Load(Scheduler scheduler)
        {
            var context = new AppDbContext();
            var autoSaving = scheduler.Schedules.EnableAutoSave(context, AutoSaveDelay);

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
