using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VainZero.Timermaid.Data.Entity;

namespace VainZero.Timermaid.Scheduling
{
    public sealed class ScheduleExecutionException
        : Exception
    {
        public Schedule Schedule { get; }

        public static string BuildMessage(Schedule schedule)
        {
            return $"スケジュール「{schedule.Name}」の実行に失敗しました。";
        }

        public ScheduleExecutionException(Schedule schedule, Exception innerException)
            : base(BuildMessage(schedule), innerException)
        {
            Schedule = schedule;
        }
    }
}
