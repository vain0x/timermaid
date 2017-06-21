using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VainZero.Timermaid.Desktop
{
    public partial class AppNotifyIcon : Component
    {
        public ICommand ShowCommand { get; set; }
        public ICommand QuitCommand { get; set; }

        public AppNotifyIcon()
        {
            InitializeComponent();
        }

        public AppNotifyIcon(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            notifyIcon.Icon = Resources.Resource.timermaid;
            notifyIconMenuShow.Click += (sender, e) => ShowCommand?.Execute(null);
            notifyIconMenuQuit.Click += (sender, e) => QuitCommand?.Execute(null);
        }
    }
}
