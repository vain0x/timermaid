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
using VainZero.Timermaid.UI.Logging;
using VainZero.Timermaid.UI.Notifications;

namespace VainZero.Timermaid.Scheduling
{
    public sealed class Scheduler
        : IDisposable
    {
        public BindableCollection<Schedule> Schedules { get; }

        ScheduleExecutor Executor { get; }

        ILogger Logger { get; }

        #region Timers
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

        public IReadOnlyList<Schedule> ActiveSchedules()
        {
            return
                Timers
                .Select(kv => kv.Key).Where(s => s.Status == ScheduleStatus.Enabled)
                .ToArray();
        }
        #endregion

        static void LogExecution(ILogger logger, Tuple<Schedule, ScheduleExecutionException> t)
        {
            var schedule = t.Item1;
            var adverb = t.Item2 == null ? "successfully" : "exceptionally";
            logger.Add($"Executed schedule '{schedule.Name}' {adverb}.");
        }

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
                LogExecution(Logger, e);
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

        Scheduler(BindableCollection<Schedule> schedules, ScheduleExecutor executor, ILogger logger)
        {
            Schedules = schedules;
            Executor = executor;
            Logger = logger;
        }

        public static Scheduler Load(IEnumerable<Schedule> schedules, INotifier notifier, ILogger logger)
        {
            var scheduler =
                new Scheduler(
                    new BindableCollection<Schedule>(schedules),
                    new ScheduleExecutor(notifier),
                    logger
                );
            scheduler.Attach();
            return scheduler;
        }
    }
}
