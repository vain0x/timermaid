using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.Timermaid.UI.Logging
{
    public sealed class LogItem
    {
        public string Message { get; }
        public DateTime DateTime { get; }

        public LogItem(string message, DateTime dateTime)
        {
            Message = message;
            DateTime = dateTime;
        }
    }
}
