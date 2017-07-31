using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Prism.Commands;
using VainZero.Timermaid.Data.Entity;
using VainZero.Timermaid.ScheduleLists;
using VainZero.Timermaid.Scheduling;
using VainZero.Timermaid.UI.Logging;
using VainZero.Timermaid.UI.Notifications;

namespace VainZero.Timermaid.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        AppMain AppMain { get; }

        AppNotifyIcon NotifyIcon { get; }

        ScheduleListWindowContainer ScheduleListWindowContainer { get; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            NotifyIcon.ShowCommand =
                new DelegateCommand(ScheduleListWindowContainer.ActivateOrCreate);
            NotifyIcon.QuitCommand =
                new DelegateCommand(Shutdown);

#if DEBUG
            ScheduleListWindowContainer.ActivateOrCreate();
#endif
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AppMain.Dispose();
            ScheduleListWindowContainer.Dispose();
            NotifyIcon.Dispose();

            base.OnExit(e);
        }

        public App()
        {
            NotifyIcon = new AppNotifyIcon(new System.ComponentModel.Container());

            var notifier = new GuiNotifier(SynchronizationContext.Current);
            notifier.ErrorNotified += (sender, error) =>
            {
                NotifyIcon.NotifyError(error);
            };

            var logger = new MemoryLogger();

            AppMain = AppMain.Load(notifier, logger);

            ScheduleListWindowContainer =
                new ScheduleListWindowContainer(() =>
                    ScheduleListView.Load(AppMain.SchedulerTask.Result, notifier, logger)
                );
        }
    }
}
