using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VainZero.Collections.ObjectModel;
using VainZero.Timermaid.Data.Entity;
using VainZero.Timermaid.Scheduling;

namespace VainZero.Timermaid.ScheduleLists
{
    public sealed class ScheduleViewList
        : BindableViewCollection<Schedule, ScheduleView>
    {
        Scheduler Scheduler { get; }

        protected override Schedule GetSource(ScheduleView target)
        {
            return target.Source;
        }

        public ScheduleViewList(Scheduler scheduler, IEnumerable<ScheduleView> targets, ICollection<Schedule> sources)
            : base(targets, sources)
        {
            Scheduler = scheduler;
        }
    }
}
