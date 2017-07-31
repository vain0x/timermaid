using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VainZero.Timermaid.Scheduling;

namespace VainZero.Timermaid.ScheduleLists
{
    public sealed class ScheduleListWindowContainer
    {
        sealed class State
            : IDisposable
        {
            public ScheduleListWindow Window { get; }
            public ScheduleListView View { get; }

            public void Dispose()
            {
                View.Dispose();
            }

            public State(ScheduleListWindow window, ScheduleListView view)
            {
                Window = window;
                View = view;
            }
        }

        Func<ScheduleListView> GetView { get; }

        State CurrentOrNull { get; set; }

        public void ActivateOrCreate()
        {
            var current = CurrentOrNull;
            if (current == null)
            {
                var view = GetView();
                var window =
                    new ScheduleListWindow()
                    {
                        DataContext = view,
                    };
                current = new State(window, view);
                CurrentOrNull = current;

                window.Closed += (sender, e) =>
                {
                    current.Dispose();
                    CurrentOrNull = null;
                };

                window.Show();
            }
            else
            {
                current.Window.Activate();
            }
        }

        public void Close()
        {
            var current = CurrentOrNull;
            if (current == null) return;

            current.Window.Dispatcher.Invoke(current.Window.Close);
        }

        public void Dispose()
        {
            CurrentOrNull?.Dispose();
        }

        public ScheduleListWindowContainer(Func<ScheduleListView> getView)
        {
            GetView = getView;
        }
    }
}
