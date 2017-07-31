using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.Timermaid.UI.Logging
{
    public interface ILogger
    {
        void Add(string message);

        LogItem[] ToArray();
    }
}
