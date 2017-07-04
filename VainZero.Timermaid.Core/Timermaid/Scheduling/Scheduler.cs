using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VainZero.Collections.ObjectModel;
using VainZero.Timermaid.Data.Entity;

namespace VainZero.Timermaid.Scheduling
{
    public sealed class Scheduler
        : IDisposable
    {
        public BindableCollection<Schedule> Schedules { get; }

        #region Timer
        Dictionary<Schedule, IDisposable> Timers { get; } =
            new Dictionary<Schedule, IDisposable>();

        public event EventHandler<ScheduleExecutionException> ExceptionThrew;

        public void AddSchedule(Schedule schedule)
        {
            if (Timers.ContainsKey(schedule)) return;
            if (schedule.Status != ScheduleStatus.Enabled) return;

            var dueTime = schedule.DueTime - DateTime.Now;
            if (dueTime <= TimeSpan.Zero) return;

            var timer =
                new Timer(
                    state =>
                    {
                        try
                        {
                            Process.Start(schedule.FilePath, schedule.Argument);
                        }
                        catch (Exception ex)
                        {
                            ExceptionThrew?.Invoke(
                                this,
                                new ScheduleExecutionException(schedule, ex)
                            );
                        }
                    },
                    default(object),
                    dueTime,
                    Timeout.InfiniteTimeSpan
                );
            Timers.Add(schedule, timer);
        }

        void RemoveSchedule(Schedule schedule)
        {
            if (Timers.TryGetValue(schedule, out var timer))
            {
                timer.Dispose();
                Timers.Remove(schedule);
            }
        }

        void Reset(Schedule schedule)
        {
            RemoveSchedule(schedule);
            AddSchedule(schedule);
        }

        void ResetAll()
        {
            foreach (var kv in Timers)
            {
                kv.Value.Dispose();
            }
        }

        void OnScheduleAdded(object sender, Schedule e)
        {
            AddSchedule(e);
        }

        void OnScheduleRemoved(object sender, Schedule e)
        {
            RemoveSchedule(e);
        }

        void OnScheduleChanged(object sender, Schedule e)
        {
            RemoveSchedule(e);
            AddSchedule(e);
        }
        #endregion

        void Attach()
        {
            foreach (var schedule in Schedules)
            {
                AddSchedule(schedule);
            }

            Schedules.Added += OnScheduleAdded;
            Schedules.Removed += OnScheduleRemoved;
            Schedules.ItemChanged += OnScheduleChanged;
        }

        void Detach()
        {
            Schedules.Added -= OnScheduleAdded;
            Schedules.Removed -= OnScheduleRemoved;
            Schedules.ItemChanged -= OnScheduleChanged;

            ResetAll();
        }

        public void Dispose()
        {
            Detach();
        }

        Scheduler(BindableCollection<Schedule> schedules)
        {
            Schedules = schedules;
        }

        public static Scheduler Load(IEnumerable<Schedule> schedules)
        {
            var scheduler = new Scheduler(new BindableCollection<Schedule>(schedules));
            scheduler.Attach();
            return scheduler;
        }
    }
}
