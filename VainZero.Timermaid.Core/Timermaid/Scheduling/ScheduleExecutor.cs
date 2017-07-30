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
            var error = default(ScheduleExecutionException);

            try
            {
                Process.Start(schedule.FilePath, schedule.Argument);
            }
            catch (Exception ex)
            {
                error = new ScheduleExecutionException(schedule, ex);
                ExceptionThrew?.Invoke(this, error);
            }
            finally
            {
                Executed?.Invoke(this, Tuple.Create(schedule, error));
            }
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
