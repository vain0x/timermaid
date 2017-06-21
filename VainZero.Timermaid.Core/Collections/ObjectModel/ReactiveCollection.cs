using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VainZero.Collections.ObjectModel
{
    /// <summary>
    /// Represents a collection which notifies changes via events.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReactiveCollection<T>
        : ObservableCollection<T>
    {
        public EventHandler<T> Added;
        public EventHandler<T> Removed;

        protected virtual void OnAdded(T item)
        {
            Added?.Invoke(this, item);
        }

        protected virtual void OnRemoved(T item)
        {
            Removed?.Invoke(this, item);
        }

        protected sealed override void SetItem(int index, T item)
        {
            var oldItem = this[index];

            base.SetItem(index, item);

            OnRemoved(oldItem);
        }

        protected sealed override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            OnAdded(item);
        }

        protected sealed override void RemoveItem(int index)
        {
            var item = this[index];

            base.RemoveItem(index);

            OnRemoved(item);
        }

        protected sealed override void ClearItems()
        {
            var items = this.ToArray();

            base.ClearItems();

            foreach (var item in items)
            {
                OnRemoved(item);
            }
        }

        protected sealed override void MoveItem(int oldIndex, int newIndex)
        {
            base.MoveItem(oldIndex, newIndex);
        }

        public ReactiveCollection()
        {
        }

        public ReactiveCollection(IEnumerable<T> collection)
            : base(collection)
        {
            foreach (var item in this)
            {
                OnAdded(item);
            }
        }
    }
}
