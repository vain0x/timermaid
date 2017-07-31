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
        DbContext Context { get; }
        DbSet<TEntity> Set { get; }
        TimeSpan Delay { get; }

        Task LastTask { get; set; } = Task.FromResult(0);
        long revision;

        Action<Exception> OnError { get; }

        void SaveAsync()
        {
            var currentRevision = ++revision;
            LastTask =
                Task.Run(async () =>
                {
                    await Task.Delay(Delay).ConfigureAwait(false);
                    if (revision != currentRevision) return;
                    try
                    {
                        await Context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        OnError(ex);
                    }
                });
        }

        public void OnAdded(object sender, TEntity entity)
        {
            Set.Add(entity);
            SaveAsync();
        }

        public void OnRemoved(object sender, TEntity entity)
        {
            Set.Remove(entity);
            SaveAsync();
        }

        public void OnItemChanged(object sender, TEntity entity)
        {
            SaveAsync();
        }

        public void Dispose()
        {
            LastTask.Wait();
        }

        sealed class Subscription
            : IDisposable
        {
            AutoSaveObserver<TEntity> Parent { get; }
            BindableCollection<TEntity> Collection { get; }

            public void Dispose()
            {
                Collection.Added -= Parent.OnAdded;
                Collection.Removed -= Parent.OnRemoved;
                Collection.ItemChanged -= Parent.OnItemChanged;
            }

            public
                Subscription(
                    AutoSaveObserver<TEntity> parent,
                    BindableCollection<TEntity> collection
                )
            {
                Parent = parent;
                Collection = collection;
            }
        }

        public IDisposable Subscribe(BindableCollection<TEntity> collection)
        {
            foreach (var entity in collection)
            {
                Context.Set<TEntity>().Attach(entity);
            }

            collection.Added += OnAdded;
            collection.Removed += OnRemoved;
            collection.ItemChanged += OnItemChanged;
            return new Subscription(this, collection);
        }

        public
            AutoSaveObserver(
                DbContext context,
                DbSet<TEntity> set,
                TimeSpan delay,
                Action<Exception> onError
            )
        {
            Context = context;
            Set = set;
            Delay = delay;
            OnError = onError;
        }
    }
}
