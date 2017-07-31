using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VainZero.Timermaid.Scheduling;
using VainZero.Timermaid.UI.Logging;
using VainZero.Timermaid.UI.Notifications;

namespace VainZero.Timermaid.ScheduleLists
{
    public sealed class ScheduleListView
        : IDisposable
    {
        public ScheduleListPage ScheduleListPage { get; }

        public DiagnosticPage DiagnosticPage { get; }

        public void Dispose()
        {
            ScheduleListPage.Dispose();
        }

        public ScheduleListView(ScheduleListPage scheduleListPage, DiagnosticPage diagnosticPage)
        {
            ScheduleListPage = scheduleListPage;
            DiagnosticPage = diagnosticPage;
        }

        public static ScheduleListView Load(Scheduler scheduler, INotifier notifier, ILogger logger)
        {
            return
                new ScheduleListView(
                    ScheduleListPage.Load(scheduler, notifier, logger),
                    new DiagnosticPage(logger)
                );
        }
    }
}
