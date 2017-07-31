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
using VainZero.Timermaid.UI.Notifications;

namespace VainZero.Timermaid.Scheduling
{
    public sealed class Scheduler
        : IDisposable
    {
        public BindableCollection<Schedule> Schedules { get; }

        public ScheduleExecutor Executor { get; }

        #region Timer
        Dictionary<Schedule, IDisposable> Timers { get; } =
            new Dictionary<Schedule, IDisposable>();

        public bool AddSchedule(Schedule schedule)
        {
            if (Timers.ContainsKey(schedule)) return false;

            if (Executor.TryStartTimer(schedule, out var timer))
            {
                Timers.Add(schedule, timer);
                return true;
            }

            return false;
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
                if (!AddSchedule(schedule))
                {
                    schedule.Disable();
                }
            }

            Schedules.Added += OnScheduleAdded;
            Schedules.Removed += OnScheduleRemoved;
            Schedules.ItemChanged += OnScheduleChanged;

            Executor.Executed += (sender, e) =>
            {
                e.Item1.Disable();
            };
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

        Scheduler(BindableCollection<Schedule> schedules, ScheduleExecutor executor)
        {
            Schedules = schedules;
            Executor = executor;
        }

        public static Scheduler Load(IEnumerable<Schedule> schedules, INotifier notifier)
        {
            var scheduler =
                new Scheduler(
                    new BindableCollection<Schedule>(schedules),
                    new ScheduleExecutor(notifier)
                );
            scheduler.Attach();
            return scheduler;
        }
    }
}
