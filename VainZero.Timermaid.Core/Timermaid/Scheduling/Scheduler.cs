﻿using System;
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

        public ScheduleExecutor Executor { get; } =
            new ScheduleExecutor();

        #region Timer
        Dictionary<Schedule, IDisposable> Timers { get; } =
            new Dictionary<Schedule, IDisposable>();

        public void AddSchedule(Schedule schedule)
        {
            if (Timers.ContainsKey(schedule)) return;

            if (Executor.TryStartTimer(schedule, out var timer))
            {
                Timers.Add(schedule, timer);
            }
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
