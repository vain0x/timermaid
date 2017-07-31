using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VainZero.Collections.ObjectModel;

namespace VainZero.Timermaid.UI.Logging
{
    public sealed class MemoryLogger
        : ILogger
    {
        List<LogItem> Items { get; } = new List<LogItem>();

        object LockObject { get; } = new object();

        public void Add(string message)
        {
            lock (LockObject)
            {
                Items.Add(new LogItem(message, DateTime.UtcNow));
            }
        }

        public LogItem[] ToArray()
        {
            lock (LockObject)
            {
                return Items.ToArray();
            }
        }
    }
}
