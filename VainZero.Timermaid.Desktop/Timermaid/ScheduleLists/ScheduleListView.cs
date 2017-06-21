using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VainZero.Timermaid.Scheduling;

namespace VainZero.Timermaid.ScheduleLists
{
    public sealed class ScheduleListView
        : IDisposable
    {
        public ScheduleListPage ScheduleListPage { get; }

        public void Dispose()
        {
            ScheduleListPage.Dispose();
        }

        public ScheduleListView(ScheduleListPage scheduleListPage)
        {
            ScheduleListPage = scheduleListPage;
        }

        public static ScheduleListView Load(Scheduler scheduler)
        {
            return new ScheduleListView(ScheduleListPage.Load(scheduler));
        }
    }
}
