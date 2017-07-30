using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VainZero.Misc;
using VainZero.Timermaid.Data.Entity;

namespace VainZero.Timermaid.ScheduleLists
{
    public enum ScheduleViewStatus
    {
        Running,
        Stopped,
        Disabled,
        Hidden,
    }

    public static class ScheduleViewStatusModule
    {
        public static ScheduleViewStatus ToView(ScheduleStatus status, bool isRunning)
        {
            switch (status)
            {
                case ScheduleStatus.Enabled:
                    return isRunning ? ScheduleViewStatus.Running : ScheduleViewStatus.Stopped;
                case ScheduleStatus.Disabled:
                    return ScheduleViewStatus.Disabled;
                case ScheduleStatus.Hidden:
                    return ScheduleViewStatus.Hidden;
                default:
                    throw new InvalidValueException(status);
            }
        }

        public static ScheduleStatus FromView(ScheduleViewStatus status)
        {
            switch (status)
            {
                case ScheduleViewStatus.Running:
                case ScheduleViewStatus.Stopped:
                    return ScheduleStatus.Enabled;
                case ScheduleViewStatus.Disabled:
                    return ScheduleStatus.Disabled;
                case ScheduleViewStatus.Hidden:
                    return ScheduleStatus.Hidden;
                default:
                    throw new InvalidValueException(status);
            }
        }

        public static ScheduleViewStatus Switch(ScheduleViewStatus status)
        {
            switch (status)
            {
                case ScheduleViewStatus.Running:
                case ScheduleViewStatus.Stopped:
                    return ScheduleViewStatus.Disabled;
                case ScheduleViewStatus.Disabled:
                    return ScheduleViewStatus.Stopped;
                default:
                    return status;
            }
        }
    }
}
