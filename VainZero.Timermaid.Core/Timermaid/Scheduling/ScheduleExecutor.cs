using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VainZero.Timermaid.Data.Entity;

namespace VainZero.Timermaid.Scheduling
{
    public sealed class ScheduleExecutor
    {
        public event EventHandler<Tuple<Schedule, ScheduleExecutionException>> Executed;

        public event EventHandler<ScheduleExecutionException> ExceptionThrew;

        public void Execute(Schedule schedule)
        {
            var retryCount = 5;
            var error = default(ScheduleExecutionException);

            while (true)
            {
                try
                {
                    Process.Start(schedule.FilePath, schedule.Argument);
                    break;
                }
                catch (Exception ex)
                {
                    if (retryCount > 0)
                    {
                        retryCount--;
                        Thread.Sleep(1);
                        continue;
                    }

                    error = new ScheduleExecutionException(schedule, ex);
                    break;
                }
            }

            if (error != null)
            {
                ExceptionThrew?.Invoke(this, error);
            }

            Executed?.Invoke(this, Tuple.Create(schedule, error));
        }

        public bool TryStartTimer(Schedule schedule, out Timer timer)
        {
            if (schedule.Status == ScheduleStatus.Enabled)
            {
                var dueTime = schedule.DueTime - DateTime.Now;
                if (dueTime > TimeSpan.Zero)
                {
                    timer =
                        new Timer(
                            state => Execute(schedule),
                            default(object),
                            dueTime,
                            Timeout.InfiniteTimeSpan
                        );
                    return true;
                }
            }

            timer = default(Timer);
            return false;
        }
    }
}
