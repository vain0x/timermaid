using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using VainZero.Collections.ObjectModel;
using VainZero.Timermaid.UI.Logging;

namespace VainZero.Timermaid.ScheduleLists
{
    public sealed class DiagnosticPage
        : BindableBase
    {
        IReadOnlyList<LogItem> items;
        public IReadOnlyList<LogItem> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public DelegateCommand<object> UpdateCommand { get; }

        ILogger Logger { get; }

        public DiagnosticPage(ILogger logger)
        {
            Logger = logger;

            UpdateCommand = new DelegateCommand<object>(_ => Items = logger.ToArray());
        }
    }
}
