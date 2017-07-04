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

            AppMain.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AppMain.Dispose();
            ScheduleListWindowContainer.Dispose();

            base.OnExit(e);
        }

        public App()
        {
            AppMain = AppMain.Load();

            ScheduleListWindowContainer =
                new ScheduleListWindowContainer(() => AppMain.SchedulerTask.Result);

            NotifyIcon =
                new AppNotifyIcon(new System.ComponentModel.Container())
                {
                    ShowCommand =
                        new DelegateCommand(ScheduleListWindowContainer.ActivateOrCreate),
                    QuitCommand =
                        new DelegateCommand(Shutdown),
                };

            AppMain.ExceptionThrew += (sender, error) =>
            {
                NotifyIcon.NotifyError(error);
            };
        }
    }
}
