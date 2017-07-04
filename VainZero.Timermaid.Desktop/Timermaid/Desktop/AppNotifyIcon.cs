using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace VainZero.Timermaid.Desktop
{
    public partial class AppNotifyIcon : Component
    {
        public ICommand ShowCommand { get; set; }
        public ICommand QuitCommand { get; set; }

        public void NotifyError(Exception error)
        {
            var timeOutMilliseconds = 5000;
            notifyIcon.ShowBalloonTip(
                timeOutMilliseconds,
                error.Message,
                error.ToString(),
                ToolTipIcon.Error
            );
        }

        public new void Dispose()
        {
            notifyIcon.Visible = false;
            base.Dispose();
        }

        public AppNotifyIcon()
        {
            InitializeComponent();
        }

        public AppNotifyIcon(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            notifyIcon.Icon = Resources.Resource.timermaid;
            notifyIcon.DoubleClick += (sender, e) => ShowCommand?.Execute(null);
            notifyIconMenuShow.Click += (sender, e) => ShowCommand?.Execute(null);
            notifyIconMenuQuit.Click += (sender, e) => QuitCommand?.Execute(null);
        }
    }
}
