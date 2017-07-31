using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.Timermaid.UI.Notifications
{
    public interface INotifier
    {
        void NotifyError(Exception error);
    }
}
