using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.Timermaid.UI.Notifications
{
    public static class NotifierExtension
    {
        public static Task<object> NotifyOnCompleted<X>(this INotifier notifier, Task<X> task)
        {
            return
                task.ContinueWith(
                    (localTask, localNotifier) =>
                    {
                        if (localTask.Status == TaskStatus.Faulted)
                        {
                            ((INotifier)localNotifier).NotifyError(localTask.Exception);
                        }
                        return default(object);
                    },
                    state: notifier
                );
        }

        public static Task<X> NotifiedBy<X>(this Task<X> task, INotifier notifier)
        {
            notifier.NotifyOnCompleted(task);
            return task;
        }

        /// <summary>
        /// Starts a task on a worker thread.
        /// </summary>
        public static Task<X> StartTask<X>(this INotifier notifier, Func<Task<X>> runAsync)
        {
            var task = Task.Run(runAsync);
            notifier.NotifyOnCompleted(task);
            return task;
        }
    }
}
