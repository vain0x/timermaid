using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VainZero.Timermaid.UI.Notifications
{
    public sealed class GuiNotifier
        : INotifier
    {
        SynchronizationContext SynchronizationContext { get; }

        public event EventHandler<Exception> ErrorNotified;

        public void NotifyError(Exception error)
        {
            SynchronizationContext.Post(_ =>
            {
                ErrorNotified?.Invoke(this, error);
            }, default(object));
        }

        public GuiNotifier(SynchronizationContext synchronizationContext)
        {
            SynchronizationContext = synchronizationContext;
        }
    }
}
