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
    sealed class AutoSaveObserver<TEntity>
        : IDisposable
        where TEntity : class, INotifyPropertyChanged
    {
        BindableCollection<TEntity> Collection { get; }

        DbContext Context { get; }
        DbSet<TEntity> Set { get; }
        TimeSpan Delay { get; }

        Task LastTask { get; set; } = Task.FromResult(0);
        long revision;

        Action OnSaved { get; }
        Action<Exception> OnError { get; }

        void StartSaveTask()
        {
            var currentRevision = ++revision;
            LastTask =
                Task.Run(async () =>
                {
                    await Task.Delay(Delay).ConfigureAwait(false);
                    if (revision != currentRevision) return;

                    try
                    {
                        var count = await Context.SaveChangesAsync();
                        if (count == 0) return;
                    }
                    catch (Exception ex)
                    {
                        OnError(ex);
                        return;
                    }

                    OnSaved();
                });
        }

        public void OnAdded(object sender, TEntity entity)
        {
            Set.Add(entity);
            StartSaveTask();
        }

        public void OnRemoved(object sender, TEntity entity)
        {
            Set.Remove(entity);
            StartSaveTask();
        }

        public void OnItemChanged(object sender, TEntity entity)
        {
            StartSaveTask();
        }

        public void Attach()
        {
            foreach (var entity in Collection)
            {
                Context.Set<TEntity>().Attach(entity);
            }

            Collection.Added += OnAdded;
            Collection.Removed += OnRemoved;
            Collection.ItemChanged += OnItemChanged;
        }

        void Detach()
        {
            Collection.Added -= OnAdded;
            Collection.Removed -= OnRemoved;
            Collection.ItemChanged -= OnItemChanged;
        }

        public void Dispose()
        {
            Detach();

            try
            {
                LastTask.Wait();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        public
            AutoSaveObserver(
                BindableCollection<TEntity> collection,
                DbContext context,
                DbSet<TEntity> set,
                TimeSpan delay,
                Action onSaved,
                Action<Exception> onError
            )
        {
            Collection = collection;
            Context = context;
            Set = set;
            Delay = delay;
            OnSaved = onSaved;
            OnError = onError;
        }
    }
}
