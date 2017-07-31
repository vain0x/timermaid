using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VainZero.Collections.ObjectModel;

namespace VainZero.Timermaid.Data.Entity
{
    public static class BindableCollectionExtensionFromDataEntity
    {
        public static IDisposable
            EnableAutoSave<E>(
                this BindableCollection<E> @this,
                DbContext context,
                TimeSpan delay,
                Action<Exception> onError
            )
            where E : class, INotifyPropertyChanged
        {
            return
                new AutoSaveObserver<E>(
                    context,
                    context.Set<E>(),
                    delay,
                    onError
                ).Subscribe(@this);
        }
    }
}
